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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using System.Net;

namespace SolidCP.UniversalInstaller
{
	/// <summary>
	/// Utils class.
	/// </summary>
	public class CryptoUtils
	{

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private CryptoUtils()
		{
		}


		#region Crypting

		static string ComputeHash(string plainText, HashAlgorithm hash) => Cryptor.Hash(plainText, hash);

		/// <summary>
		/// Computes the SHA1 hash value
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string ComputeSHA1(string plainText) => Cryptor.SHA1(plainText);
		public static string ComputeSHA256(string plainText) => Cryptor.SHA256(plainText);
        public static string ComputeSHAServerPassword(string password) => ComputeSHA256(password);
		public static bool IsSHA256(string hash) => Cryptor.IsSHA256(hash);
		public static bool SHAEquals(string plainText, string hash) => Cryptor.SHAEquals(plainText, hash);
		public static string CreateCryptoKey(int len) => Cryptor.CreateCryptoKey(len);
		public static string Encrypt(string key, string str) => new Cryptor(key).Encrypt(str);
		public static string EncryptServer(string key, string secret) => Encrypt(key, secret);
		public static string Decrypt(string key, string Base64String) => new Cryptor(key).Decrypt(Base64String);
		public static string GetRandomString(int length) => Cryptor.GetRandomString(length);
		#endregion


	}
}
