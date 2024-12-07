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

using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CryptSharp.Internal;
using CryptSharp.Utility;

namespace CryptSharp
{
    // See http://www.akkadia.org/drepper/SHA-crypt.txt for algorithm details.
    /// <summary>
    /// Base class for Sha256Crypter and Sha512Crypter. 
    /// </summary>
    public abstract class ShaCrypter : Crypter
    {
        const int MinRounds = 1000;
        const int MaxRounds = 999999999;

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

            return CryptPrefix
                + (rounds != null ? string.Format("rounds={0}$", rounds) : "")
                + Base64Encoding.UnixMD5.GetString(Security.GenerateRandomBytes(12));
        }

        /// <inheritdoc />
        public override bool CanCrypt(string salt)
        {
            Check.Null("salt", salt);

            return salt.StartsWith(CryptPrefix);
        }

        /// <inheritdoc />
        public override string Crypt(byte[] password, string salt)
        {
            Check.Null("password", password);
            Check.Null("salt", salt);

            Match match = GetRegex().Match(salt);
            if (!match.Success) { throw Exceptions.Argument("salt", "Invalid salt."); }

            string roundsString = match.Groups["rounds"].Value;
            bool roundsStringPresent = roundsString.Length != 0;
            int rounds = roundsStringPresent ? int.Parse(roundsString) : 5000;
            //int requestedRounds = rounds; // PHP tests indicate the rounds string is NOT preserved if the count is outside spec.
            if (rounds < MinRounds) { rounds = MinRounds; }
            if (rounds > MaxRounds) { rounds = MaxRounds; }

            byte[] saltBytes = null, formattedKey = null, truncatedSalt = null, crypt = null;
            try
            {
                string saltString = match.Groups["salt"].Value;
                saltBytes = Encoding.ASCII.GetBytes(saltString);

                formattedKey = FormatKey(password);
                truncatedSalt = ByteArray.TruncateAndCopy(saltBytes, 16);
                crypt = Crypt(formattedKey, truncatedSalt, rounds, CreateHashAlgorithm());

                string result = CryptPrefix
                    + (roundsStringPresent ? string.Format("rounds={0}$", rounds) : "")
                    + Encoding.ASCII.GetString(truncatedSalt) + '$'
                    + Base64Encoding.UnixMD5.GetString(crypt);
                return result;
            }
            finally
            {
                Security.Clear(saltBytes);
                Security.Clear(formattedKey);
                Security.Clear(truncatedSalt);
                Security.Clear(crypt);
            }
        }

        byte[] Crypt(byte[] key, byte[] salt, int rounds, HashAlgorithm A)
        {
            byte[] P = null, S = null, H = null, I = null;

            try
            {
                A.Initialize();
                AddToDigest(A, key);
                AddToDigest(A, salt);
                AddToDigest(A, key);
                FinishDigest(A);

                I = (byte[])A.Hash.Clone();

                A.Initialize();
                AddToDigest(A, key);
                AddToDigest(A, salt);

                AddToDigestRolling(A, I, 0, I.Length, key.Length);

                int length = key.Length;
                for (int i = 0; i < 31 && length != 0; i++)
                {
                    AddToDigest(A, (length & (1 << i)) != 0 ? I : key);
                    length &= ~(1 << i);
                }
                FinishDigest(A);

                H = (byte[])A.Hash.Clone();

                A.Initialize();
                for (int i = 0; i < key.Length; i++)
                {
                    AddToDigest(A, key);
                }
                FinishDigest(A);

                P = new byte[key.Length];
                CopyRolling(A.Hash, 0, A.Hash.Length, P);

                A.Initialize();
                for (int i = 0; i < 16 + H[0]; i++)
                {
                    AddToDigest(A, salt);
                }
                FinishDigest(A);

                S = new byte[salt.Length];
                CopyRolling(A.Hash, 0, A.Hash.Length, S);

                for (int i = 0; i < rounds; i++)
                {
                    A.Initialize();
                    if ((i & 1) != 0) { AddToDigest(A, P); }
                    if ((i & 1) == 0) { AddToDigest(A, H); }
                    if ((i % 3) != 0) { AddToDigest(A, S); }
                    if ((i % 7) != 0) { AddToDigest(A, P); }
                    if ((i & 1) != 0) { AddToDigest(A, H); }
                    if ((i & 1) == 0) { AddToDigest(A, P); }
                    FinishDigest(A);

                    Array.Copy(A.Hash, H, H.Length);
                }

                byte[] crypt = new byte[H.Length];
                int[] permutation = GetCryptPermutation();
                for (int i = 0; i < crypt.Length; i++)
                {
                    crypt[i] = H[permutation[i]];
                }

                return crypt;
            }
            finally
            {
                A.Clear();
                Security.Clear(P);
                Security.Clear(S);
                Security.Clear(H);
                Security.Clear(I);
            }
        }

        protected abstract HashAlgorithm CreateHashAlgorithm();

        protected abstract int[] GetCryptPermutation();

        protected abstract Regex GetRegex();

        protected static Regex CreateDefaultRegex(string cryptPrefix, int keyCharacters)
        {
            Check.Null("cryptPrefix", cryptPrefix);
            Check.Range("keyCharacters", keyCharacters, 0, int.MaxValue);

            string regex = @"\A"
                + Regex.Escape(cryptPrefix)
                + @"(rounds=(?<rounds>[0-9]{1,9})\$)?(?<salt>[A-Za-z0-9./]{1,99})(\$(?<crypt>[A-Za-z0-9./]{"
                + keyCharacters.ToString()
                + @"}))?\z";
            return new Regex(regex, RegexOptions.CultureInvariant);
        }

        static void AddToDigest(HashAlgorithm algorithm, byte[] buffer)
        {
            AddToDigest(algorithm, buffer, 0, buffer.Length);
        }

        static void AddToDigest(HashAlgorithm algorithm, byte[] buffer, int offset, int count)
        {
            algorithm.TransformBlock(buffer, offset, count, buffer, offset);
        }

        static void AddToDigestRolling(HashAlgorithm algorithm, byte[] buffer, int offset, int inputCount, int outputCount)
        {
            int count;
            for (count = 0; count < outputCount; count += inputCount)
            {
                AddToDigest(algorithm, buffer, offset, Math.Min(outputCount - count, inputCount));
            }
        }

        static void CopyRolling(byte[] buffer, int offset, int inputCount, byte[] output)
        {
            int count;
            for (count = 0; count < output.Length; count += inputCount)
            {
                Array.Copy(buffer, offset, output, count, Math.Min(output.Length - count, inputCount));
            }
        }

        static void FinishDigest(HashAlgorithm algorithm)
        {
            algorithm.TransformFinalBlock(new byte[0], 0, 0);
        }

        byte[] FormatKey(byte[] key)
        {
            int length = ByteArray.NullTerminatedLength(key, key.Length);
            return ByteArray.TruncateAndCopy(key, length);
        }

        protected abstract string CryptPrefix
        {
            get;
        }

        /// <inheritdoc />
        public override CrypterOptions Properties
        {
            get { return _properties; }
        }
    }
}
