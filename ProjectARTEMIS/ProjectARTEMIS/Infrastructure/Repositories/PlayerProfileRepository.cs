using ProjectARTEMIS.Infrastructure.Persistence;

public interface IPlayerProfileRepository : IRepository<PlayerProfile>
{
    Task<PlayerProfile?> GetByUserId(Guid userId);
}

public class PlayerProfileRepository : Repository<PlayerProfile>, IPlayerProfileRepository
{
    public PlayerProfileRepository(MyDbContext context) : base(context)
    {
    }
    public async Task<PlayerProfile?> GetByUserId(Guid userId) => _set.FirstOrDefault(x => x.UserId == userId);
}