<%@ Control Language="c#" AutoEventWireup="true" %>
<%@ Implements interface="SolidCP.Portal.IWebInstallerSettings" %>
<%@ Import namespace="SolidCP.EnterpriseServer" %>
<script language="C#" runat="server">
		void Page_Load()
		{
		}
		
		public void GetSettings(InstallationInfo inst)
		{
			if(chkSample.Checked)
				inst["InstallSampleData"] = "True";
		}
</script>
<table cellPadding="2" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap valign="top">
			Install sample store data:
		</td>
		<td width="100%" class=NormalBold>
			<asp:CheckBox id="chkSample" runat="server" CssClass="NormalBold" Text="Yes"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td>
		
		</td>
		<td class=Normal>
			Ticking this checkbox allows you to upload the sample data into your store database.
		</td>
	</tr>
</table>
