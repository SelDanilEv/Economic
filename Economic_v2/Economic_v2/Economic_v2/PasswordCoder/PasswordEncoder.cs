using System;
using System.Security.Cryptography;
using System.Text;

namespace Economic_v2.PasswordCoder
{
    internal class PasswordCoder
    {
        public static string GetHash(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = Encoding.UTF8.GetBytes(password);
                byte[] generatedHash = sha1.ComputeHash(hash);
                string generatedHashString = Convert.ToBase64String(generatedHash);

                return generatedHashString;
            }
        }
    }
}
