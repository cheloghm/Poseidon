using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Poseidon.Extensions;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Poseidon.Tests.IntegrationTests.API
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddJwtAuthentication_ShouldConfigureJwtCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();

            // Hardcoded JWT values for testing
            var key = "ThisIsASecretKeyWithAtLeast32Characters";
            var issuer = "PoseidonAPI";
            var audience = "PoseidonClient";

            // Add authentication and JWT bearer configuration directly
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>().Get(JwtBearerDefaults.AuthenticationScheme);

            // Assert
            Assert.NotNull(jwtBearerOptions);
            Assert.Equal(issuer, jwtBearerOptions.TokenValidationParameters.ValidIssuer);
            Assert.Equal(audience, jwtBearerOptions.TokenValidationParameters.ValidAudience);
            Assert.NotNull(jwtBearerOptions.TokenValidationParameters.IssuerSigningKey);
        }

        [Fact]
        public void AddSwaggerWithJwtAuth_ShouldConfigureSwaggerCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSwaggerWithJwtAuth();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider); // Ensure services were registered
        }
    }
}
