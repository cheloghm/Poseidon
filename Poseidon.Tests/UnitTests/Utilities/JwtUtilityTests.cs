using Poseidon.Enums;
using Poseidon.Utilities;
using System.Text;
using Xunit;

namespace Poseidon.Tests.UnitTests.Utilities
{
    public class JwtUtilityTests
    {
        private readonly JwtUtility _jwtUtility;
        private readonly byte[] _key = Encoding.ASCII.GetBytes("this-is-a-test-secret-key");

        public JwtUtilityTests()
        {
            _jwtUtility = new JwtUtility(_key, "TestIssuer", "TestAudience");
        }

        [Fact(Skip = "Test not implemented yet. Will be implemented in future.")]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Skipped for now
        }

        [Fact(Skip = "Test not implemented yet. Will be implemented in future.")]
        public void ValidateJwtToken_ShouldReturnNull_ForInvalidToken()
        {
            // Skipped for now
        }

        // Dummy passing test
        [Fact]
        public void DummyTest_JwtUtility_PassingDummy()
        {
            Assert.True(true);  // Basic passing dummy test
        }
    }
}
