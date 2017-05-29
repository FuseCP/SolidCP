<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSServers.ascx.cs" Inherits="SolidCP.Portal.RDSServers" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<script type="text/javascript">
    //<![CDATA[
    $(document).ready(function () {        
        setTimeout(checkStatus, 3000);        
    });    
    //]]>
</script>
<asp:UpdatePanel runat="server" ID="messageBoxPanel" UpdateMode="Conditional"><ContentTemplate><scp:SimpleMessageBox id="messageBox" runat="server" /></ContentTemplate></asp:UpdatePanel>
<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnAddRDSServer"  runat="server" CssClass="btn btn-primary" OnClick="btnAddRDSServer_Click" >
        <i class="fa fa-plus">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddRDSServer" />
    </CPCC:StyleButton>
</div>
<div class="panel-body">
    <div class="row">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
            <div class="col-sm-12 text-right form-inline">
                <div class="form-group">
                    <div class="input-group">
                        <asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
                        <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlPageSize_SelectedIndexChanged">
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem Selected="True">20</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control" />
                        <div class="input-group-btn">
                            <CPCC:StyleButton ID="cmdSearch" runat="server" SkinID="SearchButton" CausesValidation="false" style="vertical-align: middle;" CssClass="btn btn-primary">
                                <i class="fa fa-search" aria-hidden="true"></i>
                            </CPCC:StyleButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>
<asp:ObjectDataSource ID="odsRDSServersPaged" runat="server" EnablePaging="True" SelectCountMethod="GetRDSServersPagedCount"
            SelectMethod="GetRDSServersPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.RDSHelper" OnSelected="odsRDSServersPaged_Selected">
            <SelectParameters>
                <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />                                
            </SelectParameters>
        </asp:ObjectDataSource>

<asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional">
    <ContentTemplate>                 
        <asp:HiddenField runat="server" ID="hdnGridState" Value="false" />
        <asp:HiddenField runat="server" ID="hdnItemId" Value="false" />
        <asp:GridView id="gvRDSServers" runat="server" AutoGenerateColumns="False"
	        AllowPaging="True" AllowSorting="True"
	        CssSelectorClass="NormalGridView"
            OnRowCommand="gvRDSServers_RowCommand"
	        DataSourceID="odsRDSServersPaged" EnableViewState="False"
	        EmptyDataText="gvRDSServers">
	        <Columns>
                <asp:TemplateField>
                    <ItemStyle Width="0%" />
                    <ItemTemplate>                        
                        <asp:HiddenField ID="hdnRdsServerFqdnName" runat="server" Value='<%# Eval("FqdName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
		        <asp:TemplateField SortExpression="Name" HeaderText="Server name">
		            <HeaderStyle Wrap="false" />
                    <ItemStyle Wrap="False" Width="15%"/>
                    <ItemTemplate>
                        <asp:LinkButton OnClientClick="ShowProgressDialog('Loading ...');return true;" CommandName="EditServer" CommandArgument='<%# Eval("Id")%>' ID="lbEditServer" runat="server" Text='<%#Eval("Name") %>'/>                    
                    </ItemTemplate>                    
                </asp:TemplateField>
		        <asp:BoundField DataField="Address" HeaderText="IP Address"><ItemStyle  Width="10%"/></asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="Organization"><ItemStyle  Width="20%"/></asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Comments"><ItemStyle  Width="15%"/></asp:BoundField>
                <asp:TemplateField meta:resourcekey="gvPopupStatus">
                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Literal ID="litStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Literal>
                        <asp:HiddenField ID="hdnRdsCollectionId" runat="server" Value='<%# Eval("RdsCollectionId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="gvViewInfo">
                    <ItemStyle Width="8%" HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <CPCC:StyleButton OnClientClick="ShowProgressDialog('Getting Server Info ...');return true;" style="display:none"
                            CommandName="ViewInfo" CommandArgument='<%# Eval("Id")%>' ID="lbViewInfo" runat="server" CssClass="btn btn-primary" Text="View Info"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="gvRestart">
                    <ItemStyle HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <CPCC:StyleButton ID="lbRestart" CommandName="Restart" CommandArgument='<%# Eval("Id")%>' runat="server" Text="Restart" CssClass="btn btn-warning" style="display:none"
                            OnClientClick="if(confirm('Are you sure you want to restart selected server?')) ShowProgressDialog('Loading...'); else return false;"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField meta:resourcekey="gvShutdown">
                    <ItemStyle Width="9%" HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <CPCC:StyleButton ID="lbShutdown" CommandName="ShutDown" CssClass="btn btn-danger" CommandArgument='<%# Eval("Id")%>' style="display:none"
                            runat="server" Text="Shut Down" OnClientClick="if(confirm('Are you sure you want to shut down selected server?')) ShowProgressDialog('Loading...'); else return false;"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
			        <ItemTemplate>
				        <CPCC:StyleButton ID="lnkInstallCertificate" runat="server" CssClass="btn btn-success" Text="Certificate" style="display:none"
					        CommandName="InstallCertificate" CommandArgument='<%# Eval("Id") %>' ToolTip="Repair Certificate"
                            OnClientClick="if(confirm('Are you sure you want to install certificate?')) ShowProgressDialog('Installing certificate...'); else return false;"></CPCC:StyleButton>                        
			        </ItemTemplate>
		        </asp:TemplateField>
                <asp:TemplateField>
			        <ItemTemplate>
				        <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" Visible='<%# Eval("ItemId") == null %>'
					        CommandName="DeleteItem" CssClass="btn btn-danger" CommandArgument='<%# Eval("Id") %>' 
                            meta:resourcekey="cmdDelete" OnClientClick="if(confirm('Are you sure you want to delete selected rds server??')) ShowProgressDialog('Removeing RDS Server...'); else return false;">&nbsp; <i class="fa fa-trash-o"></i> &nbsp;</asp:LinkButton>
			        </ItemTemplate>
		        </asp:TemplateField>
	        </Columns>
        </asp:GridView>        

        <asp:Panel ID="ServerInfoPanel" runat="server" style="display:none">
            <div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-server"></i> <asp:Localize ID="Localize1" runat="server" meta:resourcekey="headerServerInfo"/></h3>
                     </div>
                    <div class="widget-content Popup">
                    <asp:UpdatePanel ID="serverInfoUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="Popup-Scroll" style="height:auto;">
                                <scp:CollapsiblePanel id="secServerInfo" runat="server" TargetControlID="panelHardwareInfo" meta:resourcekey="secRdsApplicationEdit" Text=""/>                            
                                <asp:Panel runat="server" ID="panelHardwareInfo">
                                    <table>
                                        <tr>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="locProcessor" runat="server" Text="Processor:"/>
                                            </td>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="litProcessor" runat="server"/>
                                            </td>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="locLoadPercentage" Text="Load Percentage:" runat="server"/>
                                            </td>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="litLoadPercentage" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="locMemoryAllocated" runat="server" Text="Allocated Memory:"/>
                                            </td>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="litMemoryAllocated" runat="server"/>
                                            </td>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="locFreeMemory" Text="Free Memory:" runat="server"/>
                                            </td>
                                            <td class="FormLabel150" style="width: 150px;">
                                                <asp:Literal ID="litFreeMemory" runat="server"/>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <scp:CollapsiblePanel id="secRdsApplicationEdit" runat="server" TargetControlID="panelDiskDrives" meta:resourcekey="secRdsApplicationEdit" Text="Disk Drives"/>
                                <asp:Panel runat="server" ID="panelDiskDrives">
                                    <table>
                                        <asp:Repeater ID="rpServerDrives" runat="server" EnableViewState="false">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="litDeviceId" runat="server" Text='<%# Eval("DeviceId") %>'/>
                                                    </td>
                                                    <td class="FormLabel150" style="width: 150px;"/>                                                                                                                                    
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="locVolumeName" Text="Volume Name:" runat="server"/>
                                                    </td>
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="litVolumeName" Text='<%# Eval("VolumeName") %>' runat="server"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="locSize" Text="Size:" runat="server"/>
                                                    </td>
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="litSize" Text='<%# Eval("SizeMb") + " MB" %>' runat="server"/>
                                                    </td>                                                                                                                                    
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="locFreeSpace" Text="Free Space:" runat="server"/>
                                                    </td>
                                                    <td class="FormLabel150" style="width: 150px;">
                                                        <asp:Literal ID="litFreeSpace" Text='<%# Eval("FreeSpaceMb") + " MB" %>' runat="server"/>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
					<div class="popup-buttons text-right">            
                    <CPCC:StyleButton id="btnCancelServerInfo" CssClass="btn btn-warning" runat="server" CausesValidation="false"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnServerInfoCancelText"/> </CPCC:StyleButton>
                </div>
            </div>
        </asp:Panel>

        <asp:Button ID="btnViewInfoFake" runat="server" style="display:none;" />
        <ajaxToolkit:ModalPopupExtender ID="ViewInfoModal" runat="server" TargetControlID="btnViewInfoFake" PopupControlID="ServerInfoPanel" 
            BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelServerInfo"/>
    </ContentTemplate>    
</asp:UpdatePanel>
<div class="panel-footer text-right">
    	<CPCC:StyleButton ID="StyleButton1"  runat="server" CssClass="btn btn-primary" OnClick="btnAddRDSServer_Click" ><i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddRDSServer" /></CPCC:StyleButton>
</div>