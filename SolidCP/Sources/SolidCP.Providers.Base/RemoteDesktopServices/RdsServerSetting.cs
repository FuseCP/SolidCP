using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.EnterpriseServer.Base.RDS
{
    public class RdsServerSetting
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public bool ApplyUsers { get; set; }
        public bool ApplyAdministrators { get; set; }
    }
}
