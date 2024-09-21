using DotNetEnv; // Import DotNetEnv
using Poseidon.Extensions;
using Poseidon.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Poseidon.Interfaces.IUtility;
using Poseidon.Utilities;
using System.Text;
using Poseidon.Config;

var builder = WebApplication.CreateBuilder(args);

// Load the .env file for local development
Env.Load();

// Configure Serilog via extension method
builder.Host.ConfigureSerilog();

// Get sensitive values from environment variables
string mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
string mongoDbName = Environment.GetEnvironmentVariable("MONGO_DB_NAME");
string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
string jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
string jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

// Set MongoDB config in-memory (override appsettings.json)
builder.Services.Configure<DatabaseConfig>(options =>
{
    options.ConnectionString = mongoConnectionString;
    options.DatabaseName = mongoDbName;
});

// Set up JWT Authentication with environment variables
builder.Services.AddSingleton<IJwtUtility>(sp => new JwtUtility(Encoding.UTF8.GetBytes(jwtKey), jwtIssuer, jwtAudience));

// Add services to the container.
builder.Services.AddMongoDb(builder.Configuration);  // MongoDB setup remains as-is, but using env variables
builder.Services.AddServices();
builder.Services.PasswordHash();
builder.Services.AddRepositories();
builder.Services.AddJwtAuthentication(builder.Configuration); // This will still use the Configuration object
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Use Swagger middleware
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
