
public class RequestService
{
    private readonly IUnitOfWork _uow;

    public RequestService(IUnitOfWork uow) => _uow = uow;

    public async Task RequestWhitelistAsync(RequestWhitelistDto req)
    {

    }
}

public record RequestWhitelistDto
{
    public Guid UserId { get; private set; }
    public string RealName { get; private set; } = string.Empty;
    public Guid SchoolId { get; private set; }
    public string FacebookUrl { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
}