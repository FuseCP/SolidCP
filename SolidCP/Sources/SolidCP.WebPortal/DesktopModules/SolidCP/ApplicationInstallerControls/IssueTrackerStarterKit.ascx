<%@ Control Language="c#" AutoEventWireup="true" %>
<%@ Implements interface="SolidCP.Portal.IWebInstallerSettings" %>
<%@ Import namespace="SolidCP.EnterpriseServer" %>
<script language="C#" runat="server">
		void Page_Load()
		{
		}
		
		public void GetSettings(InstallationInfo inst)
		{
			inst["admin.username"] = txtUsername.Text;
			inst["admin.password"] = txtPassword.Text;
		}
</script>
<table cellPadding="2" width="100%">
	<tr>
		<td class=SubHead colspan=2>
			Enter the username and password you would like to use for the administrator account
			of your IssueTracker application:
		</td>
	</tr>
	<tr>
		<td align="left" width=200 nowrap class=SubHead>Username:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtUsername" runat="server" CssClass=NormalTextBox>admin</asp:textbox>
		</td>
	</tr>
	<tr>
		<td align="left" class=SubHead>Password:</TD>
		<td align="left" class=Normal>
			<asp:textbox id="txtPassword" runat="server" TextMode="Password" CssClass=NormalTextBox></asp:textbox>
		</td>
	</tr>
	<tr>
		<td align="left" class=SubHead>Confirm Password:</td>
		<td align="left" class=Normal>
			<asp:textbox id="txtConfirmPassword" runat="server" TextMode="Password" CssClass=NormalTextBox></asp:textbox>
			<asp:comparevalidator EnableClientScript=True Enabled=True id="ComparePassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Cssclass="color:red">*</asp:comparevalidator>
		</td>
	</tr>
</table>