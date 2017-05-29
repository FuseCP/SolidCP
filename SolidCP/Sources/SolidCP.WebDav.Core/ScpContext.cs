using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SolidCP.WebDav.Core.Security.Authentication.Principals;
using SolidCP.WebDav.Core.Scp.Framework;

namespace SolidCP.WebDav.Core
{
    public class ScpContext
    {
        public static ScpPrincipal User { get { return HttpContext.Current.User as ScpPrincipal; } }
        public static SCP Services { get { return SCP.Services; } }
    }
}
