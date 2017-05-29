<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcHome.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VdcHome" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="wsp" %>

	    <div class="Content">
		    <div class="Center">
			    
			    <div class="FormBody">
                    
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />

                    <div class="FormButtonsBar">
                        <div class="Left">
                            <asp:Button ID="btnCreate" runat="server" meta:resourcekey="btnCreate"
                                Text="Create VPS" CssClass="Button1" CausesValidation="False" 
                                onclick="btnCreate_Click" />
                            <asp:Button ID="btnImport" runat="server" meta:resourcekey="btnImport"
                                Text="Import VPS" CssClass="Button1" CausesValidation="False" 
                                onclick="btnImport_Click" />
                        </div>
                        <div class="Right">
                            <wsp:SearchBox ID="searchBox" runat="server" />
                        </div>
                        <div class="Right" style="margin-right: 20px">
                            <asp:Button ID="btnReplicaStates" runat="server" meta:resourcekey="btnReplicaStates"
                                Text="Show Replication Statuses" CssClass="Button1" CausesValidation="False"
                                OnClick="btnReplicaStates_Click" />
                            <asp:Label runat="server" Text="Page size:" CssClass="Normal"></asp:Label>
							<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"   
                                onselectedindexchanged="ddlPageSize_SelectedIndexChanged"> 
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem Selected="True">20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>
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
							        <asp:hyperlink id="lnk1" runat="server"
								        NavigateUrl='<%# GetServerEditUrl(Eval("ItemID").ToString()) %>'>
								        <%# Eval("ItemName") %>
							        </asp:hyperlink>
						        </ItemTemplate>
					        </asp:TemplateField>
					        <asp:BoundField HeaderText="gvServersExternalIP" meta:resourcekey="gvServersExternalIP"
					            DataField="ExternalIP" SortExpression="ExternalIP" />
					        <asp:BoundField HeaderText="gvServersPrivateIP" meta:resourcekey="gvServersPrivateIP"
					            DataField="IPAddress" SortExpression="IPAddress" />
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
								    <asp:ImageButton ID="cmdDelete" runat="server" Text="Delete" SkinID="VpsDelete2012"
									    CommandName="DeleteItem" CommandArgument='<%# Eval("ItemID") %>'
									    meta:resourcekey="cmdDelete"></asp:ImageButton>
							    </ItemTemplate>
						    </asp:TemplateField>
                            <asp:TemplateField>
			                    <ItemTemplate>
				                    <asp:LinkButton ID="cmdMove" runat="server" Text="Move"
					                    CommandName="Move" CommandArgument='<%# Eval("ItemID") %>'
					                    meta:resourcekey="cmdMove"></asp:LinkButton>
					                &nbsp;
				                    <asp:LinkButton ID="cmdDetach" runat="server" Text="Detach"
					                    CommandName="Detach" CommandArgument='<%# Eval("ItemID") %>'
					                    meta:resourcekey="cmdDetach" OnClientClick="return confirm('Remove this item?');"></asp:LinkButton>
			                    </ItemTemplate>
                            </asp:TemplateField>
				        </Columns>
			        </asp:GridView>
				    <asp:ObjectDataSource ID="odsServersPaged" runat="server" EnablePaging="True"
						    SelectCountMethod="GetVirtualMachinesCount"
						    SelectMethod="GetVirtualMachines"
						    SortParameterName="sortColumn"
						    TypeName="SolidCP.Portal.VirtualMachinesProxmoxHelper"
						    OnSelected="odsServersPaged_Selected">
					    <SelectParameters>
						    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
                            <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
                            <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
					    </SelectParameters>
				    </asp:ObjectDataSource>
				    <br />
    				

    				
			    </div>
		    </div>
	    </div>
