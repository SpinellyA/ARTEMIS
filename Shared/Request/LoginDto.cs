
public class LoginDto
{
    public string Token { get; set; } = string.Empty;
}
public record RequestWhitelistDto
{
    public Guid UserId { get; set; }
    public string RealName { get; set; } = string.Empty;
    public Guid SchoolId { get; set; }
    public string FacebookUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class WhitelistApplicationDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string FacebookUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public record AcceptWhitelistRequestDto
{
    public Guid WhitelistRequestId { get; set; }
}