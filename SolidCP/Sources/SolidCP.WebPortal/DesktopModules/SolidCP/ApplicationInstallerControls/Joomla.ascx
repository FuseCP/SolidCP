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
			inst["ObjectQualifier"] = txtQualifier.Text.Trim();
			inst["ObjectQualifierNormalized"] = (txtQualifier.Text.Trim() == "") ? "" : txtQualifier.Text.Trim() + "_";
			inst["admin.username"] = txtUsername.Text;
            	inst["admin.password"] = MD5(txtPassword.Text);
			inst["admin.email"] = txtEmail.Text;
			inst["InstallDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            if (chkCreateSimple.Checked)
                inst["CreateSimple"] = "Yes";
			
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
		<td class="SubHead" width="200" nowrap>
			Database objects qualifier:
		</td>
		<td width="100%">
			<asp:TextBox id="txtQualifier" runat="server" CssClass="form-control" Text="" MaxLength="5"
				Columns="5"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td></td>
		<td class="Normal">
			Several instances of Joomla can work with the same database simultaneously. 
			They are separated by mean of database object qualifiers which are just 
			prefixes for database tables, stored procedures, etc.<br/><br/>
			So, if you install your first instance of Joomla on the selected database, 
			leave this field blank; otherwise, specify some value, for example 'jos'.
		</td>
	</tr>
	<tr>
		<td class=SubHead colspan=2>
			Enter the username and password you would like to use for the administrator account
			of your new Joomla site:
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
			<asp:Checkbox id="chkCreateSimple" runat=server Text="Create Sample Data" Checked=false />
		</td>
	</tr>
</table>