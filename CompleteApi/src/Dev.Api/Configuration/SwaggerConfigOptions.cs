using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dev.Api.Configuration
{
    public class SwaggerConfigOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        const string OPEN_SOURCE_LICENSE_URL = "https://opensource.org/licenses/MIT";

        public SwaggerConfigOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }

        static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
        {
            var openApiInfo = new OpenApiInfo()
            {
                Title = "Developer API",
                Version = description.ApiVersion.ToString(),
                Description = "ASP.NET Core API reference app for learning.",
                Contact = new OpenApiContact() { Name = "Romulo Portillo", Email = "rdportillo@gmail.com" },
                TermsOfService = new Uri(OPEN_SOURCE_LICENSE_URL),
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri(OPEN_SOURCE_LICENSE_URL) }
            };

            if(description.IsDeprecated)
            {
                openApiInfo.Description = "This version is deprecated!";
            }

            return openApiInfo;
        }
    }
}
