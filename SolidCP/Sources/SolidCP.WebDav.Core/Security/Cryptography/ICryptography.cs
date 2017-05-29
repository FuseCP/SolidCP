namespace SolidCP.WebDav.Core.Security.Cryptography
{
    public interface ICryptography
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}