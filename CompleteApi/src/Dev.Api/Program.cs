using Dev.Api.Configuration;
using Dev.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextEntity(builder.Configuration);

builder.Services.AddIdentityConfiguration(builder.Configuration);

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

// Web Api configuration
builder.Services.WebApiConfig();

// Dependency Injection configuration
builder.Services.ResolveDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseApiConfiguration();

app.MapControllers();

app.Run();
