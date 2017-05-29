<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsSpacesFolder.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsSpacesFolder" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="tblSpacesFolder" runat="server"
    TargetControlID="SpacesFolderPanel" meta:resourcekey="tblSpacesFolder" Text="Child Spaces Location Folder"/>
<asp:Panel ID="SpacesFolderPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblChildSpacesFolder" runat="server" meta:resourcekey="lblChildSpacesFolder" Text="Child Spaces Folder:"></asp:Label>
            </td>
            <td class="Normal" width="100%" valign="top">
                <asp:TextBox ID="txtSpacesFolder" runat="server" CssClass="form-control" Width="250px"></asp:TextBox>
                <br />
                <asp:Label ID="lblRelative" runat="server" meta:resourcekey="lblRelative" Text="* relative to this space root"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>