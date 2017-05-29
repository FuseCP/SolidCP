// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace SolidCP.Providers.Utils
{
    public class CRC32 : HashAlgorithm
    {
        protected static uint AllOnes = 0xffffffff;
        protected static Hashtable cachedCRC32Tables;
        protected static bool autoCache;

        protected uint[] crc32Table;
        private uint m_crc;

        /// <summary>
        /// Returns the default polynomial (used in WinZip, Ethernet, etc)
        /// </summary>
        public static uint DefaultPolynomial
        {
            get { return 0x04C11DB7; }
        }

        /// <summary>
        /// Gets or sets the auto-cache setting of this class.
        /// </summary>
        public static bool AutoCache
        {
            get { return autoCache; }
            set { autoCache = value; }
        }

        /// <summary>
        /// Initialize the cache
        /// </summary>
        static CRC32()
        {
            cachedCRC32Tables = Hashtable.Synchronized(new Hashtable());
            autoCache = true;
        }

        public static void ClearCache()
        {
            cachedCRC32Tables.Clear();
        }


        /// <summary>
        /// Builds a crc32 table given a polynomial
        /// </summary>
        /// <param name="ulPolynomial"></param>
        /// <returns></returns>
        protected static uint[] BuildCRC32Table(uint ulPolynomial)
        {
            uint dwCrc;
            uint[] table = new uint[256];

            // 256 values representing ASCII character codes. 
            for (int i = 0; i < 256; i++)
            {
                dwCrc = (uint)i;
                for (int j = 8; j > 0; j--)
                {
                    if ((dwCrc & 1) == 1)
                        dwCrc = (dwCrc >> 1) ^ ulPolynomial;
                    else
                        dwCrc >>= 1;
                }
                table[i] = dwCrc;
            }

            return table;
        }


        /// <summary>
        /// Creates a CRC32 object using the DefaultPolynomial
        /// </summary>
        public CRC32()
            : this(DefaultPolynomial)
        {
        }

        /// <summary>
        /// Creates a CRC32 object using the specified Creates a CRC32 object 
        /// </summary>
        public CRC32(uint aPolynomial)
            : this(aPolynomial, CRC32.AutoCache)
        {
        }

        /// <summary>
        /// Construct the 
        /// </summary>
        public CRC32(uint aPolynomial, bool cacheTable)
        {
            this.HashSizeValue = 32;

            crc32Table = (uint[])cachedCRC32Tables[aPolynomial];
            if (crc32Table == null)
            {
                crc32Table = CRC32.BuildCRC32Table(aPolynomial);
                if (cacheTable)
                    cachedCRC32Tables.Add(aPolynomial, crc32Table);
            }
            Initialize();
        }

        /// <summary>
        /// Initializes an implementation of HashAlgorithm.
        /// </summary>
        public override void Initialize()
        {
            m_crc = AllOnes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void HashCore(byte[] buffer, int offset, int count)
        {
            // Save the text in the buffer. 
            for (int i = offset; i < count; i++)
            {
                ulong tabPtr = (m_crc & 0xFF) ^ buffer[i];
                m_crc >>= 8;
                m_crc ^= crc32Table[tabPtr];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override byte[] HashFinal()
        {
            byte[] finalHash = new byte[4];
            ulong finalCRC = m_crc ^ AllOnes;

            finalHash[0] = (byte)((finalCRC >> 24) & 0xFF);
            finalHash[1] = (byte)((finalCRC >> 16) & 0xFF);
            finalHash[2] = (byte)((finalCRC >> 8) & 0xFF);
            finalHash[3] = (byte)((finalCRC >> 0) & 0xFF);

            return finalHash;
        }

        /// <summary>
        /// Computes the hash value for the specified Stream.
        /// </summary>
        new public byte[] ComputeHash(Stream inputStream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, 4096)) > 0)
            {
                HashCore(buffer, 0, bytesRead);
            }
            return HashFinal();
        }


        /// <summary>
        /// Overloaded. Computes the hash value for the input data.
        /// </summary>
        new public byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Overloaded. Computes the hash value for the input data.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        new public byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            HashCore(buffer, offset, count);
            return HashFinal();
        }
    }
}
