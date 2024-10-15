using DotNetEnv;
using Poseidon.Extensions;
using Poseidon.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Poseidon.Interfaces.IUtility;
using Poseidon.Utilities;
using System.Text;
using Poseidon.Config;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Load the .env file for local development
if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

// Add configuration providers
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Bind DatabaseConfig from the "DatabaseConfig" section
builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("DatabaseConfig"));

// Configure Serilog via extension method
builder.Host.ConfigureSerilog();

// Set up JWT Authentication
builder.Services.AddSingleton<IJwtUtility>(sp =>
{
    var jwtConfig = builder.Configuration.GetSection("Jwt");
    var key = jwtConfig["Key"];
    var issuer = jwtConfig["Issuer"];
    var audience = jwtConfig["Audience"];
    return new JwtUtility(Encoding.UTF8.GetBytes(key), issuer, audience);
});

// Add services to the container
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as strings
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // Optionally, you can ignore null values
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddServices();
builder.Services.PasswordHash();
builder.Services.AddRepositories();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJwtAuth();
builder.Services.AddAutoMapperProfiles();
builder.Services.AddBackgroundTasks();
builder.Services.AddEventHandlers();
builder.Services.AddMemoryCache();
builder.Services.AddHealthChecks()
    .AddCheck("Liveness", () => HealthCheckResult.Healthy("Liveness probe passed"))
    .AddCheck("Readiness", () => HealthCheckResult.Healthy("Readiness probe passed"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Poseidon API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = (check) => check.Name == "Liveness",
    ResponseWriter = async (context, report) =>
    {
        await context.Response.WriteAsync("Liveness probe: OK!");
    }
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = (check) => check.Name == "Readiness",
    ResponseWriter = async (context, report) =>
    {
        await context.Response.WriteAsync("Readiness probe: Ready to go!");
    }
});

app.MapControllers();

app.Run();
