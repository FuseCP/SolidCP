using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using SolidCP.EnterpriseServer;
using SolidCP.Portal;

namespace Knom.Helpers.Net
{
    public static class SubnetMask
    {
        public static readonly IPAddress ClassA = IPAddress.Parse("255.0.0.0");
        public static readonly IPAddress ClassB = IPAddress.Parse("255.255.0.0");
        public static readonly IPAddress ClassC = IPAddress.Parse("255.255.255.0");

        public static IPAddress CreateByHostBitLength(int hostpartLength)
        {
            int hostPartLength = hostpartLength;
            int netPartLength = 32 - hostPartLength;

            if (netPartLength < 2)
                throw new ArgumentException("Number of hosts is to large for IPv4");

            Byte[] binaryMask = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                if (i * 8 + 8 <= netPartLength)
                    binaryMask[i] = (byte)255;
                else if (i * 8 > netPartLength)
                    binaryMask[i] = (byte)0;
                else
                {
                    int oneLength = netPartLength - i * 8;
                    string binaryDigit =
                        String.Empty.PadLeft(oneLength, '1').PadRight(8, '0');
                    binaryMask[i] = Convert.ToByte(binaryDigit, 2);
                }
            }
            return new IPAddress(binaryMask);
        }

        public static IPAddress CreateByNetBitLength(int netpartLength)
        {
            int hostPartLength = 32 - netpartLength;
            return CreateByHostBitLength(hostPartLength);
        }

        public static IPAddress CreateByHostNumber(int numberOfHosts)
        {
            int maxNumber = numberOfHosts + 1;

            string b = Convert.ToString(maxNumber, 2);

            return CreateByHostBitLength(b.Length);
        }

        public static string ReturnSubnetmask(String ipaddress)
        {
            uint firstOctet = ReturnFirtsOctet(ipaddress);
            if (firstOctet >= 0 && firstOctet <= 127)
                return "255.0.0.0";
            else if (firstOctet >= 128 && firstOctet <= 191)
                return "255.255.0.0";
            else if (firstOctet >= 192 && firstOctet <= 223)
                return "255.255.255.0";
            else return "0.0.0.0";
        }

        public static uint ReturnFirtsOctet(string ipAddress)
        {
            System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse(ipAddress);
            byte[] byteIP = iPAddress.GetAddressBytes();
            uint ipInUint = (uint)byteIP[0];
            return ipInUint;
        }


        // true if ipAddress falls inside the CIDR range, example
        // bool result = IsInRange("10.50.30.7", "10.0.0.0/8");
        public static bool IsInRange(string ipAddress, string CIDRmask)
        {
            string[] parts = CIDRmask.Split('/');


            if (parts.Length == 1)
            {
                if (ipAddress == CIDRmask)
                {
                    return true;
                }
                return false;
            }
                int IP_addr = BitConverter.ToInt32(IPAddress.Parse(parts[0]).GetAddressBytes(), 0);
                int CIDR_addr = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
                int CIDR_mask = IPAddress.HostToNetworkOrder(-1 << (32 - int.Parse(parts[1])));

                return ((IP_addr & CIDR_mask) == (CIDR_addr & CIDR_mask));
            
        }
    }
   
    public static class IPAddressExtensions
    {
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }
    }

    public static class APIMailCleanerHelper  
    {
        private static StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }
        public static String getServiceURL(int PlanID)
        {
            var l_URL = SolidCP.Portal.ES.Services.Servers.GetMailFilterUrlByHostingPlan(PlanID, ResourceGroups.Filters);
            if (!String.IsNullOrEmpty(l_URL))
            {
                l_URL = l_URL.TrimEnd('/');
                l_URL += "/api/";
            }
            return l_URL;
        }
        public static String getServiceURL()
        {
            //String l_URL = string.Empty;
            //var fileter =  SolidCP.Portal.ES.Services.Servers.GetPackageServiceProvider(PanelSecurity.PackageId, ResourceGroups.Filters);
            var l_URL = SolidCP.Portal.ES.Services.Servers.GetMailFilterUrl(PanelSecurity.PackageId, ResourceGroups.Filters);
           


            //var Session = System.Web.HttpContext.Current.Session;
            //var l_PackageID  =  Session["currentPackage"];//server id
            //System.Data.DataSet dsServers = SolidCP.Portal.ES.Services.Servers.GetRawServers();
            //if (dsServers == null)
            //    return String.Empty;

            //var l_Services = new System.Data.DataView(dsServers.Tables[1], "ServerID=" + l_PackageID.ToString() , "", System.Data.DataViewRowState.CurrentRows);
            //if(l_Services.Count == 0)
            //    return String.Empty;

            ////+ " And ServiceName = 'Mail Cleaner'"
            //int l_ServiceID = 0;
            //SolidCP.EnterpriseServer.ServiceInfo l_oServiceInfo = null;
            //Boolean MailFilterAdded = false;
            //foreach (System.Data.DataRowView l_oServiceRow in l_Services)
            //{
            //    l_ServiceID = Convert.ToInt16( l_oServiceRow["ServiceID"]);
            //    l_oServiceInfo = SolidCP.Portal.ES.Services.Servers.GetServiceInfo(Convert.ToInt16(l_ServiceID));
            //    var l_oProvider = SolidCP.Portal.ES.Services.Servers.GetProvider(l_oServiceInfo.ProviderId);
            //    if (l_oProvider.ProviderName.ToUpper().Equals("MAILCLEANER"))
            //    {
            //        MailFilterAdded = true;
            //        break;
            //    }
            //}

            //if (!MailFilterAdded)
            //    return String.Empty;


            //// load service properties and bind them
            //string[] settings = SolidCP.Portal.ES.Services.Servers.GetServiceSettings(Convert.ToInt16(l_ServiceID));
            //if (settings == null)
            //    return String.Empty;

            //var l_ServiceSettings = ConvertArrayToDictionary(settings);
            //// load resource group details
            ////var resourceGroup = SolidCP.Portal.ES.Services.Servers.GetResourceGroup(provider.GroupId);
            //l_URL = l_ServiceSettings["apiurl"];

            if (!String.IsNullOrEmpty(l_URL))
            { 
                l_URL = l_URL.TrimEnd('/');
                l_URL += "/api/";
            }
            return l_URL;
        }
        public static void APICall(String f_stParam, int f_iPlanID = 0)
        {
            String l_URL = string.Empty;
            try
            {
                if(f_iPlanID==0)
                    l_URL = getServiceURL();
                else
                    l_URL = getServiceURL(f_iPlanID);
            }
            catch (Exception ex)
            {
                throw (ex);
                 //throw( new Exception("MAILCLEANER_API_404"));
            } 

            if (String.IsNullOrEmpty(l_URL))
                return;

            //"https://10.2.150.107/api/"
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(l_URL + f_stParam);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Display the content.
            //Console.WriteLine(responseFromServer);
            if (response != null)
            {
                XmlDocument xmlResult = new XmlDocument();
                // Read the content.
                String lstXMLResult = reader.ReadToEnd();
                xmlResult.LoadXml(lstXMLResult);
                String lstMessage = String.Empty;
                foreach (XmlNode lxmNode in xmlResult.ChildNodes)
                {
                    if (lxmNode.Name == "response")
                    {
                        if (lxmNode.FirstChild.Name == "message")
                            lstMessage = lxmNode.FirstChild.InnerText;
                    }
                }
                // MessageBox.Show(lstMessage);
            }
            // Clean up the streams and the response.
            reader.Close();
            response.Close();


        }
        public static void DomainAdd(String f_stDomain, int f_iPlanID)
        {
            HostingPlanContext cntx = PackagesHelper.GetCachedHostingPlanContext(f_iPlanID);
            if (cntx != null)
            { 
                if (Utils.CheckQouta("Filters.Enable", cntx))
                {
                    APICall("domain/add/name/" + f_stDomain, f_iPlanID);

                } 
            }
        }
        public static void DomainAdd(String f_stDomain)
        {
            if(PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, "Filters.Enable"))
              APICall("domain/add/name/" + f_stDomain);
        }
        public static void DomainRemove(String f_stDomain)
        {
            if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, "Filters.Enable"))
                APICall("domain/remove/name/" + f_stDomain);
        }
    }
}
