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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SolidCP.Providers.Common
{
	sealed class ByteVector
	{
		# region Public

		public ByteVector Add(Byte[] bytes, Int32 offset, Int32 count)
		{
			Byte [] b = new Byte[ count ];
			Array.Copy( bytes, offset, b, 0, count );
			_bytes.Add( b );
			_size += count;

			return this;
		}

		public ByteVector Add (Byte[] bytes)
		{
			return Add(bytes, 0, bytes.Length);
		}

		public ByteVector Add(string s, Encoding encoding)
		{
			if ( !string.IsNullOrEmpty(s) )
			{
				Add(encoding.GetBytes(s));
			}

			return this;
		}

		public ByteVector Add(string s)
		{
			return Add(s, Encoding.ASCII);
		}



		public Byte[] Get()
		{
			Byte [] result = new Byte[ _size ];
			Int32 offset = 0;

			foreach ( Byte [] b in _bytes )
				{
				Array.Copy( b, 0, result, offset, b.Length );
				offset += b.Length;
				}

			return result;
		}

		public string GetHexString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in Get())
			{
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}

		public Byte[] GetMD5Hash()
		{
			return MD5.Create().ComputeHash(Get());
		}

		public void Clear()
		{
			_bytes.Clear();
			_size = 0;
		}



		public Int64 Size
		{
			get { return _size; }
		}

		# endregion 



		# region Private

		readonly List<Byte[]> _bytes = new List<Byte[]>();
		Int64 _size;

		# endregion
	}
}
