using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Models;
using Poseidon.Utilities;
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

        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            var user = await GetByEmailAsync(email);

            if (user == null || !PasswordHasher.VerifyHashedPassword(user.Password, password))
            {
                return false; // User not found or password incorrect
            }

            return true;
        }
    }
}
