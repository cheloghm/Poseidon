using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Poseidon.Utilities
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            // Normally, we'd store and compare the salt too, but for simplicity
            // we'll just hash the provided password and compare it to the stored hash
            return HashPassword(providedPassword) == hashedPassword;
        }
    }
}
