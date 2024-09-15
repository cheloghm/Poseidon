using System.Threading.Tasks;

namespace Poseidon.Interfaces
{
    public interface ITokenRepository
    {
        Task RemoveExpiredTokens();
    }
}
