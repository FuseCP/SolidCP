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
using System.Text;
using CryptSharp.Internal;

namespace CryptSharp
{
    /// <summary>
    /// Crypts and verifies passwords. The main class for most uses of this library.
    /// </summary>
    /// 
    /// <example>
    /// <code title="Crypting a Password">
    /// using CryptSharp;
    /// 
    /// // Crypt using the Blowfish crypt ("BCrypt") algorithm.
    /// string cryptedPassword = Crypter.Blowfish.Crypt(password);
    /// </code>
    /// <code title="Checking a Password">
    /// using CryptSharp;
    /// 
    /// // Do the passwords match?
    /// // You can also check a password using the Crypt method, but this approach way is easier.
    /// bool matches = Crypter.CheckPassword(testPassword, cryptedPassword);
    /// </code>
    /// <code title="Specifying Options">
    /// using CryptSharp;
    /// 
    /// // Specify the $apr1$ Apache htpasswd variant of the MD5 crypt algorithm.
    /// string cryptedPassword = Crypter.MD5.Crypt(password, new CrypterOptions()
    ///     {
    ///         { CrypterOption.Variant, MD5CrypterVariant.Apache }
    ///     });
    /// </code>
    /// </example>
	public abstract partial class Crypter
	{
        /// <summary>
        /// Checks if the crypted password matches the given password string.
        /// </summary>
        /// <param name="password">The password string to test. Characters are UTF-8 encoded.</param>
        /// <param name="cryptedPassword">The crypted password.</param>
        /// <returns><c>true</c> if the passwords match.</returns>
        public static bool CheckPassword(string password, string cryptedPassword)
        {
            return CrypterEnvironment.Default.CheckPassword(password, cryptedPassword);
        }

        /// <summary>
        /// Checks if the crypted password matches the given password bytes.
        /// </summary>
        /// <param name="password">The password bytes to test.</param>
        /// <param name="cryptedPassword">The crypted password.</param>
        /// <returns><c>true</c> if the passwords match.</returns>
        public static bool CheckPassword(byte[] password, string cryptedPassword)
        {
            return CrypterEnvironment.Default.CheckPassword(password, cryptedPassword);
        }

        /// <summary>
        /// Searches for a crypt algorithm compatible with the specified crypted password or prefix.
        /// </summary>
        /// <param name="cryptedPassword">The crypted password or prefix.</param>
        /// <returns>A compatible crypt algorithm.</returns>
        /// <exception cref="ArgumentException">No compatible crypt algorithm was found.</exception>
        public static Crypter GetCrypter(string cryptedPassword)
        {
            return CrypterEnvironment.Default.GetCrypter(cryptedPassword);
        }

        /// <summary>
        /// Checks if the particular crypt algorithm is compatible with the salt string or crypted password.
        /// </summary>
        /// <param name="salt">The salt string or crypted password.</param>
        /// <returns><c>true</c> if the algorithm is compatible.</returns>
        public abstract bool CanCrypt(string salt);

        /// <summary>
        /// Creates a one-way password hash (crypted password) from a password string.
        /// </summary>
        /// <param name="password">The password string. Characters are UTF-8 encoded.</param>
        /// <returns>The crypted password.</returns>
        public string Crypt(string password)
        {
            return Crypt(password, GenerateSalt());
        }

        /// <summary>
        /// Creates a one-way password hash (crypted password) from a password string.
        /// Options modify the crypt operation.
        /// </summary>
        /// <param name="password">The password string. Characters are UTF-8 encoded.</param>
        /// <param name="options">Options modifying the crypt operation.</param>
        /// <returns>The crypted password.</returns>
        public string Crypt(string password, CrypterOptions options)
        {
            return Crypt(password, GenerateSalt(options));
        }

        /// <summary>
        /// Creates a one-way password hash (crypted password) from a password string and a salt string.
        /// 
        /// The salt can be produced using <see cref="Crypter.GenerateSalt(CrypterOptions)"/>.
        /// Because crypted passwords take the form <c>algorithm+salt+hash</c>, if you pass
        /// a crypted password as the salt parameter, the same algorithm and salt will be used to re-crypt the
        /// password. Since randomness comes from the salt, the same salt means the same hash, and so the
        /// same crypted password will result. Therefore, this method can both generate *and* verify crypted passwords.
        /// </summary>
        /// <param name="password">The password string. Characters are UTF-8 encoded.</param>
        /// <param name="salt">The salt string or crypted password containing a salt string.</param>
        /// <returns>The crypted password.</returns>
        public string Crypt(string password, string salt)
        {
            Check.Null("password", password);
            Check.Null("salt", salt);

            byte[] keyBytes = null;
            try
            {
                keyBytes = Encoding.UTF8.GetBytes(password);
                return Crypt(keyBytes, salt);
            }
            finally
            {
                Security.Clear(keyBytes);
            }
        }

        /// <summary>
        /// Creates a one-way password hash (crypted password) from password bytes.
        /// </summary>
        /// <param name="password">The bytes of the password.</param>
        /// <returns>The crypted password.</returns>
        public string Crypt(byte[] password)
        {
            return Crypt(password, GenerateSalt());
        }

        /// <summary>
        /// Creates a one-way password hash (crypted password) from password bytes.
        /// Options modify the crypt operation.
        /// </summary>
        /// <param name="password">The bytes of the password.</param>
        /// <param name="options">Options modifying the crypt operation.</param>
        /// <returns>The crypted password.</returns>
        public string Crypt(byte[] password, CrypterOptions options)
        {
            Check.Null("password", password);
            Check.Null("options", options);

            return Crypt(password, GenerateSalt(options));
        }

        /// <summary>
        /// Creates a one-way password hash (crypted password) from password bytes and a salt string.
        /// 
        /// The salt can be produced using <see cref="Crypter.GenerateSalt(CrypterOptions)"/>.
        /// Because crypted passwords take the form <c>algorithm+salt+hash</c>, if you pass
        /// a crypted password as the salt parameter, the same algorithm and salt will be used to re-crypt the
        /// password. Since randomness comes from the salt, the same salt means the same hash, and so the
        /// same crypted password will result. Therefore, this method can both generate *and* verify crypted passwords.
        /// </summary>
        /// <param name="password">The bytes of the password.</param>
        /// <param name="salt">The salt string or crypted password containing a salt string.</param>
        /// <returns>The crypted password.</returns>
		public abstract string Crypt(byte[] password, string salt);

        /// <summary>
        /// Generates a salt string with default options.
        /// The purpose of salt is to make dictionary attacks against a whole password database much harder,
        /// by causing the crypted password to be different even if two users have the same uncrypted password.
        /// </summary>
        /// <returns>The salt string.</returns>
        public string GenerateSalt()
        {
            return GenerateSalt(CrypterOptions.None);
        }

        /// <summary>
        /// Generates a salt string using the specified number of rounds.
        /// The purpose of salt is to make dictionary attacks against a whole password database much harder,
        /// by causing the crypted password to be different even if two users have the same uncrypted password.
        /// </summary>
        /// <returns>The salt string.</returns>
        public string GenerateSalt(int rounds)
        {
            Check.Range("rounds", rounds, 0, int.MaxValue);

            return GenerateSalt(new CrypterOptions()
            {
                { CrypterOption.Rounds, rounds }
            });
        }

        /// <summary>
        /// Generates a salt string. Options are used to modify the salt generation.
        /// The purpose of salt is to make dictionary attacks against a whole password database much harder,
        /// by causing the crypted password to be different even if two users have the same uncrypted password.
        /// 
        /// Randomness in a crypted password comes from its salt string, as do all recorded options.
        /// The same salt string, when combined with the same password, will generate the same crypted password.
        /// If the salt string differs, the same password will generate a different crypted password
        /// (crypted passwords have the form <c>algorithm+salt+hash</c>, so the salt is always carried along
        /// with the crypted password).
        /// </summary>
        /// <param name="options">Options modifying the salt generation.</param>
        /// <returns>The salt string.</returns>
        public abstract string GenerateSalt(CrypterOptions options);

        /// <summary>
        /// Properties inherent to the particular crypt algorithm. These cannot be modified.
        /// See <see cref="CrypterProperty"/> for possible keys.
        /// </summary>
        public virtual CrypterOptions Properties
        {
            get { return CrypterOptions.None; }
        }
	}
}
