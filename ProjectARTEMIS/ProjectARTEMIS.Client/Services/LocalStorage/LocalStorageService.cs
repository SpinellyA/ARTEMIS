using Microsoft.JSInterop;
using System.Text.Json;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly bool _isPrerendering;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _isPrerendering = jsRuntime is not IJSInProcessRuntime;
    }

    public async ValueTask SetAsync<T>(string key, T value)
    {
        if (_isPrerendering) return;

        if (value is null) { await RemoveAsync(key); return; }

        var json = JsonSerializer.Serialize(value, JsonOptions);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        if (_isPrerendering) return default;

        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        if (string.IsNullOrWhiteSpace(json)) return default;
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public ValueTask RemoveAsync(string key)
    {
        if (_isPrerendering) return ValueTask.CompletedTask;
        return _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public ValueTask ClearAsync()
    {
        if (_isPrerendering) return ValueTask.CompletedTask;
        return _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }

    public async ValueTask<bool> ContainsKeyAsync(string key)
    {
        if (_isPrerendering) return false;
        var value = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        return value is not null;
    }

    public async ValueTask<int> LengthAsync()
    {
        if (_isPrerendering) return 0;
        return await _jsRuntime.InvokeAsync<int>("eval", "localStorage.length");
    }

    public async ValueTask<string?> KeyAsync(int index)
    {
        if (_isPrerendering) return null;
        return await _jsRuntime.InvokeAsync<string?>("localStorage.key", index);
    }
}