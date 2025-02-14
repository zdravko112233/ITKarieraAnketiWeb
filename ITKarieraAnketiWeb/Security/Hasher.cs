using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace ITKarieraAnketiWeb.Security
{
    public class Hasher
    {
        private static readonly ILogger<Hasher> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Hasher>();

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // 16 bytes = 128 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            _logger.LogInformation("Generated salt: {Salt}", Convert.ToBase64String(salt));
            return salt;
        }

        // (HMAC-SHA256)
        public static byte[] HashPassword(string password, byte[] salt)
        {
            _logger.LogInformation("Hashing password with salt: {Salt}", Convert.ToBase64String(salt));
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32 // 32 bytes = 256 bits
            );
            _logger.LogInformation("Generated hash: {Hash}", Convert.ToBase64String(hash));
            return hash;
        }

        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            byte[] computedHash = HashPassword(password, storedSalt);
            bool isValid = CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
            _logger.LogInformation("Password verification result: {IsValid}", isValid);
            return isValid;
        }
    }
}
