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
    /// Properties inherent to particular crypt algorithms. 
    /// </summary>
    public class CrypterProperty
    {
        /// <summary>
        /// The maximum password length. Bytes beyond this length will have no effect.
        /// </summary>
        public static readonly CrypterOptionKey MaxPasswordLength = new CrypterOptionKey("MaxPasswordLength", typeof(int));

        /// <summary>
        /// The minimum number for <see cref="CrypterOption.Rounds"/>.
        /// </summary>
        public static readonly CrypterOptionKey MinRounds = new CrypterOptionKey("MinRounds", typeof(int));

        /// <summary>
        /// The maximum number for <see cref="CrypterOption.Rounds"/>.
        /// </summary>
        public static readonly CrypterOptionKey MaxRounds = new CrypterOptionKey("MaxRounds", typeof(int));

        protected CrypterProperty()
        {

        }
    }
}
