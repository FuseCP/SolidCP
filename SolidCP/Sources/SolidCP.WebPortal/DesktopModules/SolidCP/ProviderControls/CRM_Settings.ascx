<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRM_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.CRM_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="scp" %>
<table>
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblSqlServer" meta:resourcekey="lblSqlServer" /></td>
        <td>                        
            <asp:TextBox runat="server" ID="txtSqlServer" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSqlServer" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblReportingService" meta:resourcekey="lblReportingService"/></td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" Width="200px" ID="txtReportingService" MaxLength="256" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReportingService" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblCrmDomainName" meta:resourcekey="lblCrmDomainName"/></td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" Width="200px" ID="txtDomainName" MaxLength="256" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDomainName" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblSchema" meta:resourcekey="lblSchema"/></td>
        <td class="Normal" width="100%">
          <asp:DropDownList runat="server" ID="ddlSchema">
            <asp:ListItem Text="http" Value="http" />
            <asp:ListItem Text="https" Value="https" />
          </asp:DropDownList>
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblCrmIP" meta:resourcekey="lblCrmIP"/></td>
        <td class="Normal" width="100%">
            <scp:SelectIPAddress ID="ddlCrmIpAddress" runat="server" ServerIdParam="ServerID" AllowEmptySelection="false" />            
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblPort" meta:resourcekey="lblPort"/></td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtPort" Width="200px" />
            <asp:RangeValidator runat="server" ControlToValidate="txtPort" Display="dynamic" ErrorMessage="*" Type="String" MinimumValue="0" MaximumValue="9" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label runat="server" ID="lblAppRootDomain" meta:resourcekey="lblAppRootDomain"/></td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtAppRootDomain" Width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAppRootDomain" ErrorMessage="*" />
        </td>
    </tr>
    
    
    
    
</table>