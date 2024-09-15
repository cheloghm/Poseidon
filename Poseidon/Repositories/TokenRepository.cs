using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Models;
using Poseidon.Interfaces;

namespace Poseidon.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IMongoCollection<Token> _tokens;

        public TokenRepository(PoseidonContext context)
        {
            _tokens = context.Tokens;  // Get the Token collection from PoseidonContext
        }

        public async Task RemoveExpiredTokens()
        {
            var filter = Builders<Token>.Filter.Lt(t => t.Expiration, DateTime.UtcNow);
            await _tokens.DeleteManyAsync(filter);
        }
    }
}
