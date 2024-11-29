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

using System.Text.RegularExpressions;
using CryptSharp.Internal;
using CryptSharp.Utility;

namespace CryptSharp
{
    /// <summary>
    /// Extended DES crypt.
    /// </summary>
    public class ExtendedDesCrypter : Crypter
    {
        const int MinRounds = 1;
        const int MaxRounds = (1 << 24) - 1;

        static readonly Regex _regex = new Regex(Regex, RegexOptions.CultureInvariant);

        static CrypterOptions _properties = new CrypterOptions()
        {
            { CrypterProperty.MinRounds, MinRounds },
            { CrypterProperty.MaxRounds, MaxRounds }
        }.MakeReadOnly();

        /// <inheritdoc />
        public override string GenerateSalt(CrypterOptions options)
        {
            Check.Null("options", options);

            int? rounds = options.GetValue<int?>(CrypterOption.Rounds);
            if (rounds != null)
            {
                Check.Range("CrypterOption.Rounds", (int)rounds, MinRounds, MaxRounds);
            }

            byte[] roundsBytes = new byte[3], saltBytes = null;
            try
            {
                BitPacking.LEBytesFromUInt24((uint)(rounds ?? 4321), roundsBytes, 0);
                saltBytes = Security.GenerateRandomBytes(3);

                return "_"
                    + Base64Encoding.UnixMD5.GetString(roundsBytes)
                    + Base64Encoding.UnixMD5.GetString(saltBytes);
            }
            finally
            {
                Security.Clear(roundsBytes);
                Security.Clear(saltBytes);
            }
        }

        /// <inheritdoc />
        public override bool CanCrypt(string salt)
        {
            Check.Null("salt", salt);

            return _regex.IsMatch(salt);
        }

        /// <inheritdoc />
        public override string Crypt(byte[] password, string salt)
        {
            Check.Null("password", password);
            Check.Null("salt", salt);

            Match match = _regex.Match(salt);
            if (!match.Success) { throw Exceptions.Argument("salt", "Invalid salt."); }

            byte[] roundsBytes = null, saltBytes = null, crypt = null, input = null;
            try
            {
                string roundsString = match.Groups["rounds"].Value;
                roundsBytes = Base64Encoding.UnixMD5.GetBytes(roundsString);
                int roundsValue = (int)BitPacking.UInt24FromLEBytes(roundsBytes, 0);

                string saltString = match.Groups["salt"].Value;
                saltBytes = Base64Encoding.UnixMD5.GetBytes(saltString);
                int saltValue = (int)BitPacking.UInt24FromLEBytes(saltBytes, 0);

                input = new byte[8];
                int length = ByteArray.NullTerminatedLength(password, password.Length);

                for (int m = 0; m < length; m += 8)
                {
                    if (m != 0)
                    {
                        using (DesCipher cipher = DesCipher.Create(input))
                        {
                            cipher.Encipher(input, 0, input, 0);
                        }
                    }

                    for (int n = 0; n < 8 && n < length - m; n++)
                    {
                        // DES Crypt ignores the high bit of every byte.
                        input[n] ^= (byte)(password[m + n] << 1);
                    }
                }

                using (DesCipher cipher = DesCipher.Create(input))
                {
                    crypt = new byte[8];
                    cipher.Crypt(crypt, 0, roundsValue, saltValue);
                }

                return "_" + roundsString + saltString + Base64Encoding.UnixCrypt.GetString(crypt);
            }
            finally
            {
                Security.Clear(roundsBytes);
                Security.Clear(saltBytes);
                Security.Clear(crypt);
                Security.Clear(input);
            }
        }

        /// <inheritdoc />
        public override CrypterOptions Properties
        {
            get { return _properties; }
        }

        static string Regex
        {
            get { return @"\A_(?<rounds>[A-Za-z0-9./]{4})(?<salt>[A-Za-z0-9./]{4})(?<hash>[A-Za-z0-9./]{11})?\z"; }
        }
    }
}
