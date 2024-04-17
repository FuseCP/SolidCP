using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
#if NETFRAMEWORK
    public interface ITunnelHandler: System.Web.IHttpHandler
#else
    public interface ITunnelHandler
#endif
    {
        string Route { get; }
#if !NETFRAMEWORK
        void Init(Microsoft.AspNetCore.Builder.WebApplication app);
#endif
    }
}
