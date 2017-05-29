<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MSSQL_EditUser.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MSSQL_EditUser" %>
<table cellSpacing="0" cellPadding="3" width="100%">
    <tr>
        <td class="SubHead" style="width:150px;"><asp:Label ID="lblDefaultDatabase" runat="server" meta:resourcekey="lblDefaultDatabase" Text="Default database:"></asp:Label></td>
        <td>
            <asp:DropDownList id="ddlDefaultDatabase" runat="server" DataTextField="Name" DataValueField="Name" CssClass="form-control"></asp:DropDownList>
        </td>
    </tr>
</table>