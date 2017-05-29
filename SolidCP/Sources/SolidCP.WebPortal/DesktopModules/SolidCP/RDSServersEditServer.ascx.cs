using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class RDSServersEditServer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var rdsServer = ES.Services.RDS.GetRdsServer(PanelRequest.ServerId);
                lblServerName.Text = rdsServer.FqdName;
                txtServerComments.Text = rdsServer.Description;

                var serverInfo = ES.Services.RDS.GetRdsServerInfo(null, rdsServer.FqdName);
                litProcessor.Text = string.Format("{0}x{1} MHz", serverInfo.NumberOfCores, serverInfo.MaxClockSpeed);
                litLoadPercentage.Text = string.Format("{0}%", serverInfo.LoadPercentage);
                litMemoryAllocated.Text = string.Format("{0} MB", serverInfo.MemoryAllocatedMb);
                litFreeMemory.Text = string.Format("{0} MB", serverInfo.FreeMemoryMb);
                litStatus.Text = serverInfo.Status;
                rpServerDrives.DataSource = serverInfo.Drives;
                rpServerDrives.DataBind(); 
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            try
            {
                var rdsServer = ES.Services.RDS.GetRdsServer(PanelRequest.ServerId);                
                rdsServer.Description = txtServerComments.Text;

                ResultObject result = ES.Services.RDS.UpdateRdsServer(rdsServer);

                if (!result.IsSuccess && result.ErrorCodes.Count > 0)
                {
                    messageBox.ShowMessage(result, "RDSSERVER_NOT_UPDATED", "");
                    return;
                }

                RedirectToBrowsePage();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("RDSSERVER_NOT_UPDATED", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }
    }
}