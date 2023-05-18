using AtomConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Services
{
    public class AES
    {
        public static string? Decrypt(string text)
        {
            try
            {
                string key = PrivateConfig.AesKey;
                var bytetext = Convert.FromBase64String(text);
                var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;
                byte[] decryptedBytes = aes.DecryptCbc(bytetext, aes.IV, aes.Padding);
               return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }
            return null;
        }

        public static string? Encrypt(string text)
        {
            try
            {
                string key = PrivateConfig.AesKey;
                var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(key);
                var bytetext = Encoding.UTF8.GetBytes(text);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;
                byte[] decryptedBytes = aes.EncryptCbc(bytetext, aes.IV);
               return Convert.ToBase64String(decryptedBytes);
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
