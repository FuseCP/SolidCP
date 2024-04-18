// Copyright (c) 2024, SolidCP
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
using System.IO;
using System.Text;
using System.Security.Cryptography;
using SolidCP.Providers.OS;
using System.Linq;

namespace SolidCP.Providers
{
    /// <summary>
    /// Summary description for CryptoUtils.
    /// </summary>
    public class Cryptor
    {
        public string CryptoKey { get; set; }
        public bool EncryptionEnabled { get; set; } = true;
        public Cryptor(string key) { CryptoKey = key; }
        public Cryptor(string key, bool encryptionEnabled) { CryptoKey = key; EncryptionEnabled = encryptionEnabled; }

        public virtual string Encrypt(string InputText, bool legacy = false)
        {
            string Password = CryptoKey;

            if (!EncryptionEnabled || string.IsNullOrEmpty(InputText)) return InputText;

            // First we need to turn the input strings into a byte array.
            byte[] PlainText;
            if (legacy) PlainText = Encoding.Unicode.GetBytes(InputText);
            else PlainText = Encoding.UTF8.GetBytes(InputText);

            var CipherBytes = Encrypt(PlainText);
            // Convert encrypted data into a base64-encoded string.
            // A common mistake would be to use an Encoding class for that. 
            // It does not work, because not all byte values can be
            // represented by characters. We are going to be using Base64 encoding
            // That is designed exactly for what we are trying to do. 
            // We use the new UTF8 format, so prepend a ! character to the string to
            // distinguish it from the old format that used UTF16.
            // If we use UTF16 we essentially half the cypher strength as the first
            // byte in UTF16 is always 0
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            if (!legacy) EncryptedData = "!" + EncryptedData;

            // Return encrypted string.
            return EncryptedData;
        }
        public virtual byte[] Encrypt(ArraySegment<byte> InputData)
        {
            if (!EncryptionEnabled || InputData == null || InputData.Count == 0)
            {
                if (InputData.Offset == 0 && InputData.Count == InputData.Array.Length) return InputData.Array;

                var data = new byte[InputData.Count];
                Array.Copy(InputData.Array, InputData.Offset, data, 0, InputData.Count);
                return data;
            }

            var encryptor = Encryptor();
            return encryptor.TransformFinalBlock(InputData.Array, InputData.Offset, InputData.Count);
        }

        public virtual byte[] Encrypt(byte[] InputData)
        {
            if (!EncryptionEnabled || InputData == null || InputData.Length == 0) return InputData;
            var encryptor = Encryptor();
            return encryptor.TransformFinalBlock(InputData, 0, InputData.Length);
        }

        public virtual void Encrypt(Stream inputData, Stream writeTo)
        {
            using (var cryptoStream = EncryptorStream(writeTo))
            {
                inputData.CopyTo(cryptoStream);
                // Finish encrypting.
                if (cryptoStream is CryptoStream cStream) cStream.FlushFinalBlock();
                else cryptoStream.Flush();
            }
        }

        public virtual Stream EncryptorStream(Stream writeTo)
        {
            if (!EncryptionEnabled) return writeTo;

            // Create a CryptoStream through which we are going to be processing our data. 
            // CryptoStreamMode.Write means that we are going to be writing data 
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            return new CryptoStream(writeTo, Encryptor(), CryptoStreamMode.Write);
        }

        public virtual ICryptoTransform Encryptor()
        {
            // Rihndael class.
            SymmetricAlgorithm cipher = Aes.Create();

            // We are using salt to make it harder to guess our key
            // using a dictionary attack.
            byte[] Salt = Encoding.ASCII.GetBytes(CryptoKey.Length.ToString());

            // The (Secret Key) will be generated from the specified 
            // password and salt.
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(CryptoKey, Salt);

            var key = SecretKey.GetBytes(32);
            var iv = SecretKey.GetBytes(16);
            cipher.Key = key;
            cipher.IV = iv;
            cipher.Mode = CipherMode.CBC;
            cipher.Padding = PaddingMode.PKCS7;
            cipher.BlockSize = 128;
            cipher.FeedbackSize = 128;

            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key 
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            return cipher.CreateEncryptor(cipher.Key, iv);
        }

        public ICryptoTransform Decryptor()
        {

            byte[] Salt = Encoding.ASCII.GetBytes(CryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(CryptoKey, Salt);

            var key = SecretKey.GetBytes(32);
            var iv = SecretKey.GetBytes(16);

            var cipher = Aes.Create();

            cipher.KeySize = 256;
            cipher.Key = key;
            cipher.IV = iv;
            cipher.Mode = CipherMode.CBC;
            cipher.Padding = PaddingMode.PKCS7;
            cipher.BlockSize = 128;
            cipher.FeedbackSize = 128;

            // Create a decryptor from the existing SecretKey bytes.
            var decryptor = cipher.CreateDecryptor(cipher.Key, iv);
            return decryptor;
        }
        public virtual Stream DecryptorStream(Stream input)
        {
            if (!EncryptionEnabled || input.Position == input.Length) return input;

            // Create a CryptoStream. (always use Read mode for decryption).
            return new CryptoStream(input, Decryptor(), CryptoStreamMode.Read);
        }

        public virtual string Decrypt(string InputText)
        {
            if (!EncryptionEnabled) return InputText;

            if (string.IsNullOrEmpty(InputText)) return InputText;

            bool useUTF8 = InputText.StartsWith("!"); // use the new UTF8 format
            if (useUTF8) InputText = InputText.Substring(1);
         
            
            byte[] EncryptedData = Convert.FromBase64String(InputText);

            byte[] DecryptedData = Decrypt(EncryptedData);

            string DecryptedText;
            if (useUTF8) DecryptedText = Encoding.UTF8.GetString(DecryptedData);
            else DecryptedText = Encoding.Unicode.GetString(DecryptedData);
            // Return decrypted string.
            return DecryptedText;
        }

        public virtual byte[] Decrypt(byte[] InputData) => Decrypt(new ArraySegment<byte>(InputData));

        byte[] buffer = new byte[1024];
        public virtual byte[] Decrypt(ArraySegment<byte> InputData)
        {
            if (!EncryptionEnabled || InputData == null || InputData.Count == 0)
            {
                if (InputData.Offset == 0 && InputData.Count == InputData.Array.Length) return InputData.Array;

                var data = new byte[InputData.Count];
                Array.Copy(InputData.Array, InputData.Offset, data, 0, InputData.Count);
                return data;
            }

            var decryptor = Decryptor();
            return decryptor.TransformFinalBlock(InputData.Array, InputData.Offset, InputData.Count);
        }
        static string Hash(string plainText, HashAlgorithm hash)
        {
            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            // Return the result.
            return Convert.ToBase64String(hashBytes);
        }
        public static string SHA1(string plainText) => Hash(plainText, new SHA1Managed());
        public static string SHA256(string plainText) => $"SHA256:{Hash(plainText, new SHA256Managed())}";
        public static bool IsSHA256(string hash) => hash.StartsWith("SHA256:");

        public static bool SHAEquals(string plainText, string hash)
        {
            if (IsSHA256(hash)) return SHA256(plainText) == hash;
            else return SHA1(plainText) == hash;
        }
    }
}
