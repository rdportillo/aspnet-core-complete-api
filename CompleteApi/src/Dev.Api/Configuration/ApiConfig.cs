using Microsoft.AspNetCore.Mvc;

namespace Dev.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(p => p.AddPolicy("appCors", builder =>
            {
                var origins = new string[] {
                    "http://localhost:4200"
                };

                builder.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader();
            }));

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseCors("Development");

            return app;
        }
    }
}
