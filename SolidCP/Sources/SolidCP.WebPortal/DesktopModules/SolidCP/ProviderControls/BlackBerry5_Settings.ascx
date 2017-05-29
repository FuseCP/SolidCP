<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlackBerry5_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.BlackBerry5_Settings" %>
<table>
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblPath" meta:resourcekey="lblPath" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtPath" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPath" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
        <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblHandheldcleanupPath" meta:resourcekey="lblHandheldcleanupPath" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtHandheldcleanupPath" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtHandheldcleanupPath" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblUser" meta:resourcekey="lblUser" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtUser" MaxLength="256" Width="200px"    />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUser" Display="Dynamic" ErrorMessage="*" />
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
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblEnterpriseServerFQDN" meta:resourcekey="lblEnterpriseServerFQDN" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtEnterpriseServerFQDN" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEnterpriseServerFQDN" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>    

    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblMAPIProfile" meta:resourcekey="lblMAPIProfile" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtMAPIProfile" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtMAPIProfile" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>

</table>