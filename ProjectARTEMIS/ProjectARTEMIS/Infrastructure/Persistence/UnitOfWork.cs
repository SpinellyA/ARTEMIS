using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectARTEMIS.Infrastructure.Persistence;
using System.Data;

/// <summary>
/// Implementation of the UnitOfWork interface. Please see IUnitOfWork for a more detailed description.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _users;
    private ISchoolRepository? _schools;
    private IPlayerProfileRepository? _playerProfiles;
    private IWhitelistRequestRepository? _whitelistRequests;
    private ISocialMediaRepository? _socialMedia;

    public UnitOfWork(MyDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public ISchoolRepository Schools => _schools ??= new SchoolRepository(_context);
    public IPlayerProfileRepository PlayerProfiles => _playerProfiles ??= new PlayerProfileRepository(_context);
    public IWhitelistRequestRepository WhitelistRequests => _whitelistRequests ??= new RegistrationRequestRepository(_context);
    public ISocialMediaRepository SocialMedia => _socialMedia ??= new SocialMediaRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
    }

    public async Task CommitAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("No active transaction to commit.");

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction is null) return;

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}