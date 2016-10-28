using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Cryptography;

namespace Domain.Entities
{
    public class Password : IEquatable<Password>, IEquatable<string>
    {
        private static int saltSize = 32;
        public int Id { get; set; }
        private string _encryptedPassword { get; set; }
        private string _salt { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        public Password()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        /// <param name="password">String representation of password. This password will be hashed with inside generated salt</param>
        public Password(string password)
        {
            Value = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        /// <param name="password"></param>
        public Password(string password, string salt)
        {
            _salt = salt;
            _encryptedPassword = CreatePasswordHash(password, _salt);
        }

        /// <summary>
        /// The password itself
        /// </summary>
        [NotMapped]
        public string Value
        {
            get
            {
                return _encryptedPassword;
            }
            set
            {
                _salt = CreateSalt();
                _encryptedPassword = CreatePasswordHash(value, _salt);
            }
        }

        /// <summary>
        /// Salt, used to create current password
        /// </summary>
        [NotMapped]
        public string Salt
        {
            get
            {
                return _salt;
            }
        }


        public bool Equals(Password other)
        {
            if (other == null)
            {
                return false;
            }

            return Value == other.Value;
        }

        public bool Equals(string other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.Equals(new Password(other, Salt));
        }

        public static explicit operator string(Password pwd)
        {
            return pwd.Value;
        }

        public static explicit operator Password(string pwd)
        {
            return new Password(pwd);
        }

        private static string CreateSalt()
        {
            return CreateSalt(saltSize);
        }

        private static string CreateSalt(int size)
        {
            byte[] buff = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        private static string CreatePasswordHash(string pwd, string salt)
        {
            var pwdBytes = GetBytes(pwd);
            var saltBytes = GetBytes(salt);
            var hashedPassword = CreatePasswordHash(pwdBytes, saltBytes);
            return Convert.ToBase64String(hashedPassword);
        }

        private static byte[] CreatePasswordHash(byte[] pwd, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] fullText = pwd.Union(salt).ToArray();
            return algorithm.ComputeHash(fullText);
        }

        static byte[] GetBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }


        public class PasswordConfiguration : EntityTypeConfiguration<Password>
        {
            public PasswordConfiguration()
            {
                Property(password => password._encryptedPassword);
                Property(password => password._salt);
            }
        }
    }
}
