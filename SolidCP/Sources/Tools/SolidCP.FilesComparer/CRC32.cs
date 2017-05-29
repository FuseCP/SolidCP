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
using System.Collections.Generic;
using System.Text;

namespace SolidCP.FilesComparer
{
	public class CRC32
	{
		private UInt32[] crc32Table;
		private const int BUFFER_SIZE = 1024;

		/// <summary>
		/// Returns the CRC32 for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to calculate the CRC32 for</param>
		/// <returns>An unsigned integer containing the CRC32 calculation</returns>
		public UInt32 GetCrc32(System.IO.Stream stream)
		{
			unchecked
			{
				UInt32 crc32Result;
				crc32Result = 0xFFFFFFFF;
				byte[] buffer = new byte[BUFFER_SIZE];
				int readSize = BUFFER_SIZE;

				int count = stream.Read(buffer, 0, readSize);
				while (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						crc32Result = ((crc32Result) >> 8) ^ crc32Table[(buffer[i]) ^ ((crc32Result) & 0x000000FF)];
					}
					count = stream.Read(buffer, 0, readSize);
				}

				return ~crc32Result;
			}
		}

		/// <summary>
		/// Construct an instance of the CRC32 class, pre-initialising the table
		/// for speed of lookup.
		/// </summary>
		public CRC32()
		{
			unchecked
			{
				// This is the official polynomial used by CRC32 in PKZip.
				// Often the polynomial is shown reversed as 0x04C11DB7.
				UInt32 dwPolynomial = 0xEDB88320;
				UInt32 i, j;

				crc32Table = new UInt32[256];

				UInt32 dwCrc;
				for (i = 0; i < 256; i++)
				{
					dwCrc = i;
					for (j = 8; j > 0; j--)
					{
						if ((dwCrc & 1) == 1)
						{
							dwCrc = (dwCrc >> 1) ^ dwPolynomial;
						}
						else
						{
							dwCrc >>= 1;
						}
					}
					crc32Table[i] = dwCrc;
				}
			}
		}
	}
}
