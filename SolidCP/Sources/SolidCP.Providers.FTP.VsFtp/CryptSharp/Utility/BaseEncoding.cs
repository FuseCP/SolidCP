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
using System.Collections.Generic;
using System.Text;
using CryptSharp.Internal;

namespace CryptSharp.Utility
{
    /// <summary>
    /// A callback to map arbitrary characters onto the characters that can be decoded.
    /// </summary>
    /// <param name="originalCharacter">The original character.</param>
    /// <returns>the replacement character.</returns>
    public delegate char BaseEncodingDecodeFilterCallback(char originalCharacter);

    /// <summary>
    /// Performs generic binary-to-text encoding.
    /// </summary>
    public partial class BaseEncoding : Encoding
    {
        int _bitCount;
        int _bitMask;
        string _characters;
        bool _msbComesFirst;
        Dictionary<char, int> _values;
        BaseEncodingDecodeFilterCallback _decodeFilterCallback;

        /// <summary>
        /// Defines a binary-to-text encoding.
        /// </summary>
        /// <param name="characterSet">The characters of the encoding.</param>
        /// <param name="msbComesFirst">
        ///     <c>true</c> to begin with the most-significant bit of each byte.
        ///     Otherwise, the encoding begins with the least-significant bit.
        /// </param>
        public BaseEncoding(string characterSet, bool msbComesFirst)
            : this(characterSet, msbComesFirst, null, null)
        {

        }

        /// <summary>
        /// Defines a binary-to-text encoding.
        /// Additional decode characters let you add aliases, and a filter callback can be used
        /// to make decoding case-insensitive among other things.
        /// </summary>
        /// <param name="characterSet">The characters of the encoding.</param>
        /// <param name="msbComesFirst">
        ///     <c>true</c> to begin with the most-significant bit of each byte.
        ///     Otherwise, the encoding begins with the least-significant bit.
        /// </param>
        /// <param name="additionalDecodeCharacters">
        ///     A dictionary of alias characters, or <c>null</c> if no aliases are desired.
        /// </param>
        /// <param name="decodeFilterCallback">
        ///     A callback to map arbitrary characters onto the characters that can be decoded.
        /// </param>
        public BaseEncoding(string characterSet, bool msbComesFirst,
                            IDictionary<char, int> additionalDecodeCharacters,
                            BaseEncodingDecodeFilterCallback decodeFilterCallback)
        {
            Check.Null("characterSet", characterSet);

            if (!BitMath.IsPositivePowerOf2(characterSet.Length))
            {
                throw Exceptions.Argument("characterSet",
                                          "Length must be a power of 2.");
            }

            if (characterSet.Length > 256)
            {
                throw Exceptions.Argument("characterSet",
                                          "Character sets with over 256 characters are not supported.");
            }

            _bitCount = 31 - BitMath.CountLeadingZeros(characterSet.Length);
            _bitMask = (1 << _bitCount) - 1;
            _characters = characterSet;
            _msbComesFirst = msbComesFirst;
            _decodeFilterCallback = decodeFilterCallback;

            _values = additionalDecodeCharacters != null
                ? new Dictionary<char, int>(additionalDecodeCharacters)
                : new Dictionary<char, int>();
            for (int i = 0; i < characterSet.Length; i ++)
            {
                char ch = characterSet[i];
                if (_values.ContainsKey(ch))
                {
                    throw Exceptions.Argument("Duplicate characters are not supported.",
                                              "characterSet");
                }
                _values.Add(ch, (byte)i);
            }
        }

        /// <summary>
        /// Gets the value corresponding to the specified character.
        /// </summary>
        /// <param name="character">A character.</param>
        /// <returns>A value, or <c>-1</c> if the character is not part of the encoding.</returns>
        public virtual int GetValue(char character)
        {
            if (_decodeFilterCallback != null)
            {
                character = _decodeFilterCallback(character);
            }

            int value;
            return _values.TryGetValue(character, out value) ? value : -1;
        }

        /// <summary>
        /// Gets the character corresponding to the specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <returns>A character.</returns>
        public virtual char GetChar(int value)
        {
            return _characters[value & BitMask];
        }

        /// <summary>
        /// The bit mask for a single character in the current encoding.
        /// </summary>
        public int BitMask
        {
            get { return _bitMask; }
        }

        /// <summary>
        /// The number of bits per character in the current encoding.
        /// </summary>
        public int BitsPerCharacter
        {
            get { return _bitCount; }
        }

        /// <summary>
        /// <c>true</c> if the encoding begins with the most-significant bit of each byte.
        /// Otherwise, the encoding begins with the least-significant bit.
        /// </summary>
        public bool MsbComesFirst
        {
            get { return _msbComesFirst; }
        }
    }
}
