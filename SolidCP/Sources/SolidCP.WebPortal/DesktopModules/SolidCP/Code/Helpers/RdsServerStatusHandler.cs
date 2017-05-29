using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SolidCP.Portal
{
    public class RdsServerStatusHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            string fqdnName = context.Request.Params["fqdnName"];
            string itemIndex = context.Request.Params["itemIndex"];
            string result = ES.Services.RDS.GetRdsServerStatus(null, fqdnName);
            
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(string.Format("{0}:{1}:{2}", result, itemIndex, result.StartsWith("Online", StringComparison.InvariantCultureIgnoreCase)));
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);            
        }
    }
}