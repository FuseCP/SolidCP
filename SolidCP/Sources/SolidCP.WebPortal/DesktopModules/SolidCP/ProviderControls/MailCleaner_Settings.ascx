<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailCleaner_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MailCleaner_Settings" %>
<table>
    <tr>
        <td class="Normal" width="200" >
            <asp:Localize runat="server" ID="locServerName" meta:resourcekey="locServerName"/>
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtServerName"  CssClass="form-control" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtServerName" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="Normal" width="200" >
            <asp:Localize runat="server" ID="locSimpleUrlBase" meta:resourcekey="locSimpleUrlBase"/>
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtSimpleUrlBase"  CssClass="form-control" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSimpleUrlBase" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
     

</table>
