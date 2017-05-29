<%@ Control Language="c#" AutoEventWireup="true" %>
<%@ Implements interface="SolidCP.Portal.IWebInstallerSettings" %>
<%@ Import namespace="SolidCP.EnterpriseServer" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Security.Cryptography" %>

<script language="C#" runat="server">
		void Page_Load()
		{
		}
		
		public void GetSettings(InstallationInfo inst)
		{
			inst["ValidationKey"] = CreateCryptoKey(20);
			inst["DecryptionKey"] = CreateCryptoKey(24);
				
			inst["admin.username"] = txtUsername.Text;
			inst["admin.password"] = txtPassword.Text;
			inst["admin.email"] = txtEmail.Text;
			inst["createSamples"] = chkCreateSample.Checked ? "1" : "0";
		}
		
		protected string CreateCryptoKey(int len)
		{
			byte[] bytes = new byte[len];
			new RNGCryptoServiceProvider().GetBytes(bytes);
	        
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < bytes.Length; i++)
			{	
				sb.Append(string.Format("{0:X2}",bytes[i]));
			}
			
			return sb.ToString();
		}
</script>
<table cellPadding="2" width="100%">
	<tr>
		<td class=SubHead colspan=2>
			Enter the username and password you would like to use for the administrator account
			of your new Community Server site:
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
	<tr>
		<td align="left" width=200 nowrap class=SubHead>E-Mail:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtEmail" runat="server" CssClass=NormalTextBox>admin@site.com</asp:textbox>
		</td>
	</tr>
	<tr>
		<td align=left class=SubHead>&nbsp;</td>
		<td align=left class=NormalBold>
			<asp:Checkbox id="chkCreateSample" runat=server Text="Create Sample Data" Checked=false />
		</td>
	</tr>
</table>