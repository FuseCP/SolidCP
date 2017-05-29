<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SolidCP.AWStats.Viewer.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Advanced Statistics</title>
    <style type="text/css">
	    <!--
	    body { margin: 0; }
	    .aws_border { border: 2px solid #CCCCDD; margin-top: 100px; }
	    .aws_title  { font: 13px verdana, arial, helvetica, sans-serif; font-weight: bold; background-color: #CCCCDD; text-align: center; padding: 3px; color: #000000; }
	    .aws_data { background-color: #FFFFFF; }
	    .aws_button {
		    font-family: arial,verdana,helvetica, sans-serif;
		    font-size: 12px;
		    border: 1px solid #ccd7e0;
		    background-image : url(icon/other/button.gif);
	    }
	    .aws_pad { padding: 3px; }
	    td { border-color: #ECECEC; border-left-width: 0px; border-right-width: 1px; border-top-width: 0px; border-bottom-width: 1px; font: 11px verdana, arial, helvetica, sans-serif; color: #000000; }
        p { font: 11px verdana, arial, helvetica, sans-serif; color: #000000; text-align: center; }
	    b { font-weight: bold; }
	    a { font: 11px verdana, arial, helvetica, sans-serif; }
	    a:link    { color: #0011BB; text-decoration: none; }
	    a:visited { color: #0011BB; text-decoration: none; }
	    a:hover   { color: #605040; text-decoration: underline; }
	    .currentday { font-weight: bold; }
	    //-->
    </style>
</head>
<body>
    <form id="form1" runat="server">
		<table class="aws_border" align="center" cellPadding="10" cellspacing="0" width="300px">
			<tr>
				<td class="aws_title">Login to Advanced Statistics</td>
			</tr>
			<tr>
				<td align="center">
					<table class="aws_data">
						<tr>
							<td colspan="2" align="center">
								<asp:Label ID="lblMessage" Runat="server" ForeColor="Red"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="aws"><b>Domain:</b></td>
							<td>
								<asp:TextBox id="txtDomain" runat="server" Width="150px"></asp:TextBox>
								<asp:RequiredFieldValidator id="valRequireDomain" runat="server" ErrorMessage="*" ControlToValidate="txtDomain"></asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td class="aws">Username:</td>
							<td>
								<asp:TextBox id="txtUsername" runat="server" Width="150px"></asp:TextBox>
								<asp:RequiredFieldValidator id="valRequireUsername" runat="server" ErrorMessage="*" ControlToValidate="txtUsername"></asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td class="aws">Password:</td>
							<td>
								<asp:TextBox id="txtPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
								<asp:RequiredFieldValidator id="valRequirePassword" runat="server" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td class="aws_pad" colspan="2" align="center">
								<asp:Button id="btnView" runat="server" Text="Display Statistics" CssClass="aws_button" OnClick="btnView_Click"></asp:Button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<p>
		    * You should specify username and password that you use to login to
		    <a href="http://demo.SolidCP.net" target="_blank">http://demo.SolidCP.net</a>.
		</p>
    </form>
</body>
</html>
