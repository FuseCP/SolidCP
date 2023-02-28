#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using SolidCP.Providers.HeliconZoo;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
    public interface IHeliconZoo
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        HeliconZooEngine[] GetEngines();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetEngines(HeliconZooEngine[] userEngines);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsEnginesEnabled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SwithEnginesEnabled(bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetEnabledEnginesForSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetEnabledEnginesForSite(string siteId, string[] engineNames);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsWebCosoleEnabled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetWebCosoleEnabled(bool enabled);
    }

    // wcf service
    public class HeliconZooService : HeliconZoo, IHeliconZoo
    {
        public new HeliconZooEngine[] GetEngines()
        {
            return base.GetEngines();
        }

        public new void SetEngines(HeliconZooEngine[] userEngines)
        {
            base.SetEngines(userEngines);
        }

        public new bool IsEnginesEnabled()
        {
            return base.IsEnginesEnabled();
        }

        public new void SwithEnginesEnabled(bool enabled)
        {
            base.SwithEnginesEnabled(enabled);
        }

        public new string[] GetEnabledEnginesForSite(string siteId)
        {
            return base.GetEnabledEnginesForSite(siteId);
        }

        public new void SetEnabledEnginesForSite(string siteId, string[] engineNames)
        {
            base.SetEnabledEnginesForSite(siteId, engineNames);
        }

        public new bool IsWebCosoleEnabled()
        {
            return base.IsWebCosoleEnabled();
        }

        public new void SetWebCosoleEnabled(bool enabled)
        {
            base.SetWebCosoleEnabled(enabled);
        }
    }
}
#endif