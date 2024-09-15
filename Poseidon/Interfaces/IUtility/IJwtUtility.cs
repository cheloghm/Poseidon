using Poseidon.Enums;
using System.Security.Claims;

namespace Poseidon.Interfaces.IUtility
{
    public interface IJwtUtility
    {
        string GenerateJwtToken(string userId, Role role);
        ClaimsPrincipal ValidateJwtToken(string token);
    }
}
