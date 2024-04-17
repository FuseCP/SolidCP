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
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace SolidCP.Providers
{
    /// <summary>
    /// Summary description for CryptoUtils.
    /// </summary>
    public class CryptoUtility
    {
        public virtual string CryptoKey { get; set; }

        public virtual bool EncryptionEnabled { get; set; } = true;
        public CryptoUtility(string key) { CryptoKey = key; }

        public virtual string Encrypt(string InputData)
        {
            string Password = CryptoKey;

            if (!EncryptionEnabled)
                return InputData;

            if (InputData == null)
                return InputData;

            // First we need to turn the input strings into a byte array.
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputData);

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = EncryptStream(memoryStream))
            {
                // Start the encryption process.
                cryptoStream.Write(PlainText, 0, PlainText.Length);

                // Finish encrypting.
                if (cryptoStream is CryptoStream cStream) cStream.FlushFinalBlock();

                // Convert our encrypted data from a memoryStream into a byte array.
                byte[] CipherBytes = memoryStream.ToArray();

                // Convert encrypted data into a base64-encoded string.
                // A common mistake would be to use an Encoding class for that. 
                // It does not work, because not all byte values can be
                // represented by characters. We are going to be using Base64 encoding
                // That is designed exactly for what we are trying to do. 
                string EncryptedData = Convert.ToBase64String(CipherBytes);

                // Return encrypted string.
                return EncryptedData;
            }
        }
        public virtual byte[] Encrypt(byte[] InputData)
        {
            string Password = CryptoKey;

            if (!EncryptionEnabled)
                return InputData;

            if (InputData == null || InputData.Length == 0)
                return InputData;

            // Create a MemoryStream that is going to hold the encrypted bytes 
            using (MemoryStream memoryStream = new MemoryStream())
            using (var cryptoStream = EncryptStream(memoryStream))
            {
                // Start the encryption process.
                cryptoStream.Write(InputData, 0, InputData.Length);

                // Finish encrypting.
                if (cryptoStream is CryptoStream cStream) cStream.FlushFinalBlock();

                // Convert our encrypted data from a memoryStream into a byte array.
                byte[] EncryptedData = memoryStream.ToArray();

                // Return encrypted string.
                return EncryptedData;
            }
        }

        public virtual void Encrypt(Stream inputData, Stream writeTo)
        {
            using (var cryptoStream = EncryptStream(writeTo))
            {
                inputData.CopyTo(cryptoStream);
                // Finish encrypting.
                if (cryptoStream is CryptoStream cStream) cStream.FlushFinalBlock();
            }
        }

        public virtual Stream EncryptStream(Stream writeTo)
        {
            string Password = CryptoKey;

            if (!EncryptionEnabled) return writeTo;

            // We are now going to create an instance of the 
            // Rihndael class.
            RijndaelManaged RijndaelCipher = new RijndaelManaged();


            // We are using salt to make it harder to guess our key
            // using a dictionary attack.
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


            // The (Secret Key) will be generated from the specified 
            // password and salt.
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);


            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key 
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));


            // Create a CryptoStream through which we are going to be processing our data. 
            // CryptoStreamMode.Write means that we are going to be writing data 
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            return new CryptoStream(writeTo, Encryptor, CryptoStreamMode.Write);
        }

        public virtual Stream DecryptStream(Stream input)
        {
            if (!EncryptionEnabled || input.Position == input.Length)
                return input;

            string Password = CryptoKey;
            RijndaelManaged RijndaelCipher = new RijndaelManaged();


            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

            // Create a decryptor from the existing SecretKey bytes.
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));


            // Create a CryptoStream. (always use Read mode for decryption).
            return new CryptoStream(input, Decryptor, CryptoStreamMode.Read);
        }


        public virtual string Decrypt(string InputText)
        {
            if (!EncryptionEnabled)
                return InputText;

            if (InputText == null || InputText == "")
                return InputText;

            byte[] EncryptedData = Convert.FromBase64String(InputText);

            using (MemoryStream memoryStream = new MemoryStream(EncryptedData))
            using (var cryptoStream = DecryptStream(memoryStream))
            {
                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold EncryptedData;
                // DecryptedData is never longer than EncryptedData.
                byte[] PlainText = new byte[EncryptedData.Length];

                // Start decrypting.
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                // Convert decrypted data into a string. 
                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

                // Return decrypted string.   
                return DecryptedData;
            }
        }

        public virtual byte[] Decrypt(byte[] InputData) => Decrypt(new ArraySegment<byte>(InputData));

        public virtual byte[] Decrypt(ArraySegment<byte> InputData)
        {
            if (!EncryptionEnabled || InputData == null || InputData.Count == 0)
            {
                if (InputData.Offset == 0 && InputData.Count == InputData.Array.Length) return InputData.Array;

                var data = new byte[InputData.Count];
                Array.Copy(InputData.Array, InputData.Offset, data, 0, InputData.Count);
                return data;
            }

            using (MemoryStream memoryStream = new MemoryStream(InputData.Array))
            {
                memoryStream.Seek(InputData.Offset, SeekOrigin.Begin);

                // Create a CryptoStream. (always use Read mode for decryption).
                using (var cryptoStream = DecryptStream(memoryStream))
                {
                    // Since at this point we don't know what the size of decrypted data
                    // will be, allocate the buffer long enough to hold EncryptedData;
                    // DecryptedData is never longer than EncryptedData.
                    byte[] PlainData = new byte[InputData.Count];

                    // Start decrypting.
                    int DecryptedCount = cryptoStream.Read(PlainData, 0, PlainData.Length);


                    byte[] DecryptedData = new byte[DecryptedCount];
                    Array.Copy(PlainData, DecryptedData, DecryptedCount);

                    // Return decrypted data.   
                    return DecryptedData;
                }
            }
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
