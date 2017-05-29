<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDS_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.RDS_Settings" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<fieldset>
    <legend>
        <asp:Label ID="secCertificateSettings" runat="server" meta:resourcekey="secServiceSettings" Text="SSL Certificate" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>    
    <table>  
        <tr>
            <td class="SubHead" style="width:200px" nowrap>
                <asp:Localize runat="server" meta:resourcekey="lblSelectFile" />
            </td>
            <td style="padding: 10px 0 10px 0;"><asp:FileUpload ID="upPFX" runat="server"/></td>            
        </tr>        
        <tr><td></td></tr>
        <tr>
            <td class="SubHead" style="width:200px" nowrap>
                <asp:Localize runat="server" meta:resourcekey="lblPFXInstallPassword" />
            </td>
            <td>                        
                <asp:TextBox ID="txtPFXInstallPassword" runat="server" TextMode="Password" Width="200px" />
            </td>
        </tr>
    </table> 
    </fieldset>
<br />
<fieldset>   
    <legend>
        <asp:Label ID="currentCertificate" runat="server" meta:resourcekey="currentCertificate" Text="Current Certificate" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
            <asp:Panel ID="plCertificateInfo" Visible="false" runat="server">
            <table>
                <tr>
                    <td class="SubHead" style="width:200px" nowrap>
                        <asp:Localize runat="server" meta:resourcekey="lblIssuedBy" />
                    </td>
                    <td class="SubHead">                        
                        <asp:Label ID="lblIssuedBy" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px" nowrap>
                        <asp:Localize runat="server" meta:resourcekey="lblSanName" />
                    </td>
                    <td class="SubHead">                        
                        <asp:Label ID="lblSanName" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px" nowrap>
                        <asp:Localize runat="server" meta:resourcekey="lblExpiryDate" />
                    </td>
                    <td class="SubHead">                        
                        <asp:Label ID="lblExpiryDate" runat="server"/>
                    </td>
                </tr>                
            </table>
            </asp:Panel>        
</fieldset>
<br />
<fieldset>
    <legend>
        <asp:Label ID="secOther" runat="server" meta:resourcekey="secOther" Text="Other Settings" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<table>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblCollectionsImport" meta:resourcekey="lblCollectionsImport" Text="Allow Collections Import:"/>
        </td>
        <td class="Normal">
            <asp:CheckBox runat="server" ID="cbCollectionsImport" meta:resourcekey="cbCollectionsImport" Checked="false" />
        </td>
    </tr> 
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblConnectionBroker" meta:resourcekey="lblConnectionBroker" Text="Connection Broker:"/>
        </td>
        <td>                        
            <asp:TextBox runat="server" ID="txtConnectionBroker" MaxLength="1000" Width="200px"  />            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtConnectionBroker" Display="Dynamic" ErrorMessage="*" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblRootOU" meta:resourcekey="lblRootOU" Text="Root OU:"/>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtRootOU" MaxLength="1000" Width="200px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRootOU" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblComputersRootOU" meta:resourcekey="lblComputersRootOU" Text="Computers Root OU:"/>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtComputersRootOu" MaxLength="1000" Width="200px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtComputersRootOu" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblPrimaryDomainController" meta:resourcekey="lblPrimaryDomainController" Text="Primary Domain Controller:"/>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtPrimaryDomainController" MaxLength="1000" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPrimaryDomainController" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblUseCentralNPS" meta:resourcekey="lblUseCentralNPS" Text="Use Central NPS:"/>
        </td>
        <td class="Normal">
            <asp:CheckBox runat="server" ID="chkUseCentralNPS" meta:resourcekey="chkUseCentralNPS" OnCheckedChanged="chkUseCentralNPS_CheckedChanged" AutoPostBack="True" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblCentralNPS" meta:resourcekey="lblCentralNPS" MaxLength="1000" Text="Central NPS:"/>
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtCentralNPS" Width="200px"/>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap valign="top">
            <asp:Localize ID="locGWServers" runat="server" meta:resourcekey="locGWServers"
                Text="Gateway Servers:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAddGWServer" MaxLength="1000" Width="200px"  />  
            <CPCC:StyleButton id="btnAddGWServer" CssClass="btn btn-success" runat="server" OnClick="btnAddGWServer_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
            <br />
            <asp:GridView ID="gvGWServers" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
                CssSelectorClass="NormalGridView" OnRowCommand="gvGWServers_RowCommand" meta:resourcekey="gvGWServers">
                <Columns>
                    <asp:TemplateField meta:resourcekey="ServerNameColumn" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblServerName" Text='<%#Eval("ServerName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='RemoveServer' CommandArgument='<%#Eval("ServerName") %>' OnClientClick="return confirm('Delete?');"> 
                                &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                            </CPCC:StyleButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>    
</fieldset>
<br />