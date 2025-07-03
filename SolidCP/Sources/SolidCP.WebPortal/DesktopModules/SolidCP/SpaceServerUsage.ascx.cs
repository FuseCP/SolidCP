using SolidCP.EnterpriseServer;
using SolidCP.Portal.Services;
using SolidCP.Providers.Common;
using System;
using System.Linq;
using System.Web.UI;

namespace SolidCP.Portal
{
    public partial class SpaceServerUsage : SolidCPModuleBase
    {
        private const int DEFAULT_TIMEOUT = 10000; //10 sec
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
            { 
                using (var service = new ServerService())
                {
                    BindSpaceServerUsage(service.GetServerUsageData(PanelSecurity.PackageId, DEFAULT_TIMEOUT));
                }
            }

            Timer1.Enabled = false; //disable timer, after getting usage information
        }

        private void BindSpaceServerUsage(SystemResourceUsageInfo resourceUsage)
        {            
            try
            {
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