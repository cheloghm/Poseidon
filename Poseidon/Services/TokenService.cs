using System.Threading.Tasks;
using Poseidon.Interfaces;
using Poseidon.Interfaces.IBackgroundTasks;

namespace Poseidon.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task CleanupExpiredTokens()
        {
            await _tokenRepository.RemoveExpiredTokens();
        }
    }
}
