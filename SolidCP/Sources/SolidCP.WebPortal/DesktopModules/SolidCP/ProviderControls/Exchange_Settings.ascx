<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Exchange_Settings.ascx.cs"
    Inherits="SolidCP.Portal.ProviderControls.Exchange2010_Settings" %>
<%@ Register Src="../SkinControls/BootstrapDropDownList.ascx" TagName="BootstrapDropDownList" TagPrefix="scp" %>

<%@ Import Namespace="SolidCP.Portal" %>

<table cellpadding="3" cellspacing="0" width="100%">
    
    <tr runat="server" id="powershellUrl1" width="200" nowrap>
        <td class="SubHead">
        </td>
        <td>
            <asp:Label runat="server" ID="lblFileServiceInfo" meta:resourcekey="lblPowerShellUrl" Text="e.g. https://server1.domain.local/PowerShell" Font-Italic="true"></asp:Label>
        </td>
    </tr>

    <tr runat="server" id="powershellUrl2" width="200" nowrap>
        <td class="SubHead">
            <asp:Localize ID="loclocPowerShellUrl" runat="server" meta:resourcekey="locPowerShellUrl"
                Text="Powershell URL:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtPowerShellUrl" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
        </td>
    </tr>
    
    <tr runat="server" id="storageGroup">
			<td class="SubHead" width="200" nowrap>
			    <asp:Localize ID="locStorageGroup" runat="server" meta:resourcekey="locStorageGroup" Text="Storage Group Name:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox ID="txtStorageGroup" CssClass="form-control" runat="server" Width="300px"></asp:TextBox>	
            </td>
		</tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Localize ID="locMailboxDatabase" runat="server" meta:resourcekey="locMailboxDatabase"
                Text="Mailbox Database Name:"></asp:Localize>
            <asp:Localize ID="locMailboxDAG" runat="server" meta:resourcekey="locMailboxDAG"
                Text="Database Availability Group:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtMailboxDatabase" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td class="SubHead" runat="server" id="archivingGroup" width="200" nowrap>
            <asp:Localize ID="locArchivingDatabase" runat="server" meta:resourcekey="locArchivingDatabase"
                Text="Archiving Database Name:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtArchivingDatabase" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
        </td>
    </tr>


    <tr>
        <td class="SubHead">
            <asp:Localize ID="locKeepDeletedItems" runat="server" meta:resourcekey="locKeepDeletedItems"
                Text="Keep Deleted Items (days):"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtKeepDeletedItems" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Localize ID="locKeepDeletedMailboxes" runat="server" meta:resourcekey="locKeepDeletedMailboxes"
                Text="Keep Deleted Mailboxes (days):"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtKeepDeletedMailboxes" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
        </td>
    </tr>
</table>
<br />
<table cellpadding="4" cellspacing="0" width="100%">
    <tr runat="server" id="clusteredMailboxServer">
        <td class="SubHead">
            <asp:Localize ID="locMailboxClusterName" runat="server" meta:resourcekey="locMailboxClusterName"
                Text="Clustered Mailbox Server:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtMailboxClusterName" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
        <td class="SubHead">
            <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locPublicFolderServer"    ></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtPublicFolderServer" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
        <td class="SubHead">
            <asp:Localize ID="Localize2" runat="server" meta:resourcekey="locOABServer"    ></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtOABServer" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap valign="top" style="padding-top:15px;">
            <asp:Localize ID="locHubTransport" runat="server" meta:resourcekey="locHubTransport"
                Text="Hub Transport Service:"></asp:Localize>
        </td>
        <td style="padding-top:15px;">
            <div class="input-group col-sm-6">
                <asp:DropDownList ID="ddlHubTransport" runat="server" CssClass="form-control">
                </asp:DropDownList>
            <span class="input-group-btn">
                <CPCC:StyleButton id="btnAdd" CssClass="btn btn-primary" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton><br />
            </span>
            </div>
             <asp:GridView ID="gvHubTransport" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
                CssSelectorClass="NormalGridView" OnRowCommand="gvHubTransport_RowCommand" meta:resourcekey="gvHubTransport">
                <Columns>
                    <asp:TemplateField meta:resourcekey="locServerNameColumn" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblServiceName" Text='<%#Eval("ServiceName") + "(" + Eval("ServerName") +")"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                                        
                    <asp:TemplateField>
                        <ItemTemplate>
                            <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='RemoveServer' CommandArgument='<%#Eval("ServiceId") %>' OnClientClick="return confirm('Delete?');"> 
                                &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                            </CPCC:StyleButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap valign="top" style="padding-top:15px;">
            <asp:Localize ID="locClientAccess" runat="server" meta:resourcekey="locClientAccess"
                Text="Client Access Service:"></asp:Localize>
        </td>
        <td style="padding-top:15px;">
            <div class="input-group col-sm-6">
                <asp:DropDownList ID="ddlClientAccess" runat="server" CssClass="form-control">
                </asp:DropDownList>
                <span class="input-group-btn">
                    <CPCC:StyleButton id="Button1" CssClass="btn btn-primary" runat="server" OnClick="btnAddClientAccess_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton><br />
                </span>
            </div>
            <asp:GridView ID="gvClients" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
                CssSelectorClass="NormalGridView" OnRowCommand="gvClientAccess_RowCommand" meta:resourcekey="gvClientAccess">
                <Columns>
                    <asp:TemplateField meta:resourcekey="locServerNameColumn" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblServiceName" Text='<%#Eval("ServiceName") + "(" + Eval("ServerName") +")"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='RemoveServer' CommandArgument='<%#Eval("ServiceId") %>' OnClientClick="return confirm('Delete?');"> 
                                &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                            </CPCC:StyleButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<br />
<fieldset>
    <legend>
        <asp:Label ID="lblSetupVariables" runat="server" meta:resourcekey="lblSetupVariables"
            Text="Setup Instruction Variables" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" valign="top" style="width: 200px;">
                <asp:Localize ID="locSmtpServers" runat="server" meta:resourcekey="locSmtpServers"
                    Text="SMTP Servers:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtSmtpServers" runat="server" Width="300px" CssClass="form-control"
                    TextMode="MultiLine" Rows="3"></asp:TextBox>
                <br />
                <asp:Localize ID="locSmtpComments" runat="server" meta:resourcekey="locSmtpComments"
                    Text=" * one SMTP server record per line"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locAutodiscoverIP" runat="server" meta:resourcekey="locAutodiscoverIP"
                    Text="Autodiscover Server IP:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtAutodiscoverIP" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locAutodiscoverDomain" runat="server" meta:resourcekey="locAutodiscoverDomain"
                    Text="Autodiscover Server Domain:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtAutodiscoverDomain" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locOwaUrl" runat="server" meta:resourcekey="locOwaUrl" Text="OWA URL:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtOwaUrl" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locActiveSyncServer" runat="server" meta:resourcekey="locActiveSyncServer"
                    Text="ActiveSync Server:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtActiveSyncServer" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>


<div id="SeDiv" runat="server">
<fieldset>
    <legend>
        <asp:Label ID="lblRouteFromSE" runat="server" meta:resourcekey="lblRouteFromSE"
            Text="Route from SE to:" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
    <div class="Content">
        <p>Please note this will be moving to its own provider in future versions. We will provide a clear notice in the release notes.</p>
    </div>
    <asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView id="gvSEDestinations" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
	        Width="100%" EmptyDataText="" CssSelectorClass="NormalGridView" OnRowCommand="gvSEDestinations_RowCommand" >
	        <Columns>
		        <asp:TemplateField HeaderText="Destinations">
			        <ItemStyle Width="100%"></ItemStyle>
			        <ItemTemplate>
				        <asp:Label id="lblSEDestination" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Container.DataItem)%></asp:Label>
                    </ItemTemplate>
		        </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Width="65px" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <CPCC:StyleButton id="imgDelRouteFromSE" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# (string)Container.DataItem %>' OnClientClick="return confirm('Are you sure you want to delete selected route?')"> 
                            &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                        </CPCC:StyleButton>
                    </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
	        </asp:GridView>
            <br />
            <div class="input-group col-sm-6">
             <asp:TextBox ID="tbSEDestinations" CssClass="form-control" style="vertical-align: middle;" runat="server"></asp:TextBox>
                <span class="input-group-btn">
                <CPCC:StyleButton ID="bntAddSEDestination" runat="server" CssClass="btn btn-primary" OnClick="bntAddSEDestination_Click" CausesValidation="False">
                <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="bntAddSEDestination" />
                </CPCC:StyleButton>
                </span>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
</fieldset>
</div>


