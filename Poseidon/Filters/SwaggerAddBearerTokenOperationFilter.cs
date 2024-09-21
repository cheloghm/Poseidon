using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Poseidon.Filters
{
    public class SwaggerAddBearerTokenOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the operation has an "Authorization" header
            var authHeader = operation.Parameters?.FirstOrDefault(p => p.Name == "Authorization");

            // Set the header description to not require "Bearer" prefix
            if (authHeader != null && authHeader.Name == "Authorization")
            {
                authHeader.Description = "JWT Token (without Bearer prefix)";
            }

            // Automatically add the security scheme for operations requiring authorization
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            // Add the security scheme to all operations that need authentication
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Authorization",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}
