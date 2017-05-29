<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer43_EditDomain.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.hMailServer43_EditDomain" %>
<table width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal" width="100%">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" CssClass="form-control">
            </asp:DropDownList></td>
    </tr>
</table>