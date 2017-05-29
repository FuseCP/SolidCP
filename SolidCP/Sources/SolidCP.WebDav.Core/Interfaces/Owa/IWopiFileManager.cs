using Cobalt;

namespace SolidCP.WebDav.Core.Interfaces.Owa
{
    public interface IWopiFileManager
    {
        CobaltFile Create(int accessTokenId);
        CobaltFile Get(string filePath);
        bool Add(string filePath, CobaltFile file);
        bool Delete(string filePath);
    }
}