<%@ Control Language="c#" AutoEventWireup="true" %>
<%@ Implements interface="SolidCP.Portal.IWebInstallerSettings" %>
<%@ Import namespace="SolidCP.EnterpriseServer" %>
<%@ Import Namespace="System.Text" %>

<script language="C#" runat="server">
		void Page_Load()
		{
		}
		
		public void GetSettings(InstallationInfo inst)
		{
			inst["scp.portalname"] = txtPortalName.Text;
			inst["scp.enterpriseserver"] = txtEsURL.Text;
		}
</script>
<table cellPadding="2" width="100%">
	<tr>
		<td align="left" width=200 nowrap class=SubHead>Portal Name:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtPortalName" runat="server" CssClass="form-control" Width="200px">SolidCP</asp:textbox>
		</td>
	</tr>
	<tr>
		<td align="left" width=200 nowrap class=SubHead>Enterprise Server URL:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtEsURL" runat="server" CssClass="form-control" Width="200px">http://127.0.0.1:9002</asp:textbox>
		</td>
	</tr>
</table>