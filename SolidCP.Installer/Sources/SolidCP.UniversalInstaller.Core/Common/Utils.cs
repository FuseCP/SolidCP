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
using SolidCP.Providers.OS;
using System.Net;

namespace SolidCP.UniversalInstaller
{
	/// <summary>
	/// Utils class.
	/// </summary>
	public class Utils
	{

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private Utils()
		{
		}


		#region Crypting

		/// <summary>
		/// Computes the SHA1 hash value
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string ComputeSHA1(string plainText)
		{
			// Convert plain text into a byte array.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			HashAlgorithm hash = new SHA1Managed();
			// Compute hash value of our plain text with appended salt.
			byte[] hashBytes = hash.ComputeHash(plainTextBytes);
			// Return the result.
			return Convert.ToBase64String(hashBytes);
		}

		public static string CreateCryptoKey(int len)
		{
			byte[] bytes = new byte[len];
			new RNGCryptoServiceProvider().GetBytes(bytes);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				sb.Append(string.Format("{0:X2}", bytes[i]));
			}

			return sb.ToString();
		}

		public static string Encrypt(string key, string str)
		{
			if (str == null)
				return str;

			// We are now going to create an instance of the 
			// Rihndael class.
			RijndaelManaged RijndaelCipher = new RijndaelManaged();
			byte[] plainText = System.Text.Encoding.Unicode.GetBytes(str);
			byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
			PasswordDeriveBytes secretKey = new PasswordDeriveBytes(key, salt);
			ICryptoTransform encryptor = RijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));

			// encode
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainText, 0, plainText.Length);
			cryptoStream.FlushFinalBlock();
			byte[] cipherBytes = memoryStream.ToArray();

			// Close both streams
			memoryStream.Close();
			cryptoStream.Close();

			// Return encrypted string
			return Convert.ToBase64String(cipherBytes);
		}

        public static string Decrypt(string key, string Base64String)
        {
            var RijndaelCipher = new RijndaelManaged();
            byte[] secretText = Convert.FromBase64String(Base64String);
            byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            var secretKey = new PasswordDeriveBytes(key, salt);
            var decryptor = RijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            var MemStream = new MemoryStream();
            var DecryptoStream = new CryptoStream(MemStream, decryptor, CryptoStreamMode.Write);
            DecryptoStream.Write(secretText, 0, secretText.Length);
            DecryptoStream.FlushFinalBlock();
            var Result = MemStream.ToArray();
            MemStream.Close();
            DecryptoStream.Close();
            return Encoding.Unicode.GetString(Result);
        }

		public static string GetRandomString(int length)
		{
			string ptrn = "abcdefghjklmnpqrstwxyz0123456789";
			StringBuilder sb = new StringBuilder();

			byte[] randomBytes = new byte[4];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);

			// Convert 4 bytes into a 32-bit integer value.
			int seed = (randomBytes[0] & 0x7f) << 24 |
						randomBytes[1] << 16 |
						randomBytes[2] << 8 |
						randomBytes[3];


			Random rnd = new Random(seed);

			for (int i = 0; i < length; i++)
				sb.Append(ptrn[rnd.Next(ptrn.Length - 1)]);

			return sb.ToString();
		}

		#endregion


	}
}
