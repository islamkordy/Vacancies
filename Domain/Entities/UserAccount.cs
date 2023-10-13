using Domain.Common;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Entities
{
    public partial class UserAccount: BaseEntity
    {
        public UserAccount()
        {
            CreatedAt = DateTime.Now;
        }

        public string? Email { get; set; }
        public string? PwdHash { get; set; }
        public string? PwdSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }  

        public int? RoleId { get; set; }
        public string? FullName { get; set; }
        public string? MobileNumber { get; set; }

        public virtual UserRole? Role { get; set; }
        public void GeneretePassword(string? PasswordSalt, string? PasswordHash)
        {
            this.PwdHash= PasswordHash;
            this.PwdSalt= PasswordSalt;
        }
        public bool VerifyPassword(string providedPassword)
        {
            byte[] providedPasswordBytes = Encoding.UTF8.GetBytes(providedPassword);
            byte[] storedSaltBytes = Convert.FromBase64String(this.PwdSalt!);
            byte[] storedSaltedHashBytes = Convert.FromBase64String(this.PwdHash!);

            byte[] combinedBytes = new byte[storedSaltBytes.Length + providedPasswordBytes.Length];
            Buffer.BlockCopy(storedSaltBytes, 0, combinedBytes, 0, storedSaltBytes.Length);
            Buffer.BlockCopy(providedPasswordBytes, 0, combinedBytes, storedSaltBytes.Length, providedPasswordBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] computedHashBytes = sha256.ComputeHash(combinedBytes);

                if (computedHashBytes.Length != storedSaltedHashBytes.Length)
                {
                    return false;
                }

                for (int i = 0; i < computedHashBytes.Length; i++)
                {
                    if (computedHashBytes[i] != storedSaltedHashBytes[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void HashingPassword(string? password)
        {
            var passwordSaltBytes = GenerateSalt();
            this.PwdSalt = Convert.ToBase64String(passwordSaltBytes);
            this.PwdHash = HashPassword(password!, passwordSaltBytes);
        }
        public string HashPassword(string password, byte[] saltBytes)
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

        private byte[] GenerateSalt()
        {
            const int saltLength = 16;
            byte[] saltBytes = new byte[saltLength];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            return saltBytes;
        }
    }
}
