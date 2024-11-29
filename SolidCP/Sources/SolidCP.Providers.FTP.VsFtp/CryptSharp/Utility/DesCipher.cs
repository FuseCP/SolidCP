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
    // http://dhost.info/pasjagor/des/start.php?id=0 has intermediates that make debugging easy.
    /// <summary>
    /// Performs low-level encryption and decryption using the DES cipher.
    /// </summary>
    public partial class DesCipher : IDisposable
    {
        static uint R(uint a, int b) { return ((a << b) & 0xfffffff) | (a >> (28 - b)); }

        ulong[] Kex;

        DesCipher()
        {

        }

        /// <summary>
        /// Clears all memory used by the cipher.
        /// </summary>
        public void Dispose()
        {
            Security.Clear(Kex);
        }

        /// <summary>
        /// Creates a DES cipher using the provided key.
        /// </summary>
        /// <param name="key">The DES key. This must be eight bytes.</param>
        /// <returns>A DES cipher.</returns>
        public static DesCipher Create(byte[] key)
        {
            Check.Length("key", key, 8, 8);

            DesCipher cipher = new DesCipher();
            cipher.ExpandKey(key);

            return cipher;
        }

        static void CheckCipherBuffers
            (byte[] inputBuffer, int inputOffset,
             byte[] outputBuffer, int outputOffset)
        {
            Check.Bounds("inputBuffer", inputBuffer, inputOffset, 8);
            Check.Bounds("outputBuffer", outputBuffer, outputOffset, 8);
        }

        /// <summary>
        /// Enciphers eight bytes of data from one buffer and places the result in another buffer.
        /// </summary>
        /// <param name="inputBuffer">The buffer to read plaintext data from.</param>
        /// <param name="inputOffset">The offset of the first plaintext byte.</param>
        /// <param name="outputBuffer">The buffer to write enciphered data to.</param>
        /// <param name="outputOffset">The offset at which to place the first enciphered byte.</param>
        public void Encipher
			(byte[] inputBuffer, int inputOffset,
			 byte[] outputBuffer, int outputOffset)
        {
            CheckCipherBuffers(inputBuffer, inputOffset, outputBuffer, outputOffset);

            uint L, R;
            DesBegin(inputBuffer, inputOffset, out L, out R);
            for (int i = 0; i < N; i++) { DesRound(i, 0, ref L, ref R); }
            DesEnd(outputBuffer, outputOffset, ref L, ref R);
        }

        /// <summary>
        /// Reverses the encipherment of eight bytes of data from one buffer and places the result in another buffer.
        /// </summary>
        /// <param name="inputBuffer">The buffer to read enciphered data from.</param>
        /// <param name="inputOffset">The offset of the first enciphered byte.</param>
        /// <param name="outputBuffer">The buffer to write plaintext data to.</param>
        /// <param name="outputOffset">The offset at which to place the first plaintext byte.</param>
        public void Decipher
            (byte[] inputBuffer, int inputOffset,
             byte[] outputBuffer, int outputOffset)
        {
            CheckCipherBuffers(inputBuffer, inputOffset, outputBuffer, outputOffset);

            uint L, R;
            DesBegin(inputBuffer, inputOffset, out L, out R);
            for (int i = N - 1; i >= 0; i --) { DesRound(i, 0, ref L, ref R); }
            DesEnd(outputBuffer, outputOffset, ref L, ref R);
        }

        /// <summary>
        /// Crypts eight bytes of data in-place.
        /// </summary>
        /// <param name="buffer">The buffer to crypt. For traditional DES crypt, this is zero-initialized.</param>
        /// <param name="offset">The offset into the buffer.</param>
        /// <param name="iterations">The number of iterations to run.</param>
        /// <param name="salt">The salt, up to 24 bits.</param>
        public void Crypt(byte[] buffer, int offset, int iterations, int salt)
        {
            Check.Bounds("buffer", buffer, offset, 8);
            Check.Range("iterations", iterations, 0, int.MaxValue);
            Check.Range("salt", salt, 0, (1 << 24) - 1);

            ReverseSalt(ref salt);

            uint L, R;
            DesBegin(buffer, offset, out L, out R);
            for (int n = 0; n < iterations; n++)
            {
                for (int i = 0; i < N; i++) { DesRound(i, salt, ref L, ref R); }
                BitMath.Swap(ref L, ref R);
            }
            BitMath.Swap(ref L, ref R);
            DesEnd(buffer, offset, ref L, ref R);
        }

        void DesBegin(byte[] inputBuffer, int inputOffset, out uint L, out uint R)
        {
            ulong p = BitPacking.UInt64FromBEBytes(inputBuffer, inputOffset);
            ulong pp = Permute(IP, p, 64); p = 0;

            L = (uint)(pp >> 32);  R = (uint)pp;
        }

        void DesRound(int i, int reversedSalt, ref uint L, ref uint R)
        {
            uint f = F(R, Kex[i], reversedSalt);

            uint temp = R;
            R = L ^ f;
            L = temp; temp = 0;
        }

        static uint F(uint R, ulong K, int reversedSalt)
        {
            ulong ER = Permute(E, R, 32); R = 0; Salt(ref ER, reversedSalt);
            ulong KER = ER ^ K;

            uint SKER = 0;
            for (int i = 0; i < 8; i++)
            {
                int B = (int)((KER >> ((7 - i) * 6)) & 0x3f);
                int n = ((((B >> 4) & 2) | (B & 1)) << 4) | ((B >> 1) & 0xf);
                SKER |= S[i][n] << ((7 - i) * 4);
                B = 0; n = 0;
            }
            KER = 0;

            uint f = (uint)Permute(P, (ulong)SKER, 32);
            SKER = 0; return f;
        }

        void DesEnd(byte[] outputBuffer, int outputOffset, ref uint L, ref uint R)
        {
            ulong rl = (ulong)R << 32 | L; L = 0; R = 0;
            ulong rlp = Permute(FP, rl, 64); rl = 0;

            BitPacking.BEBytesFromUInt64(rlp, outputBuffer, outputOffset);
        }

        void ExpandKey(byte[] key)
        {
            Dispose();

            int[] pc1 = PC1, rotations = Rotations, pc2 = PC2;
            uint[] c = new uint[16], d = new uint[16];
            ulong[] kex = new ulong[16];

            ulong k = BitPacking.UInt64FromBEBytes(key, 0);
            ulong kp = Permute(PC1, k, 64); k = 0;

            uint cn = (uint)(kp >> 28);
            uint dn = (uint)(kp & 0xfffffff);
            kp = 0;

            for (int i = 0; i < c.Length; i++) { c[i] = cn = R(cn, rotations[i]); }
            cn = 0;

            for (int i = 0; i < c.Length; i++) { d[i] = dn = R(dn, rotations[i]); }
            dn = 0;

            for (int i = 0; i < kex.Length; i++)
            {
                ulong cd = (ulong)c[i] << 28 | d[i];
                kex[i] = Permute(PC2, cd, 56);
                cd = 0;
            }
            Security.Clear(c); Security.Clear(d);

            Kex = kex;
        }

        static ulong Permute(int[] permutation, ulong input, int inputBits)
        {
            ulong output = 0;
            for (int i = 0; i < permutation.Length; i++)
            {
                output |= ((input >> (inputBits - permutation[i])) & 1) << (permutation.Length - 1 - i);
            }
            return output;
        }

        static void ReverseSalt(ref int salt)
        {
            int outputSalt = 0;
            for (int i = 0; i < 24; i++)
            {
                outputSalt |= ((salt >> i) & 1) << (23 - i);
            }
            salt = outputSalt;
        }

        static void Salt(ref ulong E, int reversedSalt)
        {
            ulong initial = E;
            ulong H = (E >> 24) & (ulong)(uint)reversedSalt, L = E & (ulong)(uint)reversedSalt;
            E &= ~((ulong)(uint)reversedSalt << 24 | (uint)reversedSalt);
            E |= H | L << 24;
        }
    }
}
