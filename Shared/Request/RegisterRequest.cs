public record RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
    public string FacebookUrl { get; set; } = string.Empty;
    public Guid SchoolId { get; set; }
    public string MessageToAdmin { get; set; } = string.Empty;
}
