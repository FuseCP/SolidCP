using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Base
{
    [Serializable]
    public class SpamExpertsRoute
    {
        public int PackageId { get; set; }
        public string DomainName { get; set; }
        public string Route { get; set; }
        public int Port { get; set; }
        public int MaxMessageSize { get; set; }

        public string[] Destinations
        {
            get
            {
                if (String.IsNullOrEmpty(Route))
                    return null;

                return Route.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.ToLower())
                    .Distinct()
                    .ToArray();
            }
        }
    }
}
