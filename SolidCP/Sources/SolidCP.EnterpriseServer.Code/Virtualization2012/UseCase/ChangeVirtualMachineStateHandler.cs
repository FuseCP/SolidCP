using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Virtualization2012;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public static class ChangeVirtualMachineStateHandler
    {
        private const string SHUTDOWN_REASON = "SolidCP - Initiated by user";

        public static ResultObject ChangeVirtualMachineState(int itemId, VirtualMachineRequestedState state)
        {
            // start task
            ResultObject res = TaskManager.StartResultTask<ResultObject>("VPS2012", "CHANGE_STATE");

            try
            {
                // load service item
                VirtualMachine machine = (VirtualMachine)PackageController.GetPackageItem(itemId);
                if (machine == null)
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                    return res;
                }

                BackgroundTask topTask = TaskManager.TopTask;
                topTask.ItemId = machine.Id;
                topTask.ItemName = machine.Name;
                topTask.PackageId = machine.PackageId;

                TaskController.UpdateTask(topTask);

                TaskManager.WriteParameter("New state", state);

                // load proxy
                VirtualizationServer2012 vps = VirtualizationHelper.GetVirtualizationProxy(machine.ServiceId);

                try
                {
                    if (state == VirtualMachineRequestedState.ShutDown)
                    {
                        ReturnCode code = vps.ShutDownVirtualMachine(machine.VirtualMachineId, true, SHUTDOWN_REASON);
                        if (code != ReturnCode.OK)
                        {
                            res.ErrorCodes.Add(VirtualizationErrorCodes.JOB_START_ERROR + ":" + code);
                            TaskManager.CompleteResultTask(res);
                            return res;
                        }

                        // spin until fully stopped
                        VirtualMachine vm = vps.GetVirtualMachine(machine.VirtualMachineId);
                        short timeOut = 600; //10 min
                        while (vm.State != VirtualMachineState.Off)
                        {
                            timeOut--;
                            System.Threading.Thread.Sleep(1000); // sleep 1 second
                            vm = vps.GetVirtualMachine(machine.VirtualMachineId);
                            if (timeOut == 0)// turnoff
                            {
                                ResultObject turnOffResult = ChangeVirtualMachineState(itemId,
                                                                VirtualMachineRequestedState.TurnOff);
                                if (!turnOffResult.IsSuccess)
                                {
                                    TaskManager.CompleteResultTask(res);
                                    return turnOffResult;
                                }
                            }
                        }
                    }
                    else if (state == VirtualMachineRequestedState.Reboot)
                    {
                        // shutdown first
                        ResultObject shutdownResult = ChangeVirtualMachineState(itemId,
                            VirtualMachineRequestedState.ShutDown);
                        if (!shutdownResult.IsSuccess)
                        {
                            TaskManager.CompleteResultTask(res);
                            return shutdownResult;
                        }

                        // start machine
                        ResultObject startResult = ChangeVirtualMachineState(itemId, VirtualMachineRequestedState.Start);
                        if (!startResult.IsSuccess)
                        {
                            TaskManager.CompleteResultTask(res);
                            return startResult;
                        }
                    }
                    else if (state == VirtualMachineRequestedState.Reset)
                    {
                        // reset machine
                        JobResult result = vps.ChangeVirtualMachineState(machine.VirtualMachineId, VirtualMachineRequestedState.Reset, machine.ClusterName);

                        if (result.Job.JobState == ConcreteJobState.Completed)
                        {
                            LogHelper.LogReturnValueResult(res, result);
                            TaskManager.CompleteTask();
                            return res;
                        }
                        else
                        {
                            // check return
                            if (result.ReturnValue != ReturnCode.JobStarted)
                            {
                                LogHelper.LogReturnValueResult(res, result);
                                TaskManager.CompleteResultTask(res);
                                return res;
                            }

                            // wait for completion
                            if (!JobHelper.JobCompleted(vps, result.Job))
                            {
                                LogHelper.LogJobResult(res, result.Job);
                                TaskManager.CompleteResultTask(res);
                                return res;
                            }
                        }
                    }
                    else
                    {
                        if (state == VirtualMachineRequestedState.Resume)
                            state = VirtualMachineRequestedState.Start;

                        JobResult result = vps.ChangeVirtualMachineState(machine.VirtualMachineId, state, machine.ClusterName);

                        if (result.Job.JobState == ConcreteJobState.Completed)
                        {
                            LogHelper.LogReturnValueResult(res, result);
                            TaskManager.CompleteTask();
                            return res;
                        }
                        else
                        {
                            // check return
                            if (result.ReturnValue != ReturnCode.JobStarted)
                            {
                                LogHelper.LogReturnValueResult(res, result);
                                TaskManager.CompleteResultTask(res);
                                return res;
                            }

                            // wait for completion
                            if (!JobHelper.JobCompleted(vps, result.Job))
                            {
                                LogHelper.LogJobResult(res, result.Job);
                                TaskManager.CompleteResultTask(res);
                                return res;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.IsSuccess = false;
                    res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_CHANGE_VIRTUAL_SERVER_STATE);
                    TaskManager.WriteError(ex);
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorCodes.Add(VirtualizationErrorCodes.CHANGE_VIRTUAL_MACHINE_STATE_GENERAL_ERROR);
                TaskManager.WriteError(ex);
                return res;
            }

            TaskManager.CompleteTask();
            return res;
        }
    }
}
