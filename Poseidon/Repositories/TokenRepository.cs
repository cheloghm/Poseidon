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

        public async Task<long> RemoveExpiredTokens() // Return long to indicate the count of deleted tokens
        {
            var filter = Builders<Token>.Filter.Lt(t => t.Expiration, DateTime.UtcNow);
            var deleteResult = await _tokens.DeleteManyAsync(filter);
            return deleteResult.DeletedCount;  // Return the count of deleted tokens
        }
    }
}
