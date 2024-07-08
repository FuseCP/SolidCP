using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Tasks
{
    public class DeleteVirtualMachineTask: ControllerBase
    {
        public DeleteVirtualMachineTask(ControllerBase provider) : base(provider) { }

        internal void DeleteVirtualMachineNewTask(int itemId, VirtualMachine vm, bool saveFiles, bool exportVps, string exportPath, bool keepPackageItem)
        {
            string taskId = vm.CurrentTaskId;
            // start task
            int maximumExecutionSeconds = 60 * 20;
            TaskManager.StartTask(taskId, "VPS2012", "DELETE", vm.Name, vm.Id, vm.PackageId, maximumExecutionSeconds);

            DeleteVirtualMachineInternal(itemId, vm, saveFiles, exportVps, exportPath, keepPackageItem);

            // complete task
            TaskManager.CompleteTask();
        }

        internal void DeleteVirtualMachineContinueTask(string taskId, int itemId, VirtualMachine vm, bool saveFiles, bool exportVps, string exportPath, bool keepPackageItem)
        {
            if(taskId != vm.CurrentTaskId) {
                throw new ArgumentException("The task is not the same as the virtual machine :" + taskId + " and VM task" + vm.CurrentTaskId);
            }
            if (TaskManager.GetTask(vm.CurrentTaskId) == null) {
                throw new NullReferenceException("There is not the Task with ID: " + vm.CurrentTaskId);
            }

            DeleteVirtualMachineInternal(itemId, vm, saveFiles, exportVps, exportPath, keepPackageItem);

        }


        private void DeleteVirtualMachineInternal(int itemId, VirtualMachine vm, bool saveFiles, bool exportVps, string exportPath, bool keepPackageItem)
        {
            //string taskId = vm.CurrentTaskId;
            //// start task
            //int maximumExecutionSeconds = 60 * 20;
            //TaskManager.StartTask(taskId, "VPS2012", "DELETE", vm.Name, vm.Id, vm.PackageId, maximumExecutionSeconds);

            PowerShellScript.CheckCustomPsScript(PsScriptPoint.before_deletion, vm);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // check VM state
                VirtualMachine vps = vs.GetVirtualMachine(vm.VirtualMachineId);

                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                vm.ClusterName = (Utils.ParseBool(settings["UseFailoverCluster"], false)) ? settings["ClusterName"] : null;

                JobResult result = null;
                //vm.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;

                if (vps != null)
                {
                    #region turn off machine (if required)

                    // stop virtual machine
                    if (vps.State != VirtualMachineState.Off)
                    {
                        TaskManager.Write("VPS_DELETE_TURN_OFF");
                        result = vs.ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.TurnOff, vm.ClusterName);

                        // check result
                        if (result.ReturnValue != ReturnCode.JobStarted)
                        {
                            TaskManager.WriteError(VirtualizationErrorCodes.JOB_START_ERROR, result.ReturnValue.ToString());
                            return;
                        }
                        // wait for completion
                        if (!JobHelper.JobCompleted(vs, result.Job)) //TODO:
                        {
                            TaskManager.WriteError(VirtualizationErrorCodes.JOB_FAILED_ERROR, result.Job.ErrorDescription.ToString());
                            return;
                        }
                    }
                    #endregion

                    #region export machine
                    if (exportVps && !String.IsNullOrEmpty(exportPath))
                    {
                        TaskManager.Write("VPS_DELETE_EXPORT");
                        result = vs.ExportVirtualMachine(vm.VirtualMachineId, exportPath);

                        // check result
                        if (result.ReturnValue != ReturnCode.JobStarted)
                        {
                            TaskManager.WriteError(VirtualizationErrorCodes.JOB_START_ERROR, result.ReturnValue.ToString());
                            return;
                        }

                        // wait for completion
                        if (!JobHelper.JobCompleted(vs, result.Job))
                        {
                            TaskManager.WriteError(VirtualizationErrorCodes.JOB_FAILED_ERROR, result.Job.ErrorDescription.ToString());
                            return;
                        }
                    }
                    #endregion
                    
                    #region delete machine
                    TaskManager.Write("VPS_DELETE_DELETE");
                    result = saveFiles ? vs.DeleteVirtualMachine(vm.VirtualMachineId, vm.ClusterName) : vs.DeleteVirtualMachineExtended(vm.VirtualMachineId, vm.ClusterName);

                    // check result
                    if (result.ReturnValue != ReturnCode.JobStarted)
                    {
                        TaskManager.WriteError(VirtualizationErrorCodes.JOB_START_ERROR, result.ReturnValue.ToString());
                        if (!string.IsNullOrEmpty(result.Job.ErrorDescription))
                        {
                            TaskManager.WriteError("Error: {0}", result.Job.ErrorDescription);
                        }
                        return;
                    }
                    // wait for completion
                    if (!JobHelper.JobCompleted(vs, result.Job))
                    {
                        TaskManager.WriteError(VirtualizationErrorCodes.JOB_FAILED_ERROR, result.Job.ErrorDescription.ToString());
                        return;
                    }
                    #endregion
                }

                // mark as deleted
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Deleted;

                #region delete Empty folders
                if (!saveFiles)
                {
                    TaskManager.Write("VPS_DELETE_FILES", vm.RootFolderPath);
                    try
                    {
                        if (vs.IsEmptyFolders(vm.RootFolderPath)) //Prevent a possible hack to delete all files from the Main server :D
                        { //not necessarily, we are guaranteed to delete files using DeleteVirtualMachineExtended, left only for deleting folder :)
                            vs.DeleteRemoteFile(vm.RootFolderPath);//TODO: replace by powershell ???
                            TaskManager.Write(String.Format("The VM files were deleted."));
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex, VirtualizationErrorCodes.DELETE_VM_FILES_ERROR + ":");
                    }
                }
                #endregion                
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, VirtualizationErrorCodes.DELETE_ERROR);
                //return;
            }
            finally
            {
                bool isSuccessDeleted = vm.ProvisioningStatus == VirtualMachineProvisioningStatus.Deleted;

                if (keepPackageItem && isSuccessDeleted) //For reinstall.
                {
                    vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Deleted;
                    PackageController.UpdatePackageItem(vm);
                    TaskManager.Write(String.Format("Preparing VM to reinstalling."));
                }
                else if (isSuccessDeleted) //Pernament delete server and all data
                {
                    PackageController.DeletePackageItem(itemId);
                    TaskManager.Write(String.Format("The VM was deleted."));
                }
                else
                {
                    vm.CurrentTaskId = null;
                    vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
                    PackageController.UpdatePackageItem(vm); //to access the audit log.
                }

                //// complete task
                //TaskManager.CompleteTask();
            }
        }
    }
}
