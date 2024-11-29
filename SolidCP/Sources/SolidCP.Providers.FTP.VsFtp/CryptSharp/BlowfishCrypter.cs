#region License
/*
CryptSharp
Copyright (c) 2010, 2013 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

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

using System;
using System.Text.RegularExpressions;
using CryptSharp.Internal;
using CryptSharp.Utility;

namespace CryptSharp
{
    /// <summary>
    /// Blowfish crypt, sometimes called BCrypt. A very good choice.
    /// </summary>
    public class BlowfishCrypter : Crypter
    {
        const int MaxPasswordLength = 72;
        const int MinRounds = 4;
        const int MaxRounds = 31;

        static CrypterOptions _properties = new CrypterOptions()
        {
            { CrypterProperty.MaxPasswordLength, MaxPasswordLength },
            { CrypterProperty.MinRounds, MinRounds },
            { CrypterProperty.MaxRounds, MaxRounds }
        }.MakeReadOnly();

        static Regex _regex = new Regex(Regex, RegexOptions.CultureInvariant);

        /// <inheritdoc />
        public override string GenerateSalt(CrypterOptions options)
        {
            Check.Null("options", options);

            int rounds = options.GetValue(CrypterOption.Rounds, 6);
            Check.Range("CrypterOption.Rounds", rounds, MinRounds, MaxRounds);

            string prefix;
            switch (options.GetValue(CrypterOption.Variant, BlowfishCrypterVariant.Unspecified))
            {
                case BlowfishCrypterVariant.Unspecified: prefix = "$2a$"; break;
                case BlowfishCrypterVariant.Compatible: prefix = "$2x$"; break;
                case BlowfishCrypterVariant.Corrected: prefix = "$2y$"; break;
                default: throw Exceptions.ArgumentOutOfRange("CrypterOption.Variant", "Unknown variant.");
            }

            return prefix
                + rounds.ToString("00") + '$'
                + Base64Encoding.Blowfish.GetString(Security.GenerateRandomBytes(16));
        }

        /// <inheritdoc />
        public override bool CanCrypt(string salt)
        {
            Check.Null("salt", salt);

            return salt.StartsWith("$2a$")
                || salt.StartsWith("$2x$")
                || salt.StartsWith("$2y$");
        }

        /// <inheritdoc />
        public override string Crypt(byte[] password, string salt)
        {
            Check.Null("password", password);
            Check.Null("salt", salt);

            Match match = _regex.Match(salt);
            if (!match.Success) { throw Exceptions.Argument("salt", "Invalid salt."); }

            byte[] saltBytes = null, formattedKey = null, crypt = null;
            try
            {
                string prefixString = match.Groups["prefix"].Value;
                bool compatible = prefixString == "$2x$";

                int rounds = int.Parse(match.Groups["rounds"].Value);
                if (rounds < MinRounds || rounds > MaxRounds) { throw Exceptions.ArgumentOutOfRange("salt", "Invalid number of rounds."); }
                saltBytes = Base64Encoding.Blowfish.GetBytes(match.Groups["salt"].Value);

                formattedKey = FormatKey(password);
                crypt = BlowfishCipher.BCrypt(formattedKey, saltBytes, rounds,
                                              compatible
                                                ? EksBlowfishKeyExpansionFlags.EmulateCryptBlowfishSignExtensionBug
                                                : EksBlowfishKeyExpansionFlags.None);

                string result = string.Format("{0}{1}${2}{3}", prefixString, rounds.ToString("00"),
                    Base64Encoding.Blowfish.GetString(saltBytes),
                    Base64Encoding.Blowfish.GetString(crypt));
                return result;
            }
            finally
            {
                Security.Clear(saltBytes);
                Security.Clear(formattedKey);
                Security.Clear(crypt);
            }
        }

        byte[] FormatKey(byte[] key)
        {
            // In my recent investigations using PHP to generate 8-bit test vectors, I found
            // that PHP (and presumably other implementations based on crypt_blowfish) terminates
            // when it encounters a null character. Not surprising from C code really, but important
            // to handle.
            int length = ByteArray.NullTerminatedLength(key, MaxPasswordLength);

            // For keys under 72 bytes, a null terminator is vital for compatibility.
            byte[] formattedKey = new byte[Math.Min(length + 1, MaxPasswordLength)];
            Array.Copy(key, formattedKey, length);

            return formattedKey;
        }

        /// <inheritdoc />
        public override CrypterOptions Properties
        {
            get { return _properties; }
        }

        static string Regex
        {
            get { return @"\A(?<prefix>\$2[axy]\$)(?<rounds>[0-9]{2})\$(?<salt>[A-Za-z0-9./]{22})(?<crypt>[A-Za-z0-9./]{"
                + ((BlowfishCipher.BCryptLength * 8 + 5) / 6).ToString() + @"})?\z"; }
        }
    }
}
