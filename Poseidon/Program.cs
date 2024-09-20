using Poseidon.Extensions;
using Poseidon.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog via extension method
builder.Host.ConfigureSerilog();

// Add services to the container.
builder.Services.AddMongoDb(builder.Configuration);

// Register all services (UserService, PassengerService, etc.)
builder.Services.AddServices();

// Register PasswordHash
builder.Services.PasswordHash();

// Register all repositories (UserRepository, PassengerRepository, etc.)
builder.Services.AddRepositories();

// Register JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add Swagger with JWT Auth
builder.Services.AddSwaggerWithJwtAuth();

// Add AutoMapper Profiles
builder.Services.AddAutoMapperProfiles();

// Add background tasks
builder.Services.AddBackgroundTasks();

// Add EventHandlers
builder.Services.AddEventHandlers();

// Add MemoryCache for rate limiting
builder.Services.AddMemoryCache();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck("Liveness", () => HealthCheckResult.Healthy("Liveness probe passed"))
    .AddCheck("Readiness", () => HealthCheckResult.Healthy("Readiness probe passed"));

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    c.RoutePrefix = string.Empty;  // Swagger UI will be served at the root URL
});

app.UseHttpsRedirection();

// Use Rate Limiting Middleware here
app.UseMiddleware<RateLimitingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoints for Kubernetes probes
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
