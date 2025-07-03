using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;

namespace SolidCP.Portal.Services
{
    /// <summary>
    /// We use this service to prevent blocking the UI thread
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ServerService : System.Web.Services.WebService
    {
        [WebMethod]
        public SystemResourceUsageInfo GetServerUsageData(int packageId, int timeout = 60000)
        {
            PackageInfo packageInfo = PackagesHelper.GetCachedPackage(packageId);
            // TODO: We need to find a way to detect whether other services have a Remote Computer setting.
            // As of 2025, this setting exists only for Hyper-V (VPS2012).
            // In other cases, we assume it's not Hyper-V and they don't have Remote Computer settings.
            ServiceInfo serviceInfo = ES.Services.WithTimeout(timeout).Servers.GetServicesByServerIdGroupName(packageInfo.ServerId, ResourceGroups.VPS2012).FirstOrDefault();
            if (serviceInfo != null)
                return ES.Services.WithTimeout(timeout).VPS2012.GetSystemResourceUsageInfo(serviceInfo.ServiceId);

            return ES.Services.WithTimeout(timeout).Servers.GetSystemResourceUsageInfo(packageInfo.ServerId);
        }
    }
}
