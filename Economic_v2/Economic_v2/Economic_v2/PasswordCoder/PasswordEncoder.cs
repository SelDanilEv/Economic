using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
