<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BackupWizard.ascx.cs" Inherits="SolidCP.Portal.BackupWizard" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
    <table cellpadding="3" cellspacing="0">
        <tr>
            <td class="Huge" colspan="2">
                <asp:Literal ID="litBackupType" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="SubHead" style="width:200px">
                <asp:Label ID="lblBackupFile" runat="server" meta:resourcekey="lblBackupFile" Text="Backup File Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtBackupFileName" runat="server" CssClass="NormalTextBox" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireBackupFileName" runat="server" Display="Dynamic" ControlToValidate="txtBackupFileName"
                    ErrorMessage="*" ValidationGroup="Backup" meta:resourcekey="valRequireBackupFileName"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblBackupDestination" runat="server" meta:resourcekey="lblBackupDestination" Text="Copy Backup To:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlDestination" runat="server" CssClass="NormalTextBox" resourcekey="ddlDestination" AutoPostBack="True" OnSelectedIndexChanged="ddlDestination_SelectedIndexChanged">
                    <asp:ListItem Value="1">SpaceFolder</asp:ListItem>
                    <asp:ListItem Value="2">ServerFolder</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="SpaceFolderPanel" runat="server">
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td class="SubHead" style="width:200px">
                    <asp:Label ID="lblSpace" runat="server" meta:resourcekey="lblSpace" Text="Space:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSpace" runat="server" CssClass="NormalTextBox" AutoPostBack="True" OnSelectedIndexChanged="ddlSpace_SelectedIndexChanged">
                    </asp:DropDownList>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblSpaceFolder" runat="server" meta:resourcekey="lblSpaceFolder" Text="Folder:"></asp:Label>
                </td>
                <td>
                    <scp:FileLookup id="spaceFolder" runat="server" ValidationGroup="Backup">
                    </scp:FileLookup>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="ServerFolderPanel" runat="server">
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td class="SubHead" style="width:200px">
                    <asp:Label ID="lblServerPath" runat="server" meta:resourcekey="lblServerPath" Text="Path:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtServerPath" runat="server" CssClass="NormalTextBox" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireServerPath" runat="server" Display="Dynamic" ControlToValidate="txtServerPath"
                        ErrorMessage="*" ValidationGroup="Backup" meta:resourcekey="valRequireServerPath"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table cellpadding="3" cellspacing="0">
        <tr>
            <td style="width:200px">
            </td>
            <td class="SubHead">
                <asp:CheckBox ID="chkDeleteBackup" runat="server" meta:resourcekey="chkDeleteBackup" Checked="true" Text="Delete backup after copying" />
            </td>
        </tr>
    </table>
    <asp:Repeater runat="server" ID="rptBackupSetSummary">
		<HeaderTemplate>
			<div class="NormalBold" style="margin-bottom: 10px;"><asp:Localize runat="server" meta:resourcekey="lblBackupSetSummary" /></div>
			<div style="padding: 5px 5px 5px 5px; height: 180px; overflow: auto;" class="BorderFillBox">
		</HeaderTemplate>
		<ItemTemplate>
			<div>
				<asp:Label runat="server" CssClass="NormalBold" Text='<%# String.Concat(GetSharedLocalizedString("ServiceItemType." + Container.DataItem), GetLocalizedString("ServiceItemTypes.PluralForm")) %>' />
				<asp:BulletedList runat="server" SkinID="BackupSetSummary" 
					DataSource='<%# GetResourceGroupServiceItems((string)Container.DataItem) %>' />
			</div>
		</ItemTemplate>
		<FooterTemplate>
			</div>
		</FooterTemplate>
    </asp:Repeater>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnBackup" CssClass="btn btn-success" runat="server" OnClick="btnBackup_Click" ValidationGroup="Backup"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBackup"/> </CPCC:StyleButton>
</div>