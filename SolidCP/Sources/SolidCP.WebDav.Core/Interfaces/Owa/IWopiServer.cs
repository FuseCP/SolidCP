using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Entities.Owa;

namespace SolidCP.WebDav.Core.Interfaces.Owa
{
    public interface IWopiServer
    {
        CheckFileInfo GetCheckFileInfo(WebDavAccessToken token);
        byte[] GetFileBytes(int accessTokenId);
    }
}