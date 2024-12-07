#region License
/*
CryptSharp
Copyright (c) 2011 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CryptSharp.Internal;

namespace CryptSharp.Utility
{
    /// <summary>
    /// Provides comparison methods resistant to timing attack.
    /// </summary>
    public static class SecureComparison
    {
        /// <summary>
        /// Compares two strings in a timing-insensitive manner.
        /// </summary>
        /// <param name="potentialAttackerSuppliedString">The string controlled by a potential attacker.</param>
        /// <param name="referenceString">The string not controlled by a potential attacker.</param>
        /// <returns><c>true</c> if the strings are equal.</returns>
        /// <remarks>
        ///     If the reference string is zero-length, this method does not protect it against timing attacks.
        ///     If the reference string is extremely long, memory caching effects may reveal that fact.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool Equals(string potentialAttackerSuppliedString, string referenceString)
        {
            Check.Null("potentialAttackerSuppliedString", potentialAttackerSuppliedString);
            Check.Null("referenceString", referenceString);

            if (referenceString.Length == 0)
            {
                return potentialAttackerSuppliedString.Length == 0;
            }

            int attackLength = potentialAttackerSuppliedString.Length;
            int referenceLength = referenceString.Length;

            int differences = attackLength ^ referenceLength;
            for (int i = 0; i < potentialAttackerSuppliedString.Length; i++)
            {
                differences |= potentialAttackerSuppliedString[i] ^ referenceString[i % referenceString.Length];
            }
            return differences == 0;
        }
    }
}
