using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Poseidon.Config;
using Poseidon.Data;
using Poseidon.Interfaces.IServices;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Repositories;
using Poseidon.Services;
using Poseidon.Mappers;
using Poseidon.Utilities;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Poseidon.Interfaces.IUtility;
using Poseidon.BackgroundTasks;
using Poseidon.Interfaces;
using Poseidon.Events;
using Poseidon.EventHandlers;
using Poseidon.Filters;
using System;
using Poseidon.Interfaces.IEventHandlers;
using Microsoft.AspNetCore.Identity;
using Poseidon.Models;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Reflection;

namespace Poseidon.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoDbConfig = sp.GetRequiredService<IOptions<DatabaseConfig>>().Value;
                return new MongoClient(mongoDbConfig.ConnectionString);  // Uses the in-memory values from the environment variables
            });

            services.AddSingleton<PoseidonContext>();
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
                var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
                var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // Add the event to handle tokens without the "Bearer" prefix
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.ContainsKey("Authorization"))
                        {
                            var token = context.Request.Headers["Authorization"].ToString();

                            // Remove the "Bearer " prefix if it's there
                            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token.Substring("Bearer ".Length).Trim();
                            }
                            else
                            {
                                context.Token = token; // Directly use the token without "Bearer"
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddSwaggerWithJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Poseidon API", Version = "v1" });

                // Define the security scheme for JWT authentication in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter your JWT token directly, without 'Bearer' prefix.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Use the SwaggerAddBearerTokenOperationFilter to automatically handle token validation
                c.OperationFilter<SwaggerAddBearerTokenOperationFilter>();

                // Add security requirements to Swagger to ensure it knows how to handle JWT tokens
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

                // Enable XML comments in Swagger UI if you want to include detailed API documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPassengerService, PassengerService>();

            return services;
        }

        public static IServiceCollection PasswordHash(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();

            return services;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection AddBackgroundTasks(this IServiceCollection services)
        {
            services.AddHostedService<CleanupExpiredTokensTask>();
            return services;
        }

        public static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddSingleton<IEventHandler<PassengerCreatedEvent>, PassengerCreatedEventHandler>();
            services.AddSingleton<IEventHandler<UserUpdatedEvent>, UserUpdatedEventHandler>();

            return services;
        }

        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            // Register event handlers for handling specific events
            services.AddSingleton<IEventHandler<PassengerCreatedEvent>, PassengerCreatedEventHandler>();
            services.AddSingleton<IEventHandler<UserUpdatedEvent>, UserUpdatedEventHandler>();

            return services;
        }

        public static IServiceCollection AddFilters(this IServiceCollection services)
        {
            services.AddScoped<LoggingActionFilter>();

            return services;
        }
    }
}
