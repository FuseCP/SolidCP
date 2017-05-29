using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;


namespace SolidCP.Portal.VPS2012.guacamole
{
    public class Encryption
    {

        public static string GenerateEncryptionKey()
        {
            var rj = new RijndaelManaged()
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                KeySize = 256,
                BlockSize = 256,
            };
            rj.GenerateKey();
            rj.GenerateIV();

            var key = Convert.ToBase64String(rj.Key);
            var IV = Convert.ToBase64String(rj.IV);
            string strkey = "";
            foreach (var value in key)
            {
                decimal decValue = value;
                strkey = String.Format("{0} {1}", strkey, decValue.ToString());
            }
            return String.Format("{0}:{1}", key, IV);
        }
    }
}