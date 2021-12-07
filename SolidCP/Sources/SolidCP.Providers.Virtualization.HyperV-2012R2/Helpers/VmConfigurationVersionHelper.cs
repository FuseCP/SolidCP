using SolidCP.Providers.HostedSolution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public class VmConfigurationVersionHelper
    {
        const int WINDOWS_2012R2_BUILD = 9600;
        private int _build = 0;
        private PowerShellManager _powerShell;
        public VmConfigurationVersionHelper(PowerShellManager powerShell)
        {
            _powerShell = powerShell;
            try
            {
                _build = ConvertNullableToInt32(Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild", ""));
            }
            catch (Exception ex)
            {
                /* if error then Windows is too old, or MS made BC changes, or Adminstrator broke Windows Registry*/
                HostedSolutionLog.LogWarning("VmConfigurationVersionHelper", ex); //log error, but continue, cause it's not critical
            }
        }


        public Collection<PSObject> GetSupportedVerions()
        {
            Collection<PSObject> result = new Collection<PSObject>(); //init 0 collection, for be sure that we have no null.
            if (_build <= WINDOWS_2012R2_BUILD) //VM config version was intruduced in Windows 2016
            {
                return new Collection<PSObject>(); //return 0 colletion.
            }

            Command cmd = new Command("Get-VMHostSupportedVersion");
            try
            {
                result = _powerShell.Execute(cmd, true, true);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogWarning("VmConfigurationVersionHelper.GetSupportedVerions", ex); //log error, but continue, because it's not critical and we return 0 collection
            }

            return result;
        }

        public List<VMConfigurationVersion> GetSupportedVersionList()
        {
            Collection<PSObject> result = GetSupportedVerions();
            List<VMConfigurationVersion> list = new List<VMConfigurationVersion>() { new VMConfigurationVersion { Version = "0.0", Name = "Default"} };
            foreach (PSObject pS in result)
            {
                list.Add(new VMConfigurationVersion { 
                    Version = pS.GetProperty("Version").ToString(), 
                    Name = pS.GetProperty("Name").ToString() 
                });
            }
            return list;
        }

        public string[] GetSupportedVersionArray()
        {
            Collection<PSObject> result = GetSupportedVerions();
            string[] array = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                array[i] = result[i].GetProperty("Version").ToString();
            }
            return array;
        }

        public bool IsVersionConfigSupports(string version)
        {
            return Array.IndexOf(GetSupportedVersionArray(), version) != -1;
        }

        internal int ConvertNullableToInt32(object value)
        {
            return value == null ? 0 : Convert.ToInt32(value);
        }
    }
}
