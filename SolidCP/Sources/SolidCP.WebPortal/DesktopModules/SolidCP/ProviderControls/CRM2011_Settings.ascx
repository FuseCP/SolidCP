<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRM2011_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.CRM2011_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="scp" %>
<table>
    <tr>
        <td class="SubHead" width="200" nowrap>Sql Server</td>
        <td>                        
            <asp:TextBox runat="server" ID="txtSqlServer" MaxLength="256" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSqlServer" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Reporting URL </td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" Width="200px" ID="txtReportingService" MaxLength="256" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReportingService" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Web Application Server Domain</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" Width="200px" ID="txtDomainName" MaxLength="256" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDomainName" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Web Application Domain Scheme</td>
        <td class="Normal" width="100%">
          <asp:DropDownList runat="server" ID="ddlSchema">
            <asp:ListItem Text="http" Value="http" />
            <asp:ListItem Text="https" Value="https" />
          </asp:DropDownList>
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>CRM Website IP</td>
        <td class="Normal" width="100%">
            <scp:SelectIPAddress ID="ddlCrmIpAddress" runat="server" ServerIdParam="ServerID" AllowEmptySelection="false" />            
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>CRM Website Port</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtPort" Width="200px" />
            <asp:RangeValidator runat="server" ControlToValidate="txtPort" Display="dynamic" ErrorMessage="*" Type="String" MinimumValue="0" MaximumValue="9" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Web Application Server</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtAppRootDomain" Width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAppRootDomain" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Organization Web Service</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtOrganizationWebService" Width="200px" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Discovery Web Service</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtDiscoveryWebService" Width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDiscoveryWebService" ErrorMessage="*" />
        </td>
    </tr>

    <tr>
        <td class="SubHead" width="200" nowrap>Deployment Web Service</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtDeploymentWebService" Width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDeploymentWebService" ErrorMessage="*" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" width="200" nowrap>Service account</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtUserName" Width="200px" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>Password</td>
        <td class="Normal" width="100%">
            <asp:TextBox runat="server" ID="txtPassword" Width="200px" TextMode="Password" />
        </td>
    </tr>

	<tr>
	    <td class="SubHead" width="200" nowrap>Default Currency</td>
	    <td><asp:DropDownList runat="server" ID="ddlCurrency"/></td>
	</tr>
				          				          				        
    <tr>
	    <td class="SubHead" width="200" nowrap/>Default Collation</td>
	    <td><asp:DropDownList runat="server" ID="ddlCollation" /></td>
	</tr>                         

	<tr>
	    <td class="SubHead" width="200" nowrap>Default Base Language</td>
	    <td><asp:DropDownList runat="server" ID="ddlBaseLanguage" /></td>
	</tr>


</table>