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

namespace CryptSharp
{
    /// <summary>
    /// LDAP password schemes.
    /// </summary>
    public enum LdapCrypterVariant
    {
        /// <summary>
        /// Salted SHA-1. This is the default.
        /// </summary>
        SSha = 0,

        /// <summary>
        /// Unsalted SHA-1. Used in htpasswd files.
        /// </summary>
        Sha = 1,

        /// <summary>
        /// Salted SHA-256.
        /// </summary>
        SSha256 = 6,

        /// <summary>
        /// Unsalted SHA-256.
        /// </summary>
        Sha256 = 7,

        /// <summary>
        /// Salted SHA-384.
        /// </summary>
        SSha384 = 8,

        /// <summary>
        /// Unsalted SHA-384.
        /// </summary>
        Sha384 = 9,

        /// <summary>
        /// Salted SHA-512.
        /// </summary>
        SSha512 = 10,

        /// <summary>
        /// Unsalted SHA-512.
        /// </summary>
        Sha512 = 11,

        /// <summary>
        /// Salted MD-5.
        /// </summary>
        SMD5 = 2,

        /// <summary>
        /// Unsalted MD-5.
        /// </summary>
        MD5 = 3,

        /// <summary>
        /// No crypt operation is performed. The password can be read easily.
        /// </summary>
        Cleartext = 4,

        /// <summary>
        /// Any crypt algorithm.
        /// 
        /// If you specify this for <see cref="CrypterOption.Variant"/>,
        /// you must also set <see cref="LdapCrypterOption.Crypter"/>
        /// and may optionally set <see cref="LdapCrypterOption.CrypterOptions"/>.
        /// </summary>
        Crypt = 5
    }
}
