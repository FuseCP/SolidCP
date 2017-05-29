<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Backup.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.Backup" %>
<%@ Register Src="../UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="scp" %>

	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap>
                <asp:Label ID="lblBackupFile" runat="server" meta:resourcekey="lblBackupFile" Text="Backup File Name:"></asp:Label>
            </td>
            <td class="SubHead" width="100%">
                <asp:TextBox ID="txtBackupFileName" runat="server" CssClass="form-control"  Width="95%" MaxLength="1000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireBackupFileName" runat="server" Display="Dynamic" ControlToValidate="txtBackupFileName"
                    ErrorMessage="*" ValidationGroup="Backup" meta:resourcekey="valRequireBackupFileName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
                <asp:Label ID="lblBackupDestination" runat="server" meta:resourcekey="lblBackupDestination" Text="Copy Backup To:"></asp:Label>
            </td>
            <td class="SubHead" width="100%">
                <asp:DropDownList ID="ddlDestination" runat="server" CssClass="NormalDropDown" meta:resourcekey="ddlDestination" AutoPostBack="True" OnSelectedIndexChanged="ddlDestination_SelectedIndexChanged">
                    <asp:ListItem Value="1" meta:resourcekey="ddlDestinationItem1">SpaceFolder</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ddlDestinationItem2">ServerFolder</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
	</table>
    <asp:Panel ID="SpaceFolderPanel" runat="server">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td class="SubHead" nowrap>
                    <asp:Label ID="lblSpace" runat="server" meta:resourcekey="lblSpace" Text="Space:"></asp:Label>
                </td>
                <td class="SubHead" width="100%">
                    <asp:DropDownList ID="ddlSpace" runat="server" CssClass="NormalDropDown" AutoPostBack="True" OnSelectedIndexChanged="ddlSpace_SelectedIndexChanged">
                    </asp:DropDownList>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="SubHead" nowrap>
                    <asp:Label ID="lblSpaceFolder" runat="server" meta:resourcekey="lblSpaceFolder" Text="Folder:"></asp:Label>
                </td>
                <td class="SubHead" width="100%">
                    <scp:FileLookup id="spaceFolder" runat="server" ValidationGroup="Backup">
                    </scp:FileLookup>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="ServerFolderPanel" runat="server">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td class="SubHead" nowrap>
                    <asp:Label ID="lblServerPath" runat="server" meta:resourcekey="lblServerPath" Text="Path:"></asp:Label>
                </td>
                <td class="SubHead" width="100%">
                    <asp:TextBox ID="txtServerPath" runat="server" CssClass="form-control" Width="95%" MaxLength="1000"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireServerPath" runat="server" Display="Dynamic" ControlToValidate="txtServerPath"
                        ErrorMessage="*" ValidationGroup="Backup" meta:resourcekey="valRequireServerPath"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table cellpadding="4" cellspacing="0">
        <tr>
            <td class="SubHead" width="100%">
                <asp:CheckBox ID="chkDeleteBackup" runat="server" meta:resourcekey="chkDeleteBackup" Checked="true" Text="Delete backup after copying" />
            </td>
        </tr>
    </table>
	