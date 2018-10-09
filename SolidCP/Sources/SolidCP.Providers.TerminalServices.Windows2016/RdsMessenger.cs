using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public class RdsMessenger
    {
        public void SendMessage(List<RdsMessageRecipient> recipients, string text, string primaryDomainController)
        {            
            Runspace runspace = null;

            try
            {
                runspace = RdsRunspaceExtensions.OpenRunspace();
                
                var messages = recipients.GroupBy(m => m.ComputerName, m => m.SessionId, (key, g) => new {
                                                 ComputerName = key, 
                                                 SessionIds = g.ToList()});

                foreach(var message in messages)
                {
                    List<string> scripts = new List<string>
                    {
                        string.Format("msg {0} \"{1}\"", string.Join(",", message.SessionIds.ToArray()), text)
                    };

                    object[] errors;
                    runspace.ExecuteRemoteShellCommand(message.ComputerName, scripts, primaryDomainController, out errors);
                }                               
            }
            finally
            {
                runspace.CloseRunspace();
            }
        }
    }
}
