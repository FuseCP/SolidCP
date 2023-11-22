﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidCP.Providers.Virtualization;
using Newtonsoft.Json.Serialization;
using SolidCP.EnterpriseServer;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.guacamole;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers
{
    class GuacaHelper
    {

        public static string GetUrl(VirtualMachine vm)
        {

            //string iv = null;
            string[] key;
            string guacaserverurl = null;

            guacadata cookiedata = new guacadata();

            StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);

            try
            {
                key = settings["GuacamoleConnectPassword"].Split(':');
                guacaserverurl = settings["GuacamoleConnectScript"];
                cookiedata.password = settings["GuacamoleHyperVAdministratorPassword"];
                cookiedata.domain = settings["GuacamoleHyperVDomain"];
                cookiedata.hostname = settings["GuacamoleHyperVIP"];
                if (String.IsNullOrEmpty(settings["GuacamoleHyperVUser"]))
                {
                    cookiedata.username = "Administrator";
                }
                else
                {
                    cookiedata.username = settings["GuacamoleHyperVUser"];
                }
            }
            catch
            {
                return "";
            }

            cookiedata.security = "vmconnect";
            cookiedata.protocol = "rdp";
            cookiedata.port = "2179";
            cookiedata.vmhostname = vm.Hostname;
            cookiedata.timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");

            /* Als Ansatz eines pseudorandom IV mit Datum dec values drin.
            string strkey = "";
            foreach (var value in key[1])
            {
                decimal decValue = value;
                strkey = String.Format("{0} {1}", strkey, decValue.ToString());
            }
            */

            cookiedata.preconnectionblob = vm.VirtualMachineId;
            if (cookiedata.hostname == "" || cookiedata.domain == "" || cookiedata.password == "" || cookiedata.preconnectionblob == "") return "";
            string cookie = JsonConvert.SerializeObject(cookiedata);
            try
            {
                //Encryption.GenerateIV(out iv); // Random IV
                string cryptedcookie = Encryption.Encrypt(cookie, key[0], key[1]);
                //string urlstring = UrlEncodeBase64(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}|{1}", cryptedcookie, key[1])))); // Random IV mit übergeben
                string urlstring = UrlEncodeBase64(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}", cryptedcookie))));
                return String.Format("{0}?e={1}&Resolution=", guacaserverurl, urlstring);
            }
            catch
            {
                return "";
            }

        }

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

        private static string UrlEncodeBase64(string base64Data)
        {
            return new string(UrlEncodeBase64(base64Data.ToCharArray()));
        }

        private static char[] UrlEncodeBase64(char[] base64Data)
        {
            for (int i = 0; i < base64Data.Length; i++)
            {
                switch (base64Data[i])
                {
                    case '+':
                        base64Data[i] = '-';
                        break;
                    case '/':
                        base64Data[i] = '_';
                        break;
                }
            }
            return base64Data;
        }
    }
}
