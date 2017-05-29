<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlackBerry_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.BlackBerry_Settings" %>
<table>
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblPath" meta:resourcekey="lblPath" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtPath" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPath" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblPassword" meta:resourcekey="lblPassword" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtPassword" MaxLength="256" Width="200px"  TextMode="Password"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblEnterpriseServer" meta:resourcekey="lblEnterpriseServer" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtEnterpriseServer" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEnterpriseServer" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblBesAdminServiceHost" meta:resourcekey="lblBesAdminServiceHost" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtBesAdminServiceHost" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBesAdminServiceHost" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
</table>