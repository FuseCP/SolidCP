using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Server.Utils
{
    public class PowerShellManager : IDisposable
    {
        private readonly string _remoteComputerName;
        protected static InitialSessionState session = null;

        public string RemoteComputerName { get => _remoteComputerName; }

        protected Runspace RunSpace { get; set; }

        public PowerShellManager(string remoteComputerName)
        {
            _remoteComputerName = remoteComputerName;
            OpenRunspace();
        }

        protected void OpenRunspace()
        {

            if (session == null)
            {
                session = InitialSessionState.CreateDefault();
            }

            Runspace runSpace = RunspaceFactory.CreateRunspace(session);
            runSpace.Open();
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");

            RunSpace = runSpace;
        }

        public void Dispose()
        {
            try
            {
                if (RunSpace != null && RunSpace.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    RunSpace.Close();
                    RunSpace.Dispose();
                    RunSpace = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while disposing PowerShellManager", ex);
            }
        }

        public Collection<PSObject> Execute(Command cmd, bool addComputerNameParameter)
        {
            return Execute(cmd, addComputerNameParameter, false);
        }

        public Collection<PSObject> Execute(IEnumerable<Command> cmd, bool addComputerNameParameter)
        {
            return ExecuteInternal(cmd, addComputerNameParameter, false);
        }

        public Collection<PSObject> Execute(Command cmd, bool addComputerNameParameter, bool withExceptions)
        {
            return ExecuteInternal(cmd, addComputerNameParameter, withExceptions);
        }

        public Collection<PSObject> Execute(IEnumerable<Command> cmd, bool addComputerNameParameter, bool withExceptions)
        {
            return ExecuteInternal(cmd, addComputerNameParameter, withExceptions);
        }

        private Collection<PSObject> ExecuteInternal(Command commands, bool addComputerNameParameter, bool withExceptions)
        {
            return ExecuteInternal(new List<Command> { commands }, addComputerNameParameter, withExceptions);
        }

        private Collection<PSObject> ExecuteInternal(IEnumerable<Command> commands, bool addComputerNameParameter, bool withExceptions)
        {
            List<object> errorList = new List<object>();

            Collection<PSObject> results = null;

            // Create a pipeline
            Pipeline pipeLine = RunSpace.CreatePipeline();
            using (pipeLine)
            {
                var commandsList = commands.ToList();

                if (addComputerNameParameter && !string.IsNullOrWhiteSpace(_remoteComputerName))
                {
                    commandsList[0].Parameters.Add("ComputerName", _remoteComputerName.Trim());
                }
                foreach (Command cmd in commandsList)
                {
                    // Add the command
                    pipeLine.Commands.Add(cmd);
                }

                // Execute the pipeline and save the objects returned.
                results = pipeLine.Invoke();

                // Only non-terminating errors are delivered here.
                // Terminating errors raise exceptions instead.
                // Log out any errors in the pipeline execution
                // NOTE: These errors are NOT thrown as exceptions! 
                // Be sure to check this to ensure that no errors 
                // happened while executing the command.
                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                    }
                }
            }
            pipeLine = null;

            if (withExceptions)
                ExceptionIfErrors(errorList);

            return results;
        }

        private static void ExceptionIfErrors(List<object> errors)
        {
            if (errors != null && errors.Count > 0)
                throw new Exception("Invoke error: " + string.Join("; ", errors.Select(e => e.ToString())));
        }

    }
}
