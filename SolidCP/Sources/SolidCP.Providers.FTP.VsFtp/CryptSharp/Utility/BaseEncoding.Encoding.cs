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
using CryptSharp.Internal;

namespace CryptSharp.Utility
{
    partial class BaseEncoding
    {
        /// <inheritdoc />
        public override int GetMaxByteCount(int charCount)
        {
            Check.Range("charCount", charCount, 0, int.MaxValue);

            return checked(charCount * BitsPerCharacter / 8);
        }

        /// <inheritdoc />
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int byteCount = GetByteCount(chars, charIndex, charCount);

            return GetBytes(chars, charIndex, charCount, bytes, byteIndex, byteCount);
        }

        /// <summary>
        /// Converts characters from their text representation to a binary representation.
        /// </summary>
        /// <param name="chars">An input array of characters.</param>
        /// <param name="charIndex">The index of the first character.</param>
        /// <param name="charCount">The number of characters to read.</param>
        /// <param name="bytes">An output array of bytes.</param>
        /// <param name="byteIndex">The index of the first byte.</param>
        /// <param name="byteCount">The number of bytes to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount)
        {
            Check.Bounds("chars", chars, charIndex, charCount);
            Check.Bounds("bytes", bytes, byteIndex, byteCount);

            Array.Clear(bytes, byteIndex, byteCount);
            int byteEnd = checked(byteIndex + byteCount);

            int bitStartOffset = 0;
            for (int i = 0; i < charCount; i ++)
            {
                byte value = (byte)GetValue(chars[i]);

                int bitEndOffset = bitStartOffset + BitsPerCharacter;
                if (MsbComesFirst)
                {
                    if (byteIndex < byteEnd)
                    {
                        bytes[byteIndex] |= BitMath.ShiftLeft(value, 8 - bitStartOffset - BitsPerCharacter);
                    }

                    if (byteIndex + 1 < byteEnd && bitEndOffset > 8)
                    {
                        bytes[byteIndex + 1] |= BitMath.ShiftLeft(value, 16 - bitStartOffset - BitsPerCharacter);
                    }
                }
                else
                {
                    if (byteIndex < byteEnd)
                    {
                        bytes[byteIndex] |= BitMath.ShiftLeft(value, bitStartOffset);
                    }

                    if (byteIndex + 1 < byteEnd && bitEndOffset > 8)
                    {
                        bytes[byteIndex + 1] |= BitMath.ShiftLeft(value, bitStartOffset - 8);
                    }
                }

                bitStartOffset = bitEndOffset;
                if (bitStartOffset >= 8)
                {
                    bitStartOffset -= 8; byteIndex ++;
                }
            }

            return byteCount;
        }

        /// <inheritdoc />
        public override int GetByteCount(char[] chars, int index, int count)
        {
            Check.Bounds("chars", chars, index, count);

            return GetMaxByteCount(count);
        }
    }
}
