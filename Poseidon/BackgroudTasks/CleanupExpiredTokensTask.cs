using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Poseidon.Interfaces.IBackgroundTasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poseidon.BackgroundTasks
{
    public class CleanupExpiredTokensTask : BackgroundService
    {
        private readonly ILogger<CleanupExpiredTokensTask> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CleanupExpiredTokensTask(ILogger<CleanupExpiredTokensTask> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CleanupExpiredTokensTask running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Call your service to perform cleanup here.
                        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                        await tokenService.CleanupExpiredTokens();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning up expired tokens.");
                }

                // Wait for 24 hours before running the task again.
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
