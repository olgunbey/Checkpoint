using System.Security.Cryptography;
using System.Text;

namespace Checkpoint.IdentityServer.Hash
{
    public class Hashing
    {
        public static byte[] Hash(string data)
        {
            byte[] byteSecretKey;
            using (var hash = SHA512.Create())
            {
                byteSecretKey = hash.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
            return byteSecretKey;
        }
    }
}
