using System;
using System.Linq;
using System.Management.Automation.Runspaces;
namespace SolidCP.Providers.Web
{
    public class ACMESharpRunspaceHelper : RunspaceHelper
    {
        public ACMESharpRunspaceHelper()
        {
        }

        public bool AreModulesInstalled(string[] modules)
        {
            object[] objArray;
            bool result;
            Runspace runspace = null;
            try
            {
                try
                {
                    runspace = this.OpenRunspace();
                    Command command = new Command("Import-Module");
                    command.Parameters.Add("Name", modules);
                    base.ExecuteShellCommand(runspace, command, out objArray);
                    result = !objArray.Any<object>();
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
            finally
            {
                base.CloseRunspace(runspace);
            }
            return result;
        }

        public Runspace OpenRunspace()
        {
            return base.OpenRunspace(new string[] { "ACMESharp" });
        }
    }
}
