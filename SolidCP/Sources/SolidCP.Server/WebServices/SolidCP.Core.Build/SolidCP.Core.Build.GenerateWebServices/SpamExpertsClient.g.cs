#if Client
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Filters;
using SolidCP.Server.Utils;
using SolidCP.Server;
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for MailServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface ISpamExperts
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult AddEmailFilter(string name, string domain, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult DeleteDomainFilter(string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult DeleteEmailFilter(string email);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetDomainFilterUser(string domain, string password, string email);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetDomainFilterUserPassword(string name, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetEmailFilterUserPassword(string email, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult AddDomainFilterAlias(string domain, string alias);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias);
    }

    public class SpamExperts
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif