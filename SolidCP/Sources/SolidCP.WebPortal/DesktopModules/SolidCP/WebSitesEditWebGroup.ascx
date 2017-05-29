<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditWebGroup.ascx.cs" Inherits="SolidCP.Portal.WebSitesEditWebGroup" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="panel-body form-horizontal">
<table cellspacing="0" cellpadding="0" width="100%">
	<tr>
		<td>
            <table cellSpacing="0" cellPadding="5" width="100%">
	            <tr>
		            <td class="SubHead" style="width: 150px;"><asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="Group name:"></asp:Label></td>
		            <td class="NormalBold">
                        <uc2:UsernameControl ID="usernameControl" runat="server" />
                    </td>
	            </tr>
            </table>
            
            <scp:CollapsiblePanel id="secUsers" runat="server"
                TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Members">
            </scp:CollapsiblePanel>
	        <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
                <table cellSpacing="0" cellPadding="3" width="100%">
	                <tr>
		                <td colspan="2">
			                <asp:checkboxlist id="dlUsers" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
				                DataValueField="Name" DataTextField="Name"></asp:checkboxlist>
		                </td>
	                </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>
</div>