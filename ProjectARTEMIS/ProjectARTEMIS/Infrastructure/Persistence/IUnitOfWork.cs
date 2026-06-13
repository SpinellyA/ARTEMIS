using System.Data;

public interface IUnitOfWork
{
    IPlayerProfileRepository PlayerProfiles { get; }
    IWhitelistRequestRepository WhitelistRequests { get; }
    ISchoolRepository Schools { get; }
    ISocialMediaRepository SocialMedia { get; }
    IUserRepository Users { get; }

    Task BeginTransactionAsync(IsolationLevel isolationLevel);
    Task CommitAsync();
    void Dispose();
    Task RollbackAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}