using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS
{
    public enum PsScriptPoint
    {
        disabled,
        after_creation,
        before_deletion,
        before_renaming,
        after_renaming,
        external_network_configuration,
        private_network_configuration,
        management_network_configuration
    }
}
