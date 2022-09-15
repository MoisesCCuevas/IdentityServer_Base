using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace AuthenticationSystem.Quickstart
{
    public class Encrypt
    {
        public string PasswordSaltInBase64()
        {
            var salt = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string PasswordToHashBase64(string plaintextPassword, string storedPasswordSaltBase64)
        {
            var salt = Convert.FromBase64String(storedPasswordSaltBase64);
            var bytearray = KeyDerivation.Pbkdf2(plaintextPassword, salt, KeyDerivationPrf.HMACSHA512, 50000, 24);
            return Convert.ToBase64String(bytearray);
        }

        public bool PasswordValidation(string storedPasswordHashBase64, string storedPasswordSaltBase64, string plaintextToValidate)
        {
            return storedPasswordHashBase64.Equals(PasswordToHashBase64(plaintextToValidate, storedPasswordSaltBase64));
        }
    }
}
