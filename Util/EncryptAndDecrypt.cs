using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.WebPages;
namespace Util
{
    public class EncryptAndDecrypt
    {
        public static string Encrypt(string ToBeEncrypted)
        {
            if (ToBeEncrypted == null) throw new ArgumentNullException("ToBeEncrypted");

            //Encrypting...
            var data = Encoding.Unicode.GetBytes(ToBeEncrypted);
            byte[] encrypted = ProtectedData.Protect(data, null,DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string ToBeDecrypted)
        {
            if (ToBeDecrypted == null) throw new ArgumentNullException("ToBeDecrypted");
            byte[] data = Convert.FromBase64String(ToBeDecrypted);
            byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }

    }
}
