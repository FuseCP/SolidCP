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

using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CryptSharp
{
    /// <summary>
    /// SHA256 crypt. A reasonable choice if you cannot use Blowfish crypt for policy reasons.
    /// </summary>
    public class Sha256Crypter : ShaCrypter
    {
        static readonly Regex _regex = CreateDefaultRegex("$5$", 43);

        protected override HashAlgorithm CreateHashAlgorithm()
        {
            return System.Security.Cryptography.SHA256.Create();
        }

        protected override int[] GetCryptPermutation()
        {
            return new[]
            { 
                20, 10, 0,
                11, 1, 21,
                2, 22, 12,
                23, 13, 3,
                14, 4, 24,
                5, 25, 15,
                26, 16, 6,
                17, 7, 27,
                8, 28, 18,
                29, 19, 9,
                30, 31
            };
        }

        protected override Regex GetRegex()
        {
            return _regex;
        }

        protected override string CryptPrefix
        {
            get { return "$5$"; }
        }
    }
}
