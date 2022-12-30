using Dev.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Dev.Api.Configuration
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContextEntity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
