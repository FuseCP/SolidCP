#region License
/*
CryptSharp
Copyright (c) 2013 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
#endregion

using System.Collections.Generic;

namespace CryptSharp
{
    partial class Crypter
    {
        static Crypter()
        {
            Blowfish = new BlowfishCrypter();
            TraditionalDes = new TraditionalDesCrypter();
            ExtendedDes = new ExtendedDesCrypter();
            Ldap = new LdapCrypter(CrypterEnvironment.Default);
            MD5 = new MD5Crypter();
            Phpass = new PhpassCrypter();
            Sha256 = new Sha256Crypter();
            Sha512 = new Sha512Crypter();

            IList<Crypter> crypters = CrypterEnvironment.Default.Crypters;
            crypters.Add(Crypter.Blowfish);
            crypters.Add(Crypter.MD5);
            crypters.Add(Crypter.Phpass);
            crypters.Add(Crypter.Sha256);
            crypters.Add(Crypter.Sha512);
            crypters.Add(Crypter.Ldap);
            crypters.Add(Crypter.ExtendedDes);
            crypters.Add(Crypter.TraditionalDes);
        }

        /// <summary>
        /// Blowfish crypt, sometimes called BCrypt. A very good choice.
        /// </summary>
        public static BlowfishCrypter Blowfish
        {
            get;
            private set;
        }

        /// <summary>
        /// Traditional DES crypt.
        /// </summary>
        public static TraditionalDesCrypter TraditionalDes
        {
            get;
            private set;
        }

        /// <summary>
        /// Extended DES crypt.
        /// </summary>
        public static ExtendedDesCrypter ExtendedDes
        {
            get;
            private set;
        }

        /// <summary>
        /// LDAP schemes such as {SHA}.
        /// </summary>
        public static LdapCrypter Ldap
        {
            get;
            private set;
        }

        /// <summary>
        /// MD5 crypt, supported by nearly all systems. A variant supports Apache htpasswd files.
        /// </summary>
        public static MD5Crypter MD5
        {
            get;
            private set;
        }

        /// <summary>
        /// PHPass crypt. Used by WordPress. Variants support phpBB and Drupal 7+.
        /// </summary>
        public static PhpassCrypter Phpass
        {
            get;
            private set;
        }

        /// <summary>
        /// SHA256 crypt. A reasonable choice if you cannot use Blowfish crypt for policy reasons.
        /// </summary>
        public static Sha256Crypter Sha256
        {
            get;
            private set;
        }

        /// <summary>
        /// SHA512 crypt. A reasonable choice if you cannot use Blowfish crypt for policy reasons.
        /// </summary>
        public static Sha512Crypter Sha512
        {
            get;
            private set;
        }
    }
}
