<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Organizations_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Organizations_Settings" %>
<table width="100%"  cellspacing="0">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblRootOU" meta:resourcekey="lblRootOU" />
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtRootOU" MaxLength="1000" Width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRootOU" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" nowrap="true"><asp:Label runat="server" ID="lblPrimaryDomainController" meta:resourcekey="lblPrimaryDomainController" /></td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtPrimaryDomainController" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPrimaryDomainController" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap="true"><asp:Label runat="server" ID="Label1" meta:resourcekey="lblTemporyDomainName" /></td>
        <td><asp:TextBox  runat="server" ID="txtTemporyDomainName" MaxLength="100" Width="200px" />
        <asp:RequiredFieldValidator  ControlToValidate="txtTemporyDomainName" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap="nowrap"><asp:Label runat="server" ID="UserNameFormatLabel" meta:resourcekey="lblUserNameFormat"/></td>
        <td>
            <asp:DropDownList runat="server" ID="UserNameFormatDropDown">
                <asp:ListItem Value="1" meta:resourcekey="listItemStandard"/>
                <asp:ListItem Value="2" meta:resourcekey="listItemAppendOrgId"/>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap="true"><asp:Label runat="server" ID="Label2" meta:resourcekey="lblArchiveStorageSpace" /></td>
        <td>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                     <asp:TextBox  runat="server" ID="txtArchiveStorageSpace" MaxLength="100" Width="200px" />
                    <asp:CheckBox ID="chkUseStorageSpaces" runat="server" meta:resourcekey="chkUseStorageSpaces" Text="Use Storage Spaces" OnCheckedChanged="chkUseStorageSpaces_StateChanged" AutoPostBack="True"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>