using ProjectARTEMIS.Infrastructure.Persistence;

public interface IWhitelistRequestRepository : IRepository<WhitelistRequest>
{
}

public class RegistrationRequestRepository : Repository<WhitelistRequest>, IWhitelistRequestRepository
{
    public RegistrationRequestRepository(MyDbContext context) : base(context)
    {
    }
}