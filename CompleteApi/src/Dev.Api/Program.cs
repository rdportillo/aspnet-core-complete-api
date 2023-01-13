using Dev.Api.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextEntity(builder.Configuration);

builder.Services.AddIdentityConfiguration(builder.Configuration);

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Web Api configuration
builder.Services.WebApiConfig();

// Dependency Injection configuration
builder.Services.ResolveDependencies();

// Swagger configuration
builder.Services.AddSwaggerConfig();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig(provider);
} else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseApiConfiguration();

app.MapControllers();

app.Run();
