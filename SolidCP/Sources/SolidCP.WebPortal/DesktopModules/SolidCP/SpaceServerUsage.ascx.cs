using Microsoft.Web.Services3.Addressing;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.UI;

namespace SolidCP.Portal
{
    public partial class SpaceServerUsage : SolidCPModuleBase
    {
        private PackageContext cntx;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ContainerControl.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);

            if (!IsPostBack)
            {
                FillNA();
                Timer1.Enabled = true;  //Enable timer to post-load Resource Usage (to prevent slow page loading)
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if(PanelSecurity.PackageId != 1) // PackageId 1 is the serveradmin package
                BindSpaceServerUsage();

            Timer1.Enabled = false; //disable timer, after getting usage information
        }

        private SystemResourceUsageInfo GetSystemResourceUsage()
        {
            cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            PackageInfo packageInfo = cntx.Package;
            // TODO: We need to find a way to detect whether other services have a Remote Computer setting.
            // As of 2025, this setting exists only for Hyper-V (VPS2012).
            // In other cases, we assume it's not Hyper-V and they don't have Remote Computer settings.
            ServiceInfo serviceInfo = ES.Services.Servers.GetServicesByServerIdGroupName(packageInfo.ServerId, ResourceGroups.VPS2012).FirstOrDefault();
            if (serviceInfo != null)
                return ES.Services.VPS2012.GetSystemResourceUsageInfo(serviceInfo.ServiceId);

            return ES.Services.Servers.GetSystemResourceUsageInfo(packageInfo.ServerId);
        }

        private void BindSpaceServerUsage()
        {            
            try
            {
                SystemResourceUsageInfo resourceUsage = GetSystemResourceUsage();
                int cpuUsage = 0;
                if (resourceUsage.LogicalProcessorUsagePercent != -1)
                    cpuUsage =  resourceUsage.LogicalProcessorUsagePercent; //this is more accurate if installed Hyper-V
                else
                    cpuUsage = resourceUsage.ProcessorTimeUsagePercent; //this is for everything else

                usageCpu.Text = cpuUsage.ToString();
                cpuGauge.Progress = cpuUsage;
                totalCpu.Text = cpuGauge.Total.ToString();

                freeMemory.Text = (resourceUsage.SystemMemoryInfo.FreePhysicalKB / 1024).ToString();
                totalMemory.Text = (resourceUsage.SystemMemoryInfo.TotalVisibleSizeKB / 1024).ToString();
                ramGauge.Total = (int)resourceUsage.SystemMemoryInfo.TotalVisibleSizeKB / 1024;
                ramGauge.Progress = (int)((resourceUsage.SystemMemoryInfo.TotalVisibleSizeKB / 1024) - (resourceUsage.SystemMemoryInfo.FreePhysicalKB / 1024));
            }
            catch
            {
                FillNA();
            }
        }

        private void FillNA()
        {
            usageCpu.Text = totalCpu.Text =
                    freeMemory.Text = totalMemory.Text = "N/A";
        }
    }
}