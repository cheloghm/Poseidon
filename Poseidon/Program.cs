using Poseidon.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add MongoDB configuration and repository services
builder.Services.AddMongoDb(builder.Configuration);

// Register all services (UserService, PassengerService, etc.)
builder.Services.AddServices();

// Register all repositories (UserRepository, PassengerRepository, etc.)
builder.Services.AddRepositories();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
