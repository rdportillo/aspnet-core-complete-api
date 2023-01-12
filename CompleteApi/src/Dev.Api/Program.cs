using Dev.Api.Configuration;

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
