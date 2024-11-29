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
    /// Options that modify the LDAP crypt operation.
    /// </summary>
    public class LdapCrypterOption : CrypterOption
    {
        /// <summary>
        /// The crypter to use with <see cref="LdapCrypterVariant.Crypt"/>.
        /// </summary>
        public static readonly CrypterOptionKey Crypter = new CrypterOptionKey("Crypter", typeof(Crypter));

        /// <summary>
        /// The options to pass to the crypter specified by <see cref="LdapCrypterOption.Crypter"/>.
        /// </summary>
        public static readonly CrypterOptionKey CrypterOptions = new CrypterOptionKey("CrypterOptions", typeof(CrypterOptions));

        protected LdapCrypterOption()
        {

        }
    }
}
