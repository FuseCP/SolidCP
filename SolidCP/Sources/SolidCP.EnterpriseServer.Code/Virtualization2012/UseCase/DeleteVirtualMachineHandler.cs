using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Virtualization2012;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public static class DeleteVirtualMachineHandler
    {
        //TODO: Need to find how to rework it
        public static ResultObject DeleteVirtualMachine(int itemId, bool saveFiles, bool exportVps, string exportPath) //TODO: Is possible to rework method (Duplicated in server)?
        {
            ResultObject res = new ResultObject();

            #region Maintenance Mode Check
            if (VirtualizationHelper.IsMaintenanceMode(itemId))
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.MAINTENANCE_MODE_IS_ENABLE);
                return res;
            }
            #endregion

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            PowerShellScript.CheckCustomPsScript(PsScriptPoint.before_deletion, vm);

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS", "DELETE", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                // check VM state
                VirtualMachine vps = vs.GetVirtualMachine(vm.VirtualMachineId);

                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                vm.ClusterName = (Utils.ParseBool(settings["UseFailoverCluster"], false)) ? settings["ClusterName"] : null;

                JobResult result = null;

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
                            LogHelper.LogReturnValueResult(res, result);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }

                        // wait for completion
                        if (!JobHelper.JobCompleted(vs, result.Job))
                        {
                            LogHelper.LogJobResult(res, result.Job);
                            TaskManager.CompleteResultTask(res);
                            return res;
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
                            LogHelper.LogReturnValueResult(res, result);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }

                        // wait for completion
                        if (!JobHelper.JobCompleted(vs, result.Job))
                        {
                            LogHelper.LogJobResult(res, result.Job);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }
                    }
                    #endregion

                    #region delete machine
                    TaskManager.Write("VPS_DELETE_DELETE");
                    result = saveFiles ? vs.DeleteVirtualMachine(vm.VirtualMachineId, vm.ClusterName) : vs.DeleteVirtualMachineExtended(vm.VirtualMachineId, vm.ClusterName);

                    // check result
                    if (result.ReturnValue != ReturnCode.JobStarted)
                    {
                        LogHelper.LogReturnValueResult(res, result);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }

                    // wait for completion
                    if (!JobHelper.JobCompleted(vs, result.Job))
                    {
                        LogHelper.LogJobResult(res, result.Job);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                    #endregion
                }

                #region delete files
                if (!saveFiles)
                {
                    TaskManager.Write("VPS_DELETE_FILES", vm.RootFolderPath);
                    try
                    {
                        if (vs.IsEmptyFolders(vm.RootFolderPath)) //Prevent a possible hack to delete all files from the Main server :D
                            //not necessarily, we are guaranteed to delete files using DeleteVirtualMachineExtended, left only for deleting folder :)
                            vs.DeleteRemoteFile(vm.RootFolderPath);//TODO: replace by powershell ???
                    }
                    catch (Exception ex)
                    {
                        res.ErrorCodes.Add(VirtualizationErrorCodes.DELETE_VM_FILES_ERROR + ": " + ex.Message);
                    }
                }
                #endregion

                // delete meta item
                PackageController.DeletePackageItem(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.DELETE_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteVirtualMachineAsynchronous(int itemId, bool saveFiles, bool exportVps, string exportPath)
        {
            bool keepPackageItem = false;
            ResultObject res = new ResultObject();

            #region Maintenance Mode Check
            if (VirtualizationHelper.IsMaintenanceMode(itemId))
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.MAINTENANCE_MODE_IS_ENABLE);
                return res;
            }
            #endregion

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }
            else if (vm.ProvisioningStatus == VirtualMachineProvisioningStatus.DeletionProgress) //If someone tries to send 1 request twice.
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.DELETE_ERROR);
                return res;
            }

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            try
            {
                vm.CurrentTaskId = Guid.NewGuid().ToString("N"); // generate deletion task id
                vm.ProvisioningStatus = VirtualMachineProvisioningStatus.DeletionProgress;
                PackageController.UpdatePackageItem(vm);

                #region Start Asynchronous task
                try
                {
                    VirtualizationAsyncWorker2012 worker = new VirtualizationAsyncWorker2012
                    {
                        ThreadUserId = SecurityContext.User.UserId,
                        Vm = vm,
                        ItemId = itemId,
                        SaveFiles = saveFiles,
                        ExportVps = exportVps,
                        ExportPath = exportPath,
                        KeepPackageItem = keepPackageItem
                    };
                    worker.DeleteVPSAsync();
                }
                catch (Exception ex)
                {
                    res.AddError(VirtualizationErrorCodes.CREATE_TASK_START_ERROR, ex);
                    return res;
                }
                #endregion
            }
            catch (Exception ex)
            {
                res.AddError(VirtualizationErrorCodes.DELETE_ERROR, ex);
                return res;
            }

            res.IsSuccess = true;
            return res;
        }
    }
}
