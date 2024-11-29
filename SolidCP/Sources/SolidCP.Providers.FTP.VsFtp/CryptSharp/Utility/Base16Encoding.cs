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
    /// Base-16 binary-to-text encodings.
    /// </summary>
    public static class Base16Encoding
    {
        static Base16Encoding()
        {
            Hex = new BaseEncoding("0123456789ABCDEF", true, null, ch => char.ToUpperInvariant(ch));
        }

        /// <summary>
        /// Hexadecimal base-16 uses the numbers 0-9 for 0-9, and the letters A-F for 10-15.
        /// </summary>
        public static BaseEncoding Hex
        {
            get;
            private set;
        }
    }
}
