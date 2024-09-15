namespace Poseidon.Interfaces.IBackgroundTasks
{
    public interface ITokenService
    {
        Task CleanupExpiredTokens();
    }
}
