using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Orchestration.Utilities
{
    public class PasswordHelper
    {
        public static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using var sha256 = SHA256.Create();
            var combined = salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
            return sha256.ComputeHash(combined);
        }
        public static bool Verify(string password, byte[] salt, byte[] storedHash)
        {
            var hash = HashPassword(password, salt);
            return hash.SequenceEqual(storedHash);
        }
    }
}
