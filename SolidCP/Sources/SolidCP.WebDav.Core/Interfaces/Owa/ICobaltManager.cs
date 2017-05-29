using System.IO;
using Cobalt;

namespace SolidCP.WebDav.Core.Interfaces.Owa
{
    public interface ICobaltManager
    {
        Atom ProcessRequest(int accessTokenId, Stream requestStream);
    }
}