using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.OpenSearch;
using System;

namespace Poseidon.Extensions
{
    public static class SerilogExtensions
    {
        public static void ConfigureSerilog(this IHostBuilder hostBuilder)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "poseidon-logs-{0:yyyy.MM.dd}",
                    TypeName = null, // Optional: modern OpenSearch versions don’t use type names
                    ModifyConnectionSettings = x => x.BasicAuthentication("username", "password") // Add if needed
                })
                .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
