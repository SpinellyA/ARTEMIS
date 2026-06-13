using Microsoft.EntityFrameworkCore;

namespace ProjectARTEMIS.Infrastructure.Persistence
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<School> Schools { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<PlayerProfile> PlayerProfiles { get; set; }
        public DbSet<PlayerOnlineStatus> PlayerOnlineStatuses { get; set; }
        public DbSet<PlayerProfileStatus> PlayerProfileStatuses { get; set; }
        public DbSet<SocialMediaHandle> SocialMediaHandles { get; set; }


        public DbSet<WhitelistRequest> WhitelistRequests { get; set; }
        public DbSet<WhitelistRequestStatus> WhitelistRequestStatuses { get; set; }

    }
}
