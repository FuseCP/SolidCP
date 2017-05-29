<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditWebFolder.ascx.cs" Inherits="SolidCP.Portal.WebSitesEditWebFolder" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="panel-body form-horizontal">
<table cellSpacing="0" cellPadding="0" width="100%">
	<tr>
		<td>
            <table cellSpacing="0" cellPadding="5" width="100%">
	            <tr>
		            <td class="SubHead" style="width: 150px;">
						<asp:Label ID="lblFolderTitle" runat="server" meta:resourcekey="lblFolderTitle" Text="Folder Title:"></asp:Label>
					</td>
		            <td class="NormalBold">
                        <asp:TextBox ID="txtTitle" runat="server" Width="200" CssClass="NormalTextBox"></asp:TextBox>
                    </td>
	            </tr>
	            <tr>
		            <td class="SubHead"><asp:Label ID="lblFolderName" runat="server" meta:resourcekey="lblFolderName" Text="Folder Path:"></asp:Label></td>
		            <td class="NormalBold">
                        <uc1:FileLookup id="folderPath" runat="server" Width="250">
                        </uc1:FileLookup></td>
	            </tr>
            </table>
            
            <scp:CollapsiblePanel id="secUsers" runat="server"
                TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Allowed Users">
            </scp:CollapsiblePanel>
	        <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
                <table cellspacing="0" cellpadding="3" width="100%">
	                <tr>
		                <td colspan="2">
			                <asp:checkboxlist id="dlUsers" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
				                DataValueField="Name" DataTextField="Name"></asp:checkboxlist>
		                </td>
	                </tr>
                </table>
            </asp:Panel>
            
            <scp:CollapsiblePanel id="secGroups" runat="server"
                TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Allowed Groups">
            </scp:CollapsiblePanel>
	        <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
                <table cellspacing="0" cellpadding="3" width="100%">
	                <tr>
		                <td colspan="2">
			                <asp:checkboxlist id="dlGroups" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
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