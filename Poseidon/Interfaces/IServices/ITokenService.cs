namespace Poseidon.Interfaces.IServices
{
    public interface ITokenService
    {
        Task CleanupExpiredTokens();
    }
}
