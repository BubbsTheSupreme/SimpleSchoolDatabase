using System;
using System.Text;
using System.Security.Cryptography;

namespace School 
{
    public class CryptoHandler
    {
        public string SaltAndHashPassword(string password)
        {
            var sha = SHA256.Create();

            byte[] salt = Encoding.Unicode.GetBytes("XO8o2fiN"); // the salt

            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length]; // creating the array for the password and salt

            for (int i = 0; i < passwordBytes.Length; i++) // copies password to the combined array
            {
                passwordWithSalt[i] = passwordBytes[i];
            }
                
            for (int i = 0; i < salt.Length; i++) // copies the salt to the combined array
            {
                passwordWithSalt[passwordBytes.Length + i] = salt[i];
            }

            return Convert.ToBase64String(sha.ComputeHash(passwordWithSalt));
        }
    }
}