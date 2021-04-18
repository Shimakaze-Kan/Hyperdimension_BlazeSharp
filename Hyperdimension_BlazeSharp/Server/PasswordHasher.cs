using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Hyperdimension_BlazeSharp.Server
{
    public class PasswordHasher
    {
        public static string Encrypt(string password, byte[] predefinedSalt = null)
        {
            byte[] salt;

            if (predefinedSalt is not null)
            {
                salt = predefinedSalt;
            }
            else
            {
                salt = new byte[128 / 8];
                RandomNumberGenerator.Create().GetBytes(salt);
            }

            var hashed = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashed)}";
        }

        public static bool Verify(string passwordToBeChecked, string hashedPassword)
        {
            var salt = Convert.FromBase64String(hashedPassword.Split('.')[0]);

            return string.Equals(Encrypt(passwordToBeChecked, salt), hashedPassword);
        }
    }
}
