using Dev.Api.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Dev.Api.Extensions
{
    // Binder personalizado para envio de IFormFile e ViewModel dentro de um FormData compatível com .NET Core 3.1 ou superior (system.text.json)
    public class ProductModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = true
            };

            var productImageDto = JsonSerializer.Deserialize<ProductImageDto>(bindingContext.ValueProvider.GetValue("product").FirstOrDefault(), serializeOptions);
            productImageDto.ImageUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.FirstOrDefault();

            bindingContext.Result = ModelBindingResult.Success(productImageDto);
            return Task.CompletedTask;
        }
    }
}
