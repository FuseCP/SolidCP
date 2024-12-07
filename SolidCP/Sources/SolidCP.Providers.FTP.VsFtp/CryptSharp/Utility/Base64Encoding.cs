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

namespace CryptSharp.Utility
{
    /// <summary>
    /// Base-64 binary-to-text encodings.
    /// </summary>
    public static class Base64Encoding
    {
        static Base64Encoding()
        {
            Blowfish = new BaseEncoding("./ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", true);

            UnixCrypt = new BaseEncoding("./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", true);

            UnixMD5 = new BaseEncoding("./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", false);
        }

        /// <summary>
        /// Blowfish crypt orders characters differently from standard crypt, and begins encoding from
        /// the most-significant bit instead of the least-significant bit.
        /// </summary>
        public static BaseEncoding Blowfish
        {
            get;
            private set;
        }

        /// <summary>
        /// Traditional DES crypt base-64, as seen on Unix /etc/passwd, many websites, database servers, etc.
        /// </summary>
        public static BaseEncoding UnixCrypt
        {
            get;
            private set;
        }

        /// <summary>
        /// MD5, SHA256, and SHA512 crypt base-64, as seen on Unix /etc/passwd, many websites, database servers, etc.
        /// </summary>
        public static BaseEncoding UnixMD5
        {
            get;
            private set;
        }
    }
}
