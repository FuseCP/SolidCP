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

namespace CryptSharp.Utility
{
    /// <summary>
    /// Base-32 binary to text encodings.
    /// 
    /// I needed multiple variations of base-64 for the various crypt algorithms, and base-16 (hex) for test vectors,
    /// so base-32 is mostly a freebie. It's great for e-mail verifications, product keys - really anywhere you need
    /// someone to type in a randomly-generated code.
    /// </summary>
    public static class Base32Encoding
    {
        static Base32Encoding()
        {
            Crockford = new BaseEncoding("0123456789ABCDEFGHJKMNPQRSTVWXYZ", false, new Dictionary<char, int>()
                {
                    { 'O', 0 },
                    { 'I', 1 },
                    { 'L', 1 },
                    { 'U', 27 }
                }, ch => char.ToUpperInvariant(ch));

            ZBase32 = new BaseEncoding("ybndrfg8ejkmcpqxot1uwisza345h769", true, new Dictionary<char, int>()
                {
                    { '0', 16 },
                    { 'l', 18 },
                    { 'v', 19 },
                    { '2', 23 },
                }, ch => char.ToLowerInvariant(ch));
        }

        /// <summary>
        /// Crockford base-32 is somewhat traditional, but still better than the RFC 4648 standard.
        /// It is specified at http://www.crockford.com/wrmg/base32.html.
        /// </summary>
        public static BaseEncoding Crockford
        {
            get;
            private set;
        }

        /// <summary>
        /// z-base-32 is a lowercase base-32 encoding designed to be easily hand-written and read.
        /// It is specified at http://www.zer7.com/files/oss/cryptsharp/zbase32.txt.
        /// </summary>
        public static BaseEncoding ZBase32
        {
            get;
            private set;
        }
    }
}
