using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public static class EncryptionHelper
    {
        public static string Hash(string value)
        {
            var md5 = MD5.Create();
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = md5.ComputeHash(valueBytes);
            var hashStringBuilder = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashStringBuilder.Append(hashBytes[i].ToString("x2"));
            }

            var hashString = hashStringBuilder.ToString();
           
            return hashString;
        }

        public static byte[] StringToUtf8(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string ByteArrayToUTF8String(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}
