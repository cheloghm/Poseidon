using Poseidon.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog via extension method
builder.Host.ConfigureSerilog();

// Add services to the container.

// Add MongoDB configuration and repository services
builder.Services.AddMongoDb(builder.Configuration);

// Register all services (UserService, PassengerService, etc.)
builder.Services.AddServices();

// Register all repositories (UserRepository, PassengerRepository, etc.)
builder.Services.AddRepositories();

// Add AutoMapper Profiles
builder.Services.AddAutoMapperProfiles();

// Add background tasks
builder.Services.AddBackgroundTasks();

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Added to handle JWT authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
