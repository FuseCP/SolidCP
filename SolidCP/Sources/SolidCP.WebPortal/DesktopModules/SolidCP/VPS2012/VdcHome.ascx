﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcHome.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcHome" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>

<div class="FormButtonsBar right">
			                <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" CausesValidation="False"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>&nbsp;
                            <CPCC:StyleButton id="btnImport" CssClass="btn btn-warning" runat="server" OnClick="btnImport_Click" CausesValidation="False"> <i class="fa fa-download">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnImportText"/> </CPCC:StyleButton>
                  </div>
			    <div class="panel-body form-horizontal">
                    
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <div class="row">
                        <div class="col-md-4 col-md-offset-4">
                            <CPCC:StyleButton id="btnReplicaStates" CssClass="btn btn-success" runat="server" OnClick="btnReplicaStates_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnReplicaStatesText"/> </CPCC:StyleButton>
                            <asp:Label runat="server" Text="Page size:" CssClass="Normal"></asp:Label>
							<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"   
                                onselectedindexchanged="ddlPageSize_SelectedIndexChanged"> 
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem Selected="True">20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <scp:SearchBox ID="searchBox" runat="server" />
                        </div>
                    </div>
                    </div>
			        <asp:GridView ID="gvServers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
				        Width="100%" EmptyDataText="gvServers" CssSelectorClass="NormalGridView"
				        AllowPaging="True" AllowSorting="True" DataSourceID="odsServersPaged" PageSize="20"
                        onrowcommand="gvServers_RowCommand">
				        <Columns>
					        <asp:TemplateField HeaderText="gvServersName" SortExpression="ItemName" meta:resourcekey="gvServersName">
						        <ItemStyle></ItemStyle>
						        <ItemTemplate>
						            <asp:Image runat="server" SkinID="Vps2012_16" />
							        <asp:hyperlink id="lnk1" runat="server" style='<%# IsServerDeleting(Eval("ItemID").ToString()) ? "text-decoration: line-through !important;" : "" %>'
								        NavigateUrl='<%# IsServerDeleting(Eval("ItemID").ToString()) ? "" : GetServerEditUrl(Eval("ItemID").ToString()) %>'>
								        <%# Eval("ItemName") %>
							        </asp:hyperlink>
						        </ItemTemplate>
					        </asp:TemplateField>
					        <asp:BoundField HeaderText="gvServersExternalIP" meta:resourcekey="gvServersExternalIP"
					            DataField="ExternalIP" SortExpression="ExternalIP" />
					        <asp:BoundField HeaderText="gvServersPrivateIP" meta:resourcekey="gvServersPrivateIP"
					            DataField="IPAddress" SortExpression="PIP.IPAddress" />
							<asp:BoundField HeaderText="gvServersDmzIP" meta:resourcekey="gvServersDmzIP"
					            DataField="DmzIP" SortExpression="DIP.IPAddress" />
					        <asp:TemplateField HeaderText="gvServersSpace" meta:resourcekey="gvServersSpace" SortExpression="PackageName" >
						        <ItemTemplate>
							        <asp:hyperlink id="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl(Eval("PackageID").ToString()) %>'>
								        <%# Eval("PackageName") %>
							        </asp:hyperlink>
						        </ItemTemplate>
					        </asp:TemplateField>
					        <asp:TemplateField HeaderText="gvServersUser" meta:resourcekey="gvServersUser" SortExpression="Username"  >						        
						        <ItemTemplate>
							        <asp:hyperlink id="lnkUser" runat="server" NavigateUrl='<%# GetUserHomeUrl((int)Eval("UserID")) %>'>
                                        <%# Eval("UserName") %>
							        </asp:hyperlink>
						        </ItemTemplate>
					        </asp:TemplateField>
					        <asp:TemplateField HeaderText="Replication" meta:resourcekey="gvReplication" >						        
						        <ItemTemplate>
							        <asp:Localize id="locReplication" runat="server" Text='<%# GetReplicationStatus((int)Eval("ItemID")) %>'></asp:Localize>
						        </ItemTemplate>
					        </asp:TemplateField>
						    <asp:TemplateField>
							    <ItemTemplate>
								    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("ItemID") %>' Enabled='<%# !IsServerDeleting(Eval("ItemID").ToString())%>'> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <CPCC:StyleButton id="cmdReinstall" CssClass="btn btn-success" runat="server" CommandName="ReinstallItem" CommandArgument='<%# Eval("ItemID") %>' Enabled='<%# !IsServerDeleting(Eval("ItemID").ToString())%>'> &nbsp;<i class="fa fa-refresh"></i>&nbsp; </CPCC:StyleButton>							    
                                </ItemTemplate>
						    </asp:TemplateField>
                            <asp:TemplateField>
			                    <ItemTemplate>
				                    <CPCC:StyleButton id="cmdMove" CssClass="btn btn-warning" runat="server" CommandName="Move" CommandArgument='<%# Eval("ItemID") %>' Enabled='<%# !IsServerDeleting(Eval("ItemID").ToString())%>'> <i class="fa fa-clone">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="cmdMoveText"/> </CPCC:StyleButton>
					                &nbsp;
				                    <CPCC:StyleButton ID="cmdDetach" runat="server" 
 					                    CommandName="Detach" CommandArgument='<%# Eval("ItemID") %>'
					                    CssClass="btn btn-default btn-sm" OnClientClick="return confirm('Remove this item?');">
                                        <i class="fa fa-chain-broken">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="cmdDetachText"/>
                                    </CPCC:StyleButton>
			                    </ItemTemplate>
                            </asp:TemplateField>
				        </Columns>
			        </asp:GridView>
				    <asp:ObjectDataSource ID="odsServersPaged" runat="server" EnablePaging="True"
						    SelectCountMethod="GetVirtualMachinesCount"
						    SelectMethod="GetVirtualMachines"
						    SortParameterName="sortColumn"
						    TypeName="SolidCP.Portal.VirtualMachines2012Helper"
						    OnSelected="odsServersPaged_Selected">
					    <SelectParameters>
						    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
                            <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
                            <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
					    </SelectParameters>
				    </asp:ObjectDataSource>
	
    	
                    <asp:Panel ID="QuotasPanel" runat="server" class="panel-footer">
                    
                        <table cellspacing="6">
                            <tr>
                                <td><asp:Localize ID="locVpsQuota" runat="server" meta:resourcekey="locVpsQuota" Text="Number of VPS:"></asp:Localize></td>
                                <td><scp:Quota ID="vpsQuota" runat="server" QuotaName="VPS2012.ServersNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locCPUQuota" runat="server" meta:resourcekey="locCPUQuota" Text="CPU's:"></asp:Localize></td>
                                <td><scp:Quota ID="cpuQuota" runat="server" QuotaName="VPS2012.CpuNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locRamQuota" runat="server" meta:resourcekey="locRamQuota" Text="RAM, MB:"></asp:Localize></td>
                                <td><scp:Quota ID="ramQuota" runat="server" QuotaName="VPS2012.Ram" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locHddQuota" runat="server" meta:resourcekey="locHddQuota" Text="HDD, GB:"></asp:Localize></td>
                                <td><scp:Quota ID="hddQuota" runat="server" QuotaName="VPS2012.Hdd" /></td>
                            </tr>
                        </table>
                    </asp:Panel>