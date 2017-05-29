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
			inst["weblog.title"] = txtWebLogTitle.Text.Replace("'", "''");
            inst["admin.password"] = MD5(txtPassword.Text);
			inst["admin.email"] = txtEmail.Text;
		}

    public static string MD5(string str)
    {
        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashedBytes = md5.ComputeHash(enc.GetBytes(str));

        string hashedString = "";

        for (int i = 0; i < hashedBytes.Length; i++)
            hashedString += Convert.ToString(hashedBytes[i], 16).PadLeft(2, '0');

        return hashedString.PadLeft(32, '0').ToLower();
    }
    
</script>
<table cellPadding="2" width="100%">
	<tr>
		<td align="left" class=SubHead>WebLog Title:</TD>
		<td align="left" class=Normal>
			<asp:textbox id="txtWebLogTitle" runat="server" CssClass=NormalTextBox Text="My WebLog"></asp:textbox>
		</td>
	</tr>
	<tr>
		<td align="left" width=200 nowrap class=SubHead>Username:</TD>
		<td align="left" width=100% class=Normal>
			<b>admin</b>
		</td>
	</tr>
	<tr>
		<td align="left" class=SubHead>Administrator Password:</TD>
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
		<td align="left" width=200 nowrap class=SubHead>Administrator E-Mail:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtEmail" runat="server" CssClass=NormalTextBox>admin@admin.com</asp:textbox>
		</td>
	</tr>
</table>