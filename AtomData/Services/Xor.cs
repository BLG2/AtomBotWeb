using AtomConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Services
{
    public class Xor
    {
        public static string EncryptOrDecrypt(string text)
        {
            string key = PrivateConfig.XorKey;
            var replyRes = "";
            try
            {
                var result = new StringBuilder();
                for (int i = 0; i < text.Length; i++)
                    result.Append((char)(text[i] ^ (uint)key[i % key.Length]));
                replyRes = result.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Encrypt/Decrypt error");
            }
            return replyRes;
        }



        public static string Encrypt(string text)
        {
            string key = PrivateConfig.XorKey;
            var replyRes = "";
            try
            {
                var result = new StringBuilder();
                for (int i = 0; i < text.Length; i++)
                    result.Append((char)(text[i] ^ (uint)key[i % key.Length]));
                var encrypted = result.ToString();
                replyRes = Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypted));
            }
            catch (Exception )
            {
                throw new Exception("Encrypt/Decrypt error");
            }
            return replyRes;
        }
        public static string Decrypt(string text)
        {
            string key = PrivateConfig.XorKey;
            var replyRes = "";
            try
            {
                var fromBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(text));
                var result = new StringBuilder();
                for (int i = 0; i < fromBase64.Length; i++)
                    result.Append((char)(fromBase64[i] ^ (uint)key[i % key.Length]));
                replyRes = result.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Encrypt/Decrypt error");
            }
            return replyRes;
        }





    }
}
