using Dev.Business.Interfaces;
using Dev.Data.Context;
using Dev.Data.Repository;

namespace Dev.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ApiDbContext>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            return services;
        }
    }
}
