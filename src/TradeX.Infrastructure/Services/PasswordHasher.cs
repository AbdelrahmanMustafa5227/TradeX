using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;

namespace TradeX.Infrastructure.Services
{
    internal class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt , 100_000 , HashAlgorithmName.SHA512 , 32);
            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool Verify(string password , string passwordHash)
        {
            var parts = passwordHash.Split('-');
            byte[] salt = Convert.FromHexString(parts[1]);
            byte[] storedHash = Convert.FromHexString(parts[0]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA512, 32);

            return CryptographicOperations.FixedTimeEquals(storedHash,inputHash);
        }
    }
}
