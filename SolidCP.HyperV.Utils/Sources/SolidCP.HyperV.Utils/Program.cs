// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Management;

using Vds = Microsoft.Storage.Vds;
using System.Diagnostics;

namespace SolidCP.HyperV.Utils
{
    class Program
    {
        // command line parameters
        private const string ns = @"root\virtualization";
        private static Wmi wmi = null;
        private static string computer = null;
        private static string command = null;
        private static Dictionary<string, string> Parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        static void Main(string[] args)
        {
            // display welcome screen
            DisplayWelcome();

            // parse parameters
            if (!ParseParameters(args))
            {
                DisplayUsage();
                return;
            }

            // connect WMI
            wmi = new Wmi(computer, ns);

            try
            {
                // run command
                if (String.Equals(command, "MountVHD", StringComparison.CurrentCultureIgnoreCase))
                    MountVHD();
                else if (String.Equals(command, "UnmountVHD", StringComparison.CurrentCultureIgnoreCase))
                    UnmountVHD();
                else
                {
                    Console.WriteLine("Unknown command: " + command);
                    DisplayUsage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("System message: " + ex.InnerException.Message);
            }
        }

        private static void MountVHD()
        {
            // check parameters
            AssertParameter("path");
            string vhdPath = Parameters["path"];

            ManagementObject objImgSvc = GetImageManagementService();

            Console.WriteLine("Mount VHD: " + vhdPath);

            // get method params
            ManagementBaseObject inParams = objImgSvc.GetMethodParameters("Mount");
            inParams["Path"] = vhdPath;

            ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("Mount", inParams, null);
            JobResult result = CreateJobResultFromWmiMethodResults(outParams);

            // load storage job
            if (result.ReturnValue != ReturnCode.JobStarted
                || result.Job.JobState == ConcreteJobState.Exception)
            {
                throw new Exception("Mount job failed to start with the following message: " + result.Job.ErrorDescription);
            }

            ManagementObject objJob = wmi.GetWmiObject("msvm_StorageJob", "InstanceID = '{0}'", result.Job.Id);

            if (JobCompleted(result.Job))
            {
                // load output data
                ManagementObject objImage = wmi.GetRelatedWmiObject(objJob, "Msvm_MountedStorageImage");

                int pathId = Convert.ToInt32(objImage["PathId"]);
                int portNumber = Convert.ToInt32(objImage["PortNumber"]);
                int targetId = Convert.ToInt32(objImage["TargetId"]);
                int lun = Convert.ToInt32(objImage["Lun"]);

                string diskAddress = String.Format("Port{0}Path{1}Target{2}Lun{3}", portNumber, pathId, targetId, lun);

                 // find mounted disk using VDS
                Vds.Advanced.AdvancedDisk advancedDisk = null;
                Vds.Pack diskPack = null;

                // first attempt
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine("Querying mounted disk...");
                FindVdsDisk(diskAddress, out advancedDisk, out diskPack);

                // second attempt
                if (advancedDisk == null)
                {
                    System.Threading.Thread.Sleep(20000);
                    Console.WriteLine("Querying mounted disk - second attempt...");
                    FindVdsDisk(diskAddress, out advancedDisk, out diskPack);
                }

                if (advancedDisk == null)
                    throw new Exception("Could not find mounted disk");

                List<string> volumes = new List<string>();

                Console.WriteLine("Disk flags: " + advancedDisk.Flags);

                // clear READONLY
                if ((advancedDisk.Flags & Vds.DiskFlags.ReadOnly) == Vds.DiskFlags.ReadOnly)
                {
                    Console.Write("Clearing disk Read-Only flag...");
                    advancedDisk.ClearFlags(Vds.DiskFlags.ReadOnly);

                    while ((advancedDisk.Flags & Vds.DiskFlags.ReadOnly) == Vds.DiskFlags.ReadOnly)
                    {
                        System.Threading.Thread.Sleep(100);
                        advancedDisk.Refresh();
                    }
                    Console.WriteLine("Done");
                }

                // bring disk ONLINE
                if (advancedDisk.Status == Vds.DiskStatus.Offline)
                {
                    Console.Write("Bringing disk online...");
                    advancedDisk.Online();

                    while (advancedDisk.Status == Vds.DiskStatus.Offline)
                    {
                        System.Threading.Thread.Sleep(100);
                        advancedDisk.Refresh();
                    }

                    Console.WriteLine("Done");
                }

                // determine disk index for DiskPart
                Wmi cimv2 = new Wmi(computer, "root\\CIMV2");
                ManagementObject objDisk = cimv2.GetWmiObject("win32_diskdrive",
                    "Model='Msft Virtual Disk SCSI Disk Device' and ScsiTargetID={0} and ScsiLogicalUnit={1} and scsiPort={2}",
                    targetId, lun, portNumber);

                if (objDisk != null)
                {
                    Console.WriteLine("DiskPart disk index: " + Convert.ToInt32(objDisk["Index"]));
                }

                // find volume
                diskPack.Refresh();
                foreach (Vds.Volume volume in diskPack.Volumes)
                {
                    volumes.Add(volume.DriveLetter.ToString());
                }

                if (volumes.Count == 0 && objDisk != null)
                {
                    // find volumes using WMI
                    foreach (ManagementObject objPartition in objDisk.GetRelated("Win32_DiskPartition"))
                    {
                        foreach (ManagementObject objVolume in objPartition.GetRelated("Win32_LogicalDisk"))
                        {
                            volumes.Add(objVolume["Name"].ToString().TrimEnd(':'));
                        }
                    }
                }

                foreach (string volume in volumes)
                {
                    Console.WriteLine("Volume found: " + volume);
                }

                //// find disk index
                //Wmi win32 = new Wmi(computer, @"root\cimv2");

                //System.Threading.Thread.Sleep(1000); // small pause

                //ManagementObject objDisk = win32.GetWmiObject("win32_DiskDrive",
                //    "Model='Msft Virtual Disk SCSI Disk Device' and ScsiTargetID={0} and ScsiLogicalUnit={1} and ScsiPort={2}",
                //    targetId, lun, portNumber);

                //int diskIndex = Convert.ToInt32(objDisk["Index"]);

                Console.WriteLine("\nDisk has been mounted.\n");
                //Console.WriteLine("Disk index: " + advancedDisk.);
            }
        }

        private static void FindVdsDisk(string diskAddress, out Vds.Advanced.AdvancedDisk advancedDisk, out Vds.Pack diskPack)
        {
            advancedDisk = null;
            diskPack = null;

            Vds.ServiceLoader serviceLoader = new Vds.ServiceLoader();
            Vds.Service vds = serviceLoader.LoadService(computer);
            vds.WaitForServiceReady();

            foreach (Vds.Disk disk in vds.UnallocatedDisks)
            {
                if (disk.DiskAddress == diskAddress)
                {
                    advancedDisk = (Vds.Advanced.AdvancedDisk)disk;
                    break;
                }
            }

            if (advancedDisk == null)
            {
                vds.HardwareProvider = false;
                vds.SoftwareProvider = true;

                foreach (Vds.SoftwareProvider provider in vds.Providers)
                    foreach (Vds.Pack pack in provider.Packs)
                        foreach (Vds.Disk disk in pack.Disks)
                            if (disk.DiskAddress == diskAddress)
                            {
                                diskPack = pack;
                                advancedDisk = (Vds.Advanced.AdvancedDisk)disk;
                                break;
                            }
            }
        }

        private static void UnmountVHD()
        {
            // check parameters
            AssertParameter("path");
            string vhdPath = Parameters["path"];

            Console.WriteLine("Unmount VHD: " + vhdPath);

            ManagementObject objImgSvc = GetImageManagementService();

            // get method params
            ManagementBaseObject inParams = objImgSvc.GetMethodParameters("Unmount");
            inParams["Path"] = vhdPath;

            ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("Unmount", inParams, null);
            ReturnCode result = (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);
            if (result != ReturnCode.OK)
            {
                throw new Exception("Unmount task failed with the \"" + result + "\" code");
            }

            Console.WriteLine("\nDisk has been unmounted.");
        }

        #region Jobs
        private static bool JobCompleted(ConcreteJob job)
        {
            bool jobCompleted = true;

            while (job.JobState == ConcreteJobState.Starting ||
                job.JobState == ConcreteJobState.Running)
            {
                System.Threading.Thread.Sleep(200);
                job = GetJob(job.Id);
            }

            if (job.JobState != ConcreteJobState.Completed)
            {
                jobCompleted = false;
            }

            return jobCompleted;
        }

        public static ConcreteJob GetJob(string jobId)
        {
            ManagementObject objJob = GetJobWmiObject(jobId);
            return CreateJobFromWmiObject(objJob);
        }

        private static ConcreteJob CreateJobFromWmiMethodResults(ManagementBaseObject outParams)
        {
            ManagementBaseObject objJob = wmi.GetWmiObjectByPath((string)outParams["Job"]);
            if (objJob == null || objJob.Properties.Count == 0)
                return null;

            return CreateJobFromWmiObject(objJob);
        }

        private static JobResult CreateJobResultFromWmiMethodResults(ManagementBaseObject outParams)
        {
            JobResult result = new JobResult();

            // return value
            result.ReturnValue = (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);

            // job
            ManagementBaseObject objJob = wmi.GetWmiObjectByPath((string)outParams["Job"]);
            if (objJob != null && objJob.Properties.Count > 0)
            {
                result.Job = CreateJobFromWmiObject(objJob);
            }

            return result;
        }

        private static ManagementObject GetJobWmiObject(string id)
        {
            return wmi.GetWmiObject("CIM_ConcreteJob", "InstanceID = '{0}'", id);
        }

        private static ConcreteJob CreateJobFromWmiObject(ManagementBaseObject objJob)
        {
            if (objJob == null || objJob.Properties.Count == 0)
                return null;

            ConcreteJob job = new ConcreteJob();
            job.Id = (string)objJob["InstanceID"];
            job.JobState = (ConcreteJobState)Convert.ToInt32(objJob["JobState"]);
            job.Caption = (string)objJob["Caption"];
            job.Description = (string)objJob["Description"];
            job.ElapsedTime = wmi.ToDateTime((string)objJob["ElapsedTime"]);
            job.StartTime = wmi.ToDateTime((string)objJob["StartTime"]);
            job.ErrorCode = Convert.ToInt32(objJob["ErrorCode"]);
            job.ErrorDescription = (string)objJob["ErrorDescription"];
            job.PercentComplete = Convert.ToInt32(objJob["PercentComplete"]);
            return job;
        }
        #endregion

        #region Managers
        private static ManagementObject GetVirtualSystemManagementService()
        {
            return wmi.GetWmiObject("msvm_VirtualSystemManagementService");
        }

        private static ManagementObject GetVirtualSwitchManagementService()
        {
            return wmi.GetWmiObject("msvm_VirtualSwitchManagementService");
        }

        private static ManagementObject GetImageManagementService()
        {
            return wmi.GetWmiObject("msvm_ImageManagementService");
        }
        #endregion

        private static void DisplayWelcome()
        {
            string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            Console.WriteLine("VmUtils - utility to work with Hyper-V virtual resources. Version " + ver);
            Console.WriteLine("Copyright (C) 2011 by Outercurve Foundation. All rights reserved.\n");
        }

        private static bool ParseParameters(string[] args)
        {
            if (args == null || args.Length == 0)
                return false;

            // command
            command = args[0];

            for (int i = 1; i < args.Length; i++)
            {
                if (!args[i].StartsWith("-"))
                    return false; // wrong parameter format

                string name = args[i].Substring(1);

                if (i == (args.Length - 1))
                    return false; // no parameter value

                string val = args[i + 1];
                i++;

                // add parameter to the hash
                Parameters.Add(name, val);

                if (String.Equals(name, "computer", StringComparison.CurrentCultureIgnoreCase))
                    computer = val;
            }

            return true;
        }

        private static void AssertParameter(string name)
        {
            if (!Parameters.ContainsKey(name))
            {
                throw new Exception(String.Format("Command \"{0}\" expect \"{1}\" parameter which was not supplied.", command, name));
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("\nUSAGE:");
            Console.WriteLine("   vmutils <command> [-parameter1 value1 -parameter2 value2 ...]\n");

            Console.WriteLine("EXAMPLE:");
            Console.WriteLine("   vmtuils MountVHD -path \"c:\\templates\\Windows 2008.vhd\" -computer HYPERV01\n");

            Console.WriteLine("SUPPORTED COMMANDS:");
            Console.WriteLine("   MountVHD - mounts VHD.");
            Console.WriteLine("      Parameters:");
            Console.WriteLine("          -path - path to VHD file.");
            Console.WriteLine("          -computer - (optional) remote computer with Hyper-V role installed.");
            Console.WriteLine("");
            Console.WriteLine("   UnmountVHD - unmounts VHD.");
            Console.WriteLine("      Parameters:");
            Console.WriteLine("          -path - path to VHD file.");
            Console.WriteLine("          -computer - (optional) remote computer with Hyper-V role installed.");
        }
    }
}
