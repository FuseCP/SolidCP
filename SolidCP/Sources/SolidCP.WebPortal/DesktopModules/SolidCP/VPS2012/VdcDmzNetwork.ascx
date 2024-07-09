<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcDmzNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcDmzNetwork" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PackageVLANs.ascx" TagName="PackageVLANs" TagPrefix="scp" %>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">

                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanLeft">
                        </div>
                        <div class="FormButtonsBarCleanRight">
                            <scp:SearchBox ID="searchBox" runat="server" />
                        </div>
                    </div>

			        <asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="False" EnableViewState="false"
				        Width="100%" EmptyDataText="gvAddresses" CssSelectorClass="NormalGridView"
				        AllowPaging="True" AllowSorting="True" DataKeyNames="DmzAddressId" DataSourceID="odsDmzAddressesPaged">
				        <Columns>
					        <asp:BoundField HeaderText="gvAddressesIPAddress" meta:resourcekey="gvAddressesIPAddress"
					            DataField="IPAddress" SortExpression="IPAddress" />
					            
					        <asp:TemplateField HeaderText="gvAddressesItemName" meta:resourcekey="gvAddressesItemName" SortExpression="ItemName">						        						        
						        <ItemTemplate>						        
							         <asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# GetServerEditUrl(Eval("ItemID").ToString()) %>'>
								        <%# Eval("ItemName") %>
							        </asp:hyperlink>
						        </ItemTemplate>
					        </asp:TemplateField>

					        <asp:TemplateField HeaderText="gvAddressesPrimary" meta:resourcekey="gvAddressesPrimary" SortExpression="IsPrimary">						        						        
						        <ItemTemplate>						        
							            <asp:Image runat="server" ID="imgPrimary" ImageUrl='<%# PortalUtils.GetThemedImage("Exchange/checkbox.png")%>'
							                Visible='<%# Eval("IsPrimary") %>' ImageAlign="AbsMiddle"  />&nbsp;
						        </ItemTemplate>
					        </asp:TemplateField>
				        </Columns>
			        </asp:GridView>
				    <asp:ObjectDataSource ID="odsDmzAddressesPaged" runat="server" EnablePaging="True"
						    SelectCountMethod="GetPackageDmzIPAddressesCount"
						    SelectMethod="GetPackageDmzIPAddresses"
						    SortParameterName="sortColumn"
						    TypeName="SolidCP.Portal.VirtualMachines2012Helper"
						    OnSelected="odsDmzAddressesPaged_Selected">
					    <SelectParameters>
						    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
                            <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
                            <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
					    </SelectParameters>
				    </asp:ObjectDataSource>
				    <br />

                    <scp:CollapsiblePanel id="secVLAN" runat="server"
                        TargetControlID="VLANPanel" meta:resourcekey="secVLAN" Text="VLAN" IsCollapsed="false">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="VLANPanel" runat="server" Height="0" style="overflow:hidden;">
                        <scp:PackageVLANs id="packageVLANs" runat="server"
                            SpaceHomeControl="vdc_dmz_network"
                            AllocateVLANsControl="vdc_allocate_dmz_vlan"  />
                    </asp:Panel>
    				
				    <scp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                    
                        <table cellspacing="6">
                            <tr>
                                <td><asp:Localize ID="locVpsAddressesQuota" runat="server" meta:resourcekey="locVpsAddressesQuota" Text="IP addresses per VPS:"></asp:Localize></td>
                                <td><scp:Quota ID="addressesPerVps" runat="server" QuotaName="VPS2012.DMZIPAddressesNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locVLANsQuota" runat="server" meta:resourcekey="locVLANsQuota" Text="DMZ Network VLANs:"></asp:Localize></td>
                                <td><scp:Quota ID="vlansQuota" runat="server" QuotaName="VPS2012.DMZVLANsNumber" /></td>
                            </tr>
                        </table>
                    
                    
                    </asp:Panel>


			    </div>
		    </div>
	    </div>
