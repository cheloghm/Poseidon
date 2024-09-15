using System.Security.Claims;

namespace Poseidon.Interfaces.IUtility
{
    public interface IJwtUtility
    {
        string GenerateJwtToken(string userId);
        ClaimsPrincipal ValidateJwtToken(string token);
    }
}
