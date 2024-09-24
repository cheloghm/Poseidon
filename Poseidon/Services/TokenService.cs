using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Poseidon.Interfaces;
using Poseidon.Interfaces.IServices;

namespace Poseidon.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<TokenService> _logger;

        public TokenService(ITokenRepository tokenRepository, ILogger<TokenService> logger)
        {
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        public async Task CleanupExpiredTokens()
        {
            try
            {
                _logger.LogInformation("Starting cleanup of expired tokens at {Time}", DateTime.UtcNow);

                var removedCount = await _tokenRepository.RemoveExpiredTokens();

                _logger.LogInformation("{Count} expired tokens were successfully removed at {Time}", removedCount, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cleaning up expired tokens at {Time}", DateTime.UtcNow);
            }
        }
    }
}
