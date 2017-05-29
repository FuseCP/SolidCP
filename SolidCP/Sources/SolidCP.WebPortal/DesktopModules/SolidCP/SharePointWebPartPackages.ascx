<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointWebPartPackages.ascx.cs" Inherits="SolidCP.Portal.SharePointWebPartPackages" %>
<div class="panel-body form-horizontal">
    <table cellspacing="0" cellpadding="3" width="100%">
	    <tr>
		    <td class="Huge" colspan="2"><asp:Literal id="litSiteName" runat="server"></asp:Literal></td>
	    </tr>
	    <tr>
	        <td colspan="2">
	            <br />
                <asp:Button id="btnInstall" runat="server" meta:resourcekey="btnInstall" Text="Install Package" CssClass="Button2" CausesValidation="false" OnClick="btnInstall_Click"/>
	        </td>
	    </tr>
	    <tr>
		    <td>
    		
        		
                <asp:ListBox ID="lbWebPartPackages" runat="server" Rows="10" Width="300px">
                </asp:ListBox>



		    </td>
		    <td valign="top" width="100%">
                <CPCC:StyleButton id="btnUninstall" CssClass="btn btn-danger" runat="server" OnClick="btnUninstall_Click" CausesValidation="false" OnClientClick="return confirm('Uninstall?');"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUninstallText"/> </CPCC:StyleButton>
		    </td>
	    </tr>
    </table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
</div>