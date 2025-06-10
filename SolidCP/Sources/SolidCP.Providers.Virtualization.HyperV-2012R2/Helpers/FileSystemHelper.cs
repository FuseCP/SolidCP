using Microsoft.Management.Infrastructure;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public class FileSystemHelper
    {
        private MiManager _miCim;
        private string _serverName;

        public FileSystemHelper(MiManager mi)
        {
            _serverName = mi.TargetComputer;
            _miCim = new MiManager(mi, Constants.WMI_CIMV2_NAMESPACE);
        }

        public string ReadRemoteFile(string path)
        {
            // temp file name on "system" drive available through hidden share
            string tempPath = Path.Combine(GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

            HostedSolutionLog.LogInfo("Read remote file: " + path);
            HostedSolutionLog.LogInfo("Local file temp path: " + tempPath);

            // copy remote file to temp file (WMI)
            if (!CopyFile(path, tempPath))
                return null;

            // read content of temp file
            string remoteTempPath = ConvertToUNC(tempPath);
            HostedSolutionLog.LogInfo("Remote file temp path: " + remoteTempPath);

            string content = File.ReadAllText(remoteTempPath);

            // delete temp file (WMI)
            DeleteFile(tempPath);

            return content;
        }

        public void WriteRemoteFile(string path, string content)
        {
            // temp file name on "system" drive available through hidden share
            string tempPath = Path.Combine(GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

            // write to temp file
            string remoteTempPath = ConvertToUNC(tempPath);
            File.WriteAllText(remoteTempPath, content);

            // delete file (WMI)
            if (FileExists(path))
                DeleteFile(path);

            // copy (WMI)
            CopyFile(tempPath, path);

            // delete temp file (WMI)
            DeleteFile(tempPath);
        }

        public void DeleteFile(string path)
        {
            if (path.StartsWith(@"\\"))
            {
                // network share
                File.Delete(path);
            }
            else
            {
                // delete file using WMI
                CimInstance objFile = _miCim.GetCimInstance("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
                CimMethodResult result = _miCim.InvokeMethod(objFile, "Delete", null);
                //check result?
            }
        }

        public void DeleteFolder(string path)
        {
            if (path.StartsWith(@"\\"))
            {
                // network share
                try
                {
                    FileUtils.DeleteFile(path);
                }
                catch { /* just skip */ }
                FileUtils.DeleteFile(path);
            }
            else
            {
                // local folder
                // delete sub folders first
                CimInstance[] objSubFolders = GetSubFolders(path);
                foreach (CimInstance objSubFolder in objSubFolders)
                    DeleteFolder(objSubFolder.CimInstanceProperties["Name"].Value.ToString());

                // delete this folder itself
                CimInstance objFile = _miCim.GetCimInstance("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
                _miCim.InvokeMethod(objFile, "Delete", null); //check result?
            }
        }

        private CimInstance[] GetSubFolders(string path)
        {
            if (path.EndsWith("\\"))
                path = path.Substring(0, path.Length - 1);

            var directory = _miCim.GetCimInstance("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));

            return _miCim.EnumerateAssociatedInstances(directory, "Win32_Subdirectory", "Win32_Directory", "GroupComponent", "PartComponent");
        }

        public bool CopyFile(string sourceFileName, string destinationFileName)
        {
            HostedSolutionLog.LogInfo("Copy file - source: " + sourceFileName);
            HostedSolutionLog.LogInfo("Copy file - destination: " + destinationFileName);

            if (sourceFileName.StartsWith(@"\\")) // network share
            {
                if (!File.Exists(sourceFileName))
                    return false;

                File.Copy(sourceFileName, destinationFileName);
            }
            else
            {
                if (!FileExists(sourceFileName))
                    return false;

                CimInstance objFile = _miCim.GetCimInstance("CIM_DataFile", "Name = '{0}'", sourceFileName.Replace("\\", "\\\\"));
                if (objFile == null)
                    throw new Exception("Source file does not exists: " + sourceFileName);


                var inParams = new CimMethodParametersCollection
                    {
                        CimMethodParameter.Create(
                            "FileName",
                            destinationFileName,
                            CimType.String,
                            CimFlags.In
                        )
                    };

                CimMethodResult result = _miCim.InvokeMethod(objFile, "Copy", inParams);

                if ((uint)result.ReturnValue.Value != 0)
                    throw new Exception("Copy file failed: Result error: " + result.ReturnValue.Value.ToString());

                //    // copy using WMI
                //    Wmi cimv2 = new Wmi(ServerNameSettings, Constants.WMI_CIMV2_NAMESPACE);
                //ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", sourceFileName.Replace("\\", "\\\\"));
                //if (objFile == null)
                //    throw new Exception("Source file does not exists: " + sourceFileName);

                //objFile.InvokeMethod("Copy", new object[] { destinationFileName });
            }
            return true;
        }

        public bool FileExists(string path)
        {
            HostedSolutionLog.LogInfo("Check remote file exists: " + path);

            if (path.StartsWith(@"\\")) // network share
                return File.Exists(path);
            else
            {
                CimInstance objDir = _miCim.GetCimInstance("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
                return (objDir != null);
            }
        }

        public string ConvertToUNC(string path)
        {
            if (String.IsNullOrEmpty(_serverName)
                || path.StartsWith(@"\\"))
                return path;

            return String.Format(@"\\{0}\{1}", _serverName, path.Replace(":", "$"));
        }

        public string GetTempRemoteFolder()
        {
            CimInstance objOS = _miCim.GetCimInstance("win32_OperatingSystem");
            string sysPath = objOS.CimInstanceProperties["SystemDirectory"].Value.ToString();
            // remove trailing slash
            if (sysPath.EndsWith("\\"))
                sysPath = sysPath.Substring(0, sysPath.Length - 1);

            sysPath = sysPath.Substring(0, sysPath.LastIndexOf("\\") + 1) + "Temp";

            return sysPath;
        }

        public bool DirectoryExists(string path)
        {
            if (path.StartsWith(@"\\")) // network share
                                        // TODO: That won't work with remote HyperV, unless network share added into domain.
                                        // Need to check remotly
                return Directory.Exists(path);
            else
            {
                CimInstance objDir = _miCim.GetCimInstance("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
                return (objDir != null);
            }
        }

        public void CreateFolder(string path)
        {
            ExecuteRemoteProcess(String.Format("cmd.exe /c md \"{0}\"", path));
        }

        public bool ExecuteRemoteProcess(string command, bool terminateTimeoutProcess = false, TimeSpan? timeout = null)
        {
            return ExecuteRemoteProcessAsync(command, terminateTimeoutProcess, timeout).GetAwaiter().GetResult();
        }

        public async Task<bool> ExecuteRemoteProcessAsync(string command, bool terminateTimeoutProcess = false, TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? TimeSpan.FromSeconds(20);

            var methodParams = new CimMethodParametersCollection
            {
                CimMethodParameter.Create("CommandLine", command, Microsoft.Management.Infrastructure.CimType.String, CimFlags.In)
            };

            // run process
            var createResult = await Task.Run(() => _miCim.InvokeStaticMethod("Win32_Process", "Create", methodParams));

            uint returnValue = (uint)createResult.ReturnValue.Value;
            if (returnValue != 0)
            {
                throw new InvalidOperationException($"Failed to create remote process. Win32_Process.Create returned error code: {returnValue}.");
            }

            uint processId = (uint)createResult.OutParameters["ProcessId"].Value;
            if (processId == 0)
            {
                throw new InvalidOperationException("Win32_Process.Create executed successfully, but did not return a process identifier (ProcessId).");
            }

            // wait until finished (CIM has no event watcher so we will use polling)
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            while (stopwatch.Elapsed < effectiveTimeout)
            {
                var processInstance = await Task.Run(() => _miCim.GetCimInstance("Win32_Process", "ProcessId = {0}", processId));

                if (processInstance == null)
                {
                    return true; // successfully finished
                }

                await Task.Delay(1000);
            }

            if (terminateTimeoutProcess)
            {
                await TryTerminateProcessAsync(processId);
            }

            return false; // process did not finish in time
        }

        private async Task TryTerminateProcessAsync(uint processId)
        {
            try
            {
                var processToKill = await Task.Run(() => _miCim.GetCimInstance("Win32_Process", "ProcessId = {0}", processId));
                if (processToKill != null)
                {
                    await Task.Run(() => _miCim.InvokeMethod(processToKill, "Terminate"));
                }
            }
            catch { }
        }
    }
}
