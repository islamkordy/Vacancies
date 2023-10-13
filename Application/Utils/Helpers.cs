using FluentValidation.Results;
using System.Security.Cryptography;
using System.Text;

namespace Application.Utils
{
    public static class Helpers
    {
        public static string ArrangeValidationErrors(List<ValidationFailure> validationFailures)
        {
            var errors = string.Empty;
            foreach (var error in validationFailures)
            {
                errors += $"{error.ErrorMessage}\n";
            }
            return errors;
        }
        public static string GenerateRandomOtp(int length)
        {
            const string chars = "0123456789";
            var random = new Random();

            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);

        }
        public static byte[] GenerateSalt()
        {
            const int saltLength = 16;
            byte[] saltBytes = new byte[saltLength];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            return saltBytes;
        }

        public static string HashPassword(string password, byte[] saltBytes)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                byte[] saltedHashBytes = new byte[saltBytes.Length + hashBytes.Length];
                Buffer.BlockCopy(saltBytes, 0, saltedHashBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(hashBytes, 0, saltedHashBytes, saltBytes.Length, hashBytes.Length);
                string saltedHash = Convert.ToBase64String(saltedHashBytes);

                return saltedHash;
            }
        }
    }
}