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

// I ported this from Bruce Schneier's C implementation.
//
// Unlike Crypter, the BlowfishCipher class does NOT automatically
// add a null terminating last byte to the key. You have to do that
// yourself if your particular application requires it (Blowfish
// crypt does).

using System;
using System.Collections.Generic;
using System.Text;
using CryptSharp.Internal;

namespace CryptSharp.Utility
{
    /// <summary>
    /// Performs low-level encryption and decryption using the Blowfish cipher.
    /// </summary>
	public partial class BlowfishCipher : IDisposable
	{
		static byte[] _zeroSalt = new byte[16];
		uint[] P; uint[][] S;

        static BlowfishCipher()
        {
            byte[] magicBytes = Encoding.ASCII.GetBytes(BCryptMagic);
            Array.Resize(ref magicBytes, (magicBytes.Length + 7) / 8 * 8);

            Magic = new uint[(magicBytes.Length + 3) / 4];
            for (int i = 0; i < Magic.Length; i++) { Magic[i] = BitPacking.UInt32FromBEBytes(magicBytes, i * 4); }
        }

        static readonly uint[] Magic;

        /// <summary>
        /// The number of bytes returned by <see cref="BlowfishCipher.BCrypt()"/>.
        /// </summary>
        public static int BCryptLength { get { return Magic.Length * 4 - 1; } }

		BlowfishCipher(uint[] p, uint[][] s)
		{
            Clone(p ?? P0, s ?? S0);
		}

        void Clone(uint[] p, uint[][] s)
        {
            P = (uint[])p.Clone();
            S = new uint[][] { (uint[])s[0].Clone(), (uint[])s[1].Clone(), (uint[])s[2].Clone(), (uint[])s[3].Clone() };
        }

        /// <summary>
        /// Clears all memory used by the cipher.
        /// </summary>
        public void Dispose()
        {
            Security.Clear(P);
            for (int i = 0; i < S.Length; i++) { Security.Clear(S[i]); }
        }

        /// <summary>
        /// Creates a Blowfish cipher using the provided key.
        /// </summary>
        /// <param name="key">The Blowfish key. This must be between 4 and 56 bytes.</param>
        /// <returns>A Blowfish cipher.</returns>
        public static BlowfishCipher Create(byte[] key)
		{
			Check.Length("key", key, 4, 56);

            BlowfishCipher fish = new BlowfishCipher(null, null);
			fish.ExpandKey(key, _zeroSalt, EksBlowfishKeyExpansionFlags.None);
			return fish;
		}

        /// <summary>
        /// Performs an Expensive Key Schedule (EKS) Blowfish key expansion and
        /// creates a Blowfish cipher using the result.
        /// </summary>
        /// <param name="key">
        ///     The key. This must be between 1 and 72 bytes.
        ///     Unlike <see cref="BlowfishCrypter"/>, this method does NOT automatically add a null byte to the key.
        /// </param>
        /// <param name="salt">The salt. This must be 16 bytes.</param>
        /// <param name="cost">
        ///     The expansion cost. This is a value between 4 and 31,
        ///     specifying the logarithm of the number of iterations.
        /// </param>
        /// <returns>A Blowfish cipher.</returns>
        public static BlowfishCipher CreateEks(byte[] key, byte[] salt, int cost)
        {
            return CreateEks(key, salt, cost, EksBlowfishKeyExpansionFlags.None);
        }

        /// <summary>
        /// Performs an Expensive Key Schedule (EKS) Blowfish key expansion and
        /// creates a Blowfish cipher using the result. Flags may modify the key expansion.
        /// </summary>
        /// <param name="key">
        ///     The key. This must be between 1 and 72 bytes.
        ///     Unlike <see cref="BlowfishCrypter"/>, this method does NOT automatically add a null byte to the key.
        /// </param>
        /// <param name="salt">The salt. This must be 16 bytes.</param>
        /// <param name="cost">
        ///     The expansion cost. This is a value between 4 and 31,
        ///     specifying the logarithm of the number of iterations.
        /// </param>
        /// <param name="flags">Flags modifying the key expansion.</param>
        /// <returns>A Blowfish cipher.</returns>
		public static BlowfishCipher CreateEks(byte[] key, byte[] salt, int cost,
                                               EksBlowfishKeyExpansionFlags flags)
		{
			Check.Length("key", key, 1, 72);
			Check.Length("salt", salt, 16, 16);
			Check.Range("cost", cost, 4, 31);

            BlowfishCipher fish = new BlowfishCipher(null, null);
            fish.ExpandKey(key, salt, flags);
			for (uint i = 1u << cost; i > 0; i --)
			{
                fish.ExpandKey(key, _zeroSalt, flags);
                fish.ExpandKey(salt, _zeroSalt, EksBlowfishKeyExpansionFlags.None);
            }
			return fish;
		}

        /// <summary>
        /// Uses the given key, salt, and cost to generate a BCrypt hash.
        /// </summary>
        /// <param name="key">
        ///     The key. This must be between 1 and 72 bytes.
        ///     Unlike <see cref="BlowfishCrypter"/>, this method does NOT automatically add a null byte to the key.
        /// </param>
        /// <param name="salt">The salt. This must be 16 bytes.</param>
        /// <param name="cost">
        ///     The expansion cost. This is a value between 4 and 31,
        ///     specifying the logarithm of the number of iterations.
        /// </param>
        /// <returns>A BCrypt hash.</returns>
        public static byte[] BCrypt(byte[] key, byte[] salt, int cost)
        {
            return BCrypt(key, salt, cost, EksBlowfishKeyExpansionFlags.None);
        }

        /// <summary>
        /// Uses the given key, salt, and cost to generate a BCrypt hash.
        /// Flags may modify the key expansion.
        /// </summary>
        /// <param name="key">
        ///     The key. This must be between 1 and 72 bytes.
        ///     Unlike <see cref="BlowfishCrypter"/>, this method does NOT automatically add a null byte to the key.
        /// </param>
        /// <param name="salt">The salt. This must be 16 bytes.</param>
        /// <param name="cost">
        ///     The expansion cost. This is a value between 4 and 31,
        ///     specifying the logarithm of the number of iterations.
        /// </param>
        /// <param name="flags">Flags modifying the key expansion.</param>
        /// <returns>A BCrypt hash.</returns>
		public static byte[] BCrypt(byte[] key, byte[] salt, int cost,
                                    EksBlowfishKeyExpansionFlags flags)
		{
            using (BlowfishCipher fish = CreateEks(key, salt, cost, flags))
            {
                return fish.BCrypt();
            }
		}
		
        /// <summary>
        /// Uses the cipher to generate a BCrypt hash.
        /// </summary>
        /// <returns>A BCrypt hash.</returns>
		public byte[] BCrypt()
		{
            uint[] magicWords = null;
            byte[] magicBytes = null;

            try
            {
                magicWords = (uint[])Magic.Clone();
                for (int j = 0; j < magicWords.Length; j += 2)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        Encipher(ref magicWords[j], ref magicWords[j + 1]);
                    }
                }

                magicBytes = new byte[magicWords.Length * 4];
                for (int i = 0; i < magicWords.Length; i++) { BitPacking.BEBytesFromUInt32(magicWords[i], magicBytes, i * 4); }
                return ByteArray.TruncateAndCopy(magicBytes, magicBytes.Length - 1);
            }
            finally
            {
                Security.Clear(magicWords);
                Security.Clear(magicBytes);
            }
		}
			
		void ExpandKey(byte[] key, byte[] salt, EksBlowfishKeyExpansionFlags flags)
		{
			uint[] p = P; uint[][] s = S;
			int i, j, k; uint data, datal, datar;
			
			j = 0;
			for (i = 0; i < p.Length; i ++)
			{
				data = 0x00000000;
				for (k = 0; k < 4; k ++)
				{
                    if ((flags & EksBlowfishKeyExpansionFlags.EmulateCryptBlowfishSignExtensionBug) != 0)
                    {
                        data = (data << 8) | (uint)(int)(sbyte)key[j];
                    }
                    else
                    {
                        data = (data << 8) | key[j];
                    }

                    j = (j + 1) % key.Length;
				}
				p[i] = p[i] ^ data;
			}

			uint saltL0 = BitPacking.UInt32FromBEBytes(salt, 0);
			uint saltR0 = BitPacking.UInt32FromBEBytes(salt, 4);
			uint saltL1 = BitPacking.UInt32FromBEBytes(salt, 8);
			uint saltR1 = BitPacking.UInt32FromBEBytes(salt, 12);

			datal = 0x00000000;
			datar = 0x00000000;
			
			for (i = 0; i < p.Length; i += 4)
			{
				datal ^= saltL0; datar ^= saltR0;
				Encipher(ref datal, ref datar); p[i + 0] = datal; p[i + 1] = datar;
			
				if (i + 2 == p.Length) { break; } // 18 here
				datal ^= saltL1; datar ^= saltR1;
				Encipher(ref datal, ref datar); p[i + 2] = datal; p[i + 3] = datar;
			}

			for (i = 0; i < s.Length; i ++)
			{
				uint[] sb = s[i];
				for (j = 0; j < sb.Length; j += 4)
				{
					datal ^= saltL1; datar ^= saltR1;
					Encipher(ref datal, ref datar); sb[j + 0] = datal; sb[j + 1] = datar;
					
					datal ^= saltL0; datar ^= saltR0;
					Encipher(ref datal, ref datar); sb[j + 2] = datal; sb[j + 3] = datar;
				}
			}
		}
		
		uint F(uint x)
		{
		   uint a, b, c, d; uint y;
		
		   d = x & 0xff;
		   x >>= 8;
		   c = x & 0xff;
		   x >>= 8;
		   b = x & 0xff;
		   x >>= 8;
		   a = x & 0xff;
		   y = S[0][a] + S[1][b];
		   y = y ^ S[2][c];
		   y = y + S[3][d];
		
		   return y;
		}

		static void CheckCipherBuffers
			(byte[] inputBuffer, int inputOffset,
			 byte[] outputBuffer, int outputOffset)
		{
			Check.Bounds("inputBuffer", inputBuffer, inputOffset, 8);
            Check.Bounds("outputBuffer", outputBuffer, outputOffset, 8);
		}
		
        /// <summary>
        /// Enciphers eight bytes of data in-place.
        /// </summary>
        /// <param name="buffer">The buffer containing the data.</param>
        /// <param name="offset">The offset of the first byte to encipher.</param>
		public void Encipher(byte[] buffer, int offset)
		{
			Encipher(buffer, offset, buffer, offset);
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

			uint xl = BitPacking.UInt32FromBEBytes(inputBuffer, inputOffset + 0);
			uint xr = BitPacking.UInt32FromBEBytes(inputBuffer, inputOffset + 4);
			Encipher(ref xl, ref xr);
			BitPacking.BEBytesFromUInt32(xl, outputBuffer, outputOffset + 0);
			BitPacking.BEBytesFromUInt32(xr, outputBuffer, outputOffset + 4);
		}

        /// <summary>
        /// Enciphers eight bytes of data.
        /// </summary>
        /// <param name="xl">The first four bytes.</param>
        /// <param name="xr">The last four bytes.</param>
		public void Encipher(ref uint xl, ref uint xr)
		{
			uint Xl, Xr, temp; int i;
			
			Xl = xl;
			Xr = xr;
			
			for (i = 0; i < N; i ++)
			{
				Xl = Xl ^ P[i];
				Xr = F(Xl) ^ Xr;
				
				temp = Xl;
				Xl = Xr;
				Xr = temp;
			}
			
			temp = Xl;
			Xl = Xr;
			Xr = temp;
			
			Xr = Xr ^ P[N];
			Xl = Xl ^ P[N + 1];
			
			xl = Xl;
			xr = Xr;
		}

        /// <summary>
        /// Reverses the encipherment of eight bytes of data in-place.
        /// </summary>
        /// <param name="buffer">The buffer containing the data.</param>
        /// <param name="offset">The offset of the first byte to decipher.</param>
		public void Decipher(byte[] buffer, int offset)
		{
			Decipher(buffer, offset, buffer, offset);
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

            uint xl = BitPacking.UInt32FromBEBytes(inputBuffer, inputOffset + 0);
			uint xr = BitPacking.UInt32FromBEBytes(inputBuffer, inputOffset + 4);
			Decipher(ref xl, ref xr);
			BitPacking.BEBytesFromUInt32(xl, outputBuffer, outputOffset + 0);
			BitPacking.BEBytesFromUInt32(xr, outputBuffer, outputOffset + 4);
		}

        /// <summary>
        /// Reverses the encipherment of eight bytes of data.
        /// </summary>
        /// <param name="xl">The first four bytes.</param>
        /// <param name="xr">The last four bytes.</param>
		public void Decipher(ref uint xl, ref uint xr)
		{
			uint Xl, Xr, temp; int i;
			
			Xl = xl;
			Xr = xr;
			
			for (i = N + 1; i > 1; i --)
			{
				Xl = Xl ^ P[i];
				Xr = F(Xl) ^ Xr;
				
				temp = Xl;
				Xl = Xr;
				Xr = temp;
			}
			
			temp = Xl;
			Xl = Xr;
			Xr = temp;
			
			Xr = Xr ^ P[1];
			Xl = Xl ^ P[0];
			
			xl = Xl;
			xr = Xr;
		}

        /// <summary>
        /// A Blowfish key is weak if one of its S-boxes has a duplicate entry.
        /// See http://www.schneier.com/paper-blowfish-oneyear.html for more information.
        /// </summary>
        public bool IsKeyWeak
        {
            get
            {
                foreach (uint[] sbox in S)
                {
                    // The bool here is useless, but .NET 2.0 lacked HashSet.
                    // We needn't require .NET 3.5+ for anything else here, so why start now...
                    Dictionary<uint, bool> test = new Dictionary<uint, bool>();

                    foreach (uint entry in sbox)
                    {
                        if (test.ContainsKey(entry)) { return true; }
                        test.Add(entry, false);
                    }
                }

                return false;
            }
        }
	}
}