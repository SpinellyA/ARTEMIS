
namespace ApplicationLayer {
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<WhitelistRequestService>();
            services.AddScoped<UserService>();
            services.AddScoped<SchoolService>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            return services;
        }
    }
}