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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<School>().HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "University of San Carlos (USC)",
                    Description = "One of the oldest and most prestigious universities in Cebu, known for its rigorous academic programs and sprawling campuses.",
                    ColorCode = "#006633" // Green and Gold
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Cebu Institute of Technology – University (CIT-U)",
                    Description = "Renowned for its excellence in engineering, technology, and producing top-notch board exam placers (Home of the Technologians).",
                    ColorCode = "#800000" // Maroon and Gold
                },
                new
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "University of San Jose – Recoletos (USJ-R)",
                    Description = "A highly respected Catholic institution known for outstanding business, IT, and engineering courses.",
                    ColorCode = "#003366" // Blue and Gold
                },
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "University of the Philippines Cebu (UP Cebu)",
                    Description = "The premier state university in the region, highly recognized for its competitive arts, design, and computer science programs.",
                    ColorCode = "#7B1113" // Maroon and Forest Green
                },
                new
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Cebu Doctors' University (CDU)",
                    Description = "A top-tier medical and health sciences institution located near North Reclamation Area, producing world-class medical professionals.",
                    ColorCode = "#FFFFFF" // Maroon and White
                }
            );
        }

    }
}
