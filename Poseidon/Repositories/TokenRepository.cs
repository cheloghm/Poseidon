using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Models;
using Poseidon.Interfaces;
using Poseidon.Interfaces.IRepositories;

namespace Poseidon.Repositories
{
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        public TokenRepository(IPoseidonContext context) : base(context.Tokens)
        {
        }

        public async Task<long> RemoveExpiredTokens() // Return long to indicate the count of deleted tokens
        {
            var filter = Builders<Token>.Filter.Lt(t => t.Expiration, DateTime.UtcNow);
            var deleteResult = await _collection.DeleteManyAsync(filter);
            return deleteResult.DeletedCount;  // Return the count of deleted tokens
        }
    }
}
