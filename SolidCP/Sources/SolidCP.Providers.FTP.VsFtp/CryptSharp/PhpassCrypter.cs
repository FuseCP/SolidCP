#region License
/*
CryptSharp
Copyright (c) 2013-2014 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CryptSharp.Internal;
using CryptSharp.Utility;

namespace CryptSharp
{
    /// <summary>
    /// PHPass crypt. Used by WordPress. Variants support phpBB and Drupal 7+.
    /// </summary>
    public class PhpassCrypter : Crypter
    {
        const int MinRounds = 7;
        const int MaxRounds = 30;

        static CrypterOptions _properties = new CrypterOptions()
        {
            { CrypterProperty.MinRounds, MinRounds },
            { CrypterProperty.MaxRounds, MaxRounds }
        }.MakeReadOnly();

        static Regex _regex = new Regex(Regex, RegexOptions.CultureInvariant);

        /// <inheritdoc />
        public override string GenerateSalt(CrypterOptions options)
        {
            Check.Null("options", options);

            int rounds = options.GetValue(CrypterOption.Rounds, 14);
            Check.Range("CrypterOption.Rounds", rounds, MinRounds, MaxRounds);

            string prefix;
            switch (options.GetValue(CrypterOption.Variant, PhpassCrypterVariant.Standard))
            {
                case PhpassCrypterVariant.Standard: prefix = "$P$"; break;
                case PhpassCrypterVariant.Phpbb: prefix = "$H$"; break;
                case PhpassCrypterVariant.Drupal: prefix = "$S$"; break;
                default: throw Exceptions.ArgumentOutOfRange("CrypterOption.Variant", "Unknown variant.");
            }

            return prefix
                + Base64Encoding.UnixMD5.GetChar(rounds)
                + Base64Encoding.UnixMD5.GetString(Security.GenerateRandomBytes(6));
        }

        /// <inheritdoc />
        public override bool CanCrypt(string salt)
        {
            Check.Null("salt", salt);

            return salt.StartsWith("$P$")
                || salt.StartsWith("$H$")
                || salt.StartsWith("$S$");
        }

        /// <inheritdoc />
        public override string Crypt(byte[] password, string salt)
        {
            Check.Null("password", password);
            Check.Null("salt", salt);

            Match match = _regex.Match(salt);
            if (!match.Success) { throw Exceptions.Argument("salt", "Invalid salt."); }

            byte[] saltBytes = null, crypt = null;
            try
            {
                string roundsString = match.Groups["rounds"].Value;
                int rounds = Base64Encoding.UnixMD5.GetValue(roundsString[0]);
                if (rounds < MinRounds || rounds > MaxRounds) { throw Exceptions.ArgumentOutOfRange("salt", "Invalid number of rounds."); }

                string prefixString = match.Groups["prefix"].Value;
                bool sha512 = prefixString == "$S$";

                string saltString = match.Groups["salt"].Value;
                saltBytes = Encoding.ASCII.GetBytes(saltString);

                HashAlgorithm A;
                if (sha512)
                {
                    A = System.Security.Cryptography.SHA512.Create();
                }
                else
                {
                    A = System.Security.Cryptography.MD5.Create();
                }

                crypt = Crypt(password, saltBytes, rounds, A);

                string hashString = Base64Encoding.UnixMD5.GetString(crypt);
                if (sha512)
                {
                    hashString = hashString.Substring(0, 43);
                }

                string result = prefixString
                    + roundsString
                    + saltString
                    + hashString;
                return result;
            }
            finally
            {
                Security.Clear(saltBytes);
                Security.Clear(crypt);
            }
        }

        byte[] Crypt(byte[] key, byte[] salt, int rounds, HashAlgorithm A)
        {
            byte[] H = null;

            try
            {
                A.Initialize();
                AddToDigest(A, salt);
                AddToDigest(A, key);
                FinishDigest(A);

                H = (byte[])A.Hash.Clone();

                for (int i = 0; i < (1 << rounds); i++)
                {
                    A.Initialize();
                    AddToDigest(A, H);
                    AddToDigest(A, key);
                    FinishDigest(A);

                    Array.Copy(A.Hash, H, H.Length);
                }

                return (byte[])H.Clone();
            }
            finally
            {
                A.Clear();
                Security.Clear(H);
            }
        }

        void AddToDigest(HashAlgorithm algorithm, byte[] buffer)
        {
            algorithm.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
        }

        void FinishDigest(HashAlgorithm algorithm)
        {
            algorithm.TransformFinalBlock(new byte[0], 0, 0);
        }

        /// <inheritdoc />
        public override CrypterOptions Properties
        {
            get { return _properties; }
        }

        static string Regex
        {
            get { return @"\A(?<prefix>\$[PHS]\$)(?<rounds>[A-Za-z0-9./])(?<salt>[A-Za-z0-9./]{8})(?<crypt>[A-Za-z0-9./]{22,86})?\z"; }
        }
    }
}
