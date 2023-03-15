using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS;
using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public static class UpdateVirtualMachineHostNameHandler
    {
        public static ResultObject UpdateVirtualMachineHostName(int itemId, string hostname, bool updateNetBIOS)
        {
            if (String.IsNullOrEmpty(hostname))
                throw new ArgumentNullException("hostname");

            ResultObject res = new ResultObject();

            // load service item
            VirtualMachine vm = (VirtualMachine)PackageController.GetPackageItem(itemId);
            if (vm == null)
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                return res;
            }

            PowerShellScript.CheckCustomPsScript(PsScriptPoint.before_renaming, vm);

            #region Check account and space statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, vm.PackageId, DemandPackage.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("VPS2012", "UPDATE_HOSTNAME", vm.Id, vm.Name, vm.PackageId);

            try
            {
                // get proxy
                VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);

                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                vm.ClusterName = (Utils.ParseBool(settings["UseFailoverCluster"], false)) ? settings["ClusterName"] : null;

                // update virtual machine name
                JobResult result = vs.RenameVirtualMachine(vm.VirtualMachineId, hostname, vm.ClusterName);
                if (result.ReturnValue != ReturnCode.OK)
                {
                    LogHelper.LogReturnValueResult(res, result);
                    TaskManager.CompleteResultTask(res);
                    return res;
                }

                // update meta item
                vm.Name = hostname;
                PackageController.UpdatePackageItem(vm);

                // update NetBIOS name if required
                if (updateNetBIOS)
                {
                    result = KvpExchangeHelper.SendComputerNameKVP(itemId, hostname);
                    if (result.ReturnValue != ReturnCode.JobStarted
                        && result.Job.JobState == ConcreteJobState.Completed)
                    {
                        LogHelper.LogReturnValueResult(res, result);
                        TaskManager.CompleteResultTask(res);
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CHANGE_ADMIN_PASSWORD_ERROR, ex);
                return res;
            }

            PowerShellScript.CheckCustomPsScript(PsScriptPoint.after_renaming, vm);

            TaskManager.CompleteResultTask();
            return res;
        }
    }
}
