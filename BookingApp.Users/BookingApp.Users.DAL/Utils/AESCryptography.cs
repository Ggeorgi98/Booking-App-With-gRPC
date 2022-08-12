using BookingApp.Users.Domain;
using System.Security.Cryptography;

namespace BookingApp.Users.DAL.Utils
{
    public class AESCryptography : IAESCryptography
    {
        private readonly string _encryptionKey;
        private readonly string _encryptionIV;

        public AESCryptography(string encryptionKey, string encryptionIV)
        {
            _encryptionKey = encryptionKey;
            _encryptionIV = encryptionIV;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return null;
            }

            if (string.IsNullOrEmpty(_encryptionKey))
            {
                throw new ArgumentNullException(paramName: _encryptionKey);
            }

            if (string.IsNullOrEmpty(_encryptionIV))
            {
                throw new ArgumentNullException(paramName: _encryptionIV);
            }

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = Convert.FromBase64String(_encryptionKey);
                aesAlg.IV = Convert.FromBase64String(_encryptionIV);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                encrypted = msEncrypt.ToArray();
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return null;
            }

            var bytes = Convert.FromBase64String(cipherText);

            if (string.IsNullOrEmpty(_encryptionKey))
            {
                throw new ArgumentNullException(paramName: _encryptionKey);
            }

            if (string.IsNullOrEmpty(_encryptionIV))
            {
                throw new ArgumentNullException(paramName: _encryptionIV);
            }

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = Convert.FromBase64String(_encryptionKey);
                aesAlg.IV = Convert.FromBase64String(_encryptionIV);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(bytes);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using (StreamReader srDecrypt = new(csDecrypt))
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }
    }
}
