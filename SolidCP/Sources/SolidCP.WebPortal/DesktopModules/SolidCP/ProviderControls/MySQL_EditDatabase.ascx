<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MySQL_EditDatabase.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MySQL_EditDatabase" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<scp:CollapsiblePanel id="secDataFiles" runat="server" IsCollapsed="true"
    TargetControlID="FilesPanel" meta:resourcekey="secDataFiles" Text="Database Files">
</scp:CollapsiblePanel>
<asp:Panel ID="FilesPanel" runat="server" Height="0" style="overflow:hidden;">
    <table id="tblFiles" runat="server" width="100%" cellpadding="3">
        <tr>
            <td style="width: 150px;" class="Medium">
                <asp:Label ID="lblDataFile" runat="server" meta:resourcekey="lblDataFile" Text="Data File"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="3">
	                <tr>
		                <td class="SubHead" nowrap><asp:Label ID="lblDataSize" runat="server" meta:resourcekey="lblSize" Text="Size, KB:"></asp:Label></td>
		                <td class="Normal"><asp:Literal id="litDataSize" Runat="server" Text="0"></asp:Literal></td>
	                </tr>
                </table>
            </td>
        </tr>
    </table> 
</asp:Panel>
<scp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true"
    TargetControlID="MainToolsPanel" meta:resourcekey="secMainTools" Text="Maintenance Tools">
</scp:CollapsiblePanel>
<asp:Panel ID="MainToolsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table cellpadding="10">
        <tr>
            <td>
                <CPCC:StyleButton id="btnBackup" CssClass="btn btn-primary" runat="server" OnClick="btnBackup_Click" CausesValidation="false"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBackupText"/> </CPCC:StyleButton>&nbsp;
                <CPCC:StyleButton id="btnRestore" CssClass="btn btn-warning" runat="server" OnClick="btnRestore_Click" CausesValidation="false"> <i class="fa fa-repeat">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRestoreText"/> </CPCC:StyleButton>
            </td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secHousekeepingTools" runat="server" IsCollapsed="true"
    TargetControlID="HousekeepingToolsPanel" meta:resourcekey="secHousekeepingTools" Text="Housekeeping Tools">
</scp:CollapsiblePanel>
<asp:Panel ID="HousekeepingToolsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Button ID="btnTruncate" runat="server" meta:resourcekey="btnTruncate" CausesValidation="false" 
                    Text="Truncate Files" CssClass="btn btn-primary" OnClick="btnTruncate_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>