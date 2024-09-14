using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Models;
using System.Threading.Tasks;

namespace Poseidon.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PoseidonContext context) : base(context.Users)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
