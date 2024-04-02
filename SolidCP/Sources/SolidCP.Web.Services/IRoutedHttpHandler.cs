using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Web.Services
{
    public interface IRoutedHttpHandler
    {
        string Route { get; }
#if !NETFRAMEWORK
        void Init(Microsoft.AspNetCore.Builder.WebApplication app);
#endif
    }
}
