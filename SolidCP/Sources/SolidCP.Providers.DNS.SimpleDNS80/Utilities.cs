using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.DNS.SimpleDNS80
{
    public static class Utilities
    {
        public static string CorrectSOARecord(string zoneName, string data)
        {
            if (data == "")
                return "@";
            if (data.EndsWith("." + zoneName))
                return data.Substring(0, data.Length - zoneName.Length - 1);
            if (data.IndexOf(".", StringComparison.Ordinal) == -1)
                return data;
            return data + ".";
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, HttpContent iContent)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = iContent
            };

            var response = new HttpResponseMessage();
            try
            {
                response = await client.SendAsync(request);
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("ERROR: " + e.ToString());
            }

            return response;
        }
    }
}
