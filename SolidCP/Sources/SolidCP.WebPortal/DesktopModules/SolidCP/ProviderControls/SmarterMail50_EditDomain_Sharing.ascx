<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail50_EditDomain_Sharing.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail50_EditDomain_Sharing" %>
<table width="100%">
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label1" runat="server" meta:resourcekey="cbGlobalAddressList"/></td>
        <td><asp:CheckBox runat="server" ID="cbGlobalAddressList"  /></td>        
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label2" runat="server" meta:resourcekey="cbSharedCalendars" /></td>
        <td><asp:CheckBox runat="server" ID="cbSharedCalendars" /></td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label3" runat="server" meta:resourcekey="cbSharedContacts" /></td>
        <td><asp:CheckBox runat="server" ID="cbSharedContacts" /></td>        
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label4" runat="server" meta:resourcekey="cbSharedFolders"/></td>
        <td><asp:CheckBox runat="server" ID="cbSharedFolders" /></td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label5" runat="server" meta:resourcekey="cbSharedNotes" /></td>
        <td><asp:CheckBox runat="server" ID="cbSharedNotes"  /></td>        
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label6" runat="server" meta:resourcekey="cbSharedTasks" /></td>
        <td><asp:CheckBox runat="server" ID="cbSharedTasks"  /></td>
    </tr>
</table>