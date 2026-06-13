using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    public JwtAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetRawAsync("token");
        Console.WriteLine($"[JWT] Token retrieved: {(string.IsNullOrWhiteSpace(token) ? "NULL" : token[..20] + "...")}");

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetRawAsync("token", token);

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        NotifyAuthenticationStateChanged(authState);
        await authState;
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveAsync("token");

        _http.DefaultRequestHeaders.Authorization = null;

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine("[JWT] InitializeAsync called");
        var token = await _localStorage.GetRawAsync("token");
        Console.WriteLine($"[JWT] InitializeAsync token: {(string.IsNullOrWhiteSpace(token) ? "NULL" : "EXISTS")}");

        if (string.IsNullOrWhiteSpace(token))
            return;

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var payload = token.Split('.')[1];
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(
            payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')));

        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;

        return dict.Select(kvp => new Claim(MapClaimType(kvp.Key), kvp.Value.ToString()!));
    }

    private static string MapClaimType(string key) => key switch
    {
        "nameid" => ClaimTypes.NameIdentifier,
        "unique_name" => ClaimTypes.Name,
        "role" => ClaimTypes.Role,
        _ => key
    };
}