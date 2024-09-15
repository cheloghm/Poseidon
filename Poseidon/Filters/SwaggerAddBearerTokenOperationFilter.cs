using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Poseidon.Filters
{
    public class SwaggerAddBearerTokenOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authHeader = operation.Parameters?.FirstOrDefault(p => p.Name == "Authorization");

            if (authHeader != null && authHeader.Name == "Authorization")
            {
                authHeader.Description = "JWT Token (without Bearer prefix)";
            }

            // Automatically add Bearer prefix to the token for all operations
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

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
