using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.UseCase
{
    public class ReinstallVirtualMachineHandler: ControllerBase
    {
        public ReinstallVirtualMachineHandler(ControllerBase provider): base(provider) { }

        public IntResult ReinstallVirtualMachine(int itemId, VirtualMachine VMSettings, string adminPassword, string[] privIps, string[] dmzIps,
            bool saveVirtualDisk, bool exportVps, string exportPath)
        {
            IntResult res = new IntResult();

            #region Maintenance Mode Check
            if (VirtualizationHelper.IsMaintenanceMode(itemId))
            {
                res.ErrorCodes.Add(VirtualizationErrorCodes.MAINTENANCE_MODE_IS_ENABLE);
                return res;
            }
            #endregion

            int PackageId = VMSettings.PackageId;
            if (string.IsNullOrEmpty(VMSettings.OperatingSystemTemplatePath)) //check if we lose VMSettings 
            {
                
                VMSettings = VirtualMachineHelper.GetVirtualMachineByItemId(itemId);
                if (VMSettings == null)
                {
                    res.ErrorCodes.Add(VirtualizationErrorCodes.CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM);
                    return res;
                }
                VMSettings.OperatingSystemTemplate = Path.GetFileName(VMSettings.OperatingSystemTemplatePath);
                VMSettings.PackageId = PackageId;

                
            }

            #region Context variables
            // service ID
            int serviceId = VMSettings.ServiceId;

            // load service settings
            StringDictionary settings = ServerController.GetServiceSettings(serviceId);
            #endregion

            string osTemplateFile = VMSettings.OperatingSystemTemplate;

            //TODO: move to a class Load OS template ? <============================
            #region load OS templates
            // load OS templates
            LibraryItem osTemplate = null;

            try
            {
                bool isTemplateExist = false;
                LibraryItem[] osTemplates = VirtualizationHelper.GetOperatingSystemTemplates(VMSettings.PackageId);
                foreach (LibraryItem item in osTemplates)
                {
                    if (String.Compare(item.Path, osTemplateFile, true) == 0)
                    {
                        osTemplate = item;
                        isTemplateExist = true;

                        // check minimal disk size
                        if (osTemplate.DiskSize > 0 && VMSettings.HddSize[0] < osTemplate.DiskSize)
                        {
                            TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.QUOTA_TEMPLATE_DISK_MINIMAL_SIZE + ":" + osTemplate.DiskSize);
                            return res;
                        }
                        if (osTemplate.Generation < 1)
                        {
                            //TODO: add a special errorCode?
                            throw new ApplicationException("The generation of VM was not configured in the template");
                        }
                        VMSettings.Generation = osTemplate.Generation;
                        VMSettings.SecureBootTemplate = osTemplate.SecureBootTemplate;
                        VMSettings.EnableSecureBoot = osTemplate.Generation == 1 ? false : osTemplate.EnableSecureBoot;
                        VMSettings.OperatingSystemTemplate = osTemplate.Name;
                        VMSettings.LegacyNetworkAdapter = osTemplate.LegacyNetworkAdapter;
                        VMSettings.RemoteDesktopEnabled = osTemplate.RemoteDesktop;
                        break;
                    }
                }
                if (!isTemplateExist)
                { //give a description of the error for Third party services if they use SOAP
                    //TaskManager.WriteError("The template " + osTemplateFile + " was not found in the HyperV Service Template Library.");
                    //TODO: add a special errorCode?
                    throw new ApplicationException("The template " + osTemplateFile + " was not found in the HyperV Service Template Library.");
                }
            }
            catch (ApplicationException ex)
            {
                TaskManager.CompleteResultTask(res, null, null, ex.Message);
                return res;
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, null, ex);
                return res;
            }
            #endregion


            VMSettings.Version = VirtualizationUtils.GetHyperVConfigurationVersionFromSettings(settings);

            #region setup VM paths
            // setup VM paths
            VMSettings.RootFolderPath = VirtualizationUtils.GetCorrectVmRootFolderPath(settings, VMSettings);

            string templatesPath = settings["OsTemplatesPath"];
            var correctVhdPath = VirtualizationUtils.GetCorrectTemplateFilePath(templatesPath, osTemplateFile);
            VMSettings.OperatingSystemTemplatePath = correctVhdPath;
            VMSettings.VirtualHardDrivePath = VirtualizationUtils.GetCorrectVmVirtualHardDrivePaths(VMSettings.HddSize.Length, VMSettings.Name, VMSettings.RootFolderPath, Path.GetExtension(correctVhdPath));

            #endregion

            try
            {
                VMSettings.CurrentTaskId = Guid.NewGuid().ToString("N"); // generate reinstall task id
                VMSettings.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;
                PackageController.UpdatePackageItem(VMSettings);
                #region Start Asynchronous task
                try
                {
                    VirtualizationAsyncWorker2012 worker = new VirtualizationAsyncWorker2012
                    {
                        ThreadUserId = SecurityContext.User.UserId,
                        Vm = VMSettings,
                        OsTemplate = osTemplate,
                        ItemId = itemId,
                        AdminPassword = adminPassword,
                        PrivIps = privIps,
                        DmzIps = dmzIps,
                        SaveFiles = saveVirtualDisk,
                        ExportVps = exportVps,
                        ExportPath = exportPath,
                    };
                    worker.ReinstallVPSAsync();
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
            res.Value = VMSettings.Id;
            return res;
        }
    }
}
