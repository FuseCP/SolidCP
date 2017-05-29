<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcPrivateNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS.VdcPrivateNetwork" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>

	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Network48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Private Network"></asp:Localize>
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="vdc_private_network" />
                <div class="panel panel-default tab-content">
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
				        AllowPaging="True" AllowSorting="True" DataKeyNames="PrivateAddressId" DataSourceID="odsPrivateAddressesPaged">
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
				    <asp:ObjectDataSource ID="odsPrivateAddressesPaged" runat="server" EnablePaging="True"
						    SelectCountMethod="GetPackagePrivateIPAddressesCount"
						    SelectMethod="GetPackagePrivateIPAddresses"
						    SortParameterName="sortColumn"
						    TypeName="SolidCP.Portal.VirtualMachinesHelper"
						    OnSelected="odsPrivateAddressesPaged_Selected">
					    <SelectParameters>
						    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
                            <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
                            <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
					    </SelectParameters>
				    </asp:ObjectDataSource>
				    <br />
    				
				    <scp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                    
                        <table cellspacing="6">
                            <tr>
                                <td><asp:Localize ID="locVpsAddressesQuota" runat="server" meta:resourcekey="locVpsAddressesQuota" Text="IP addresses per VPS:"></asp:Localize></td>
                                <td><scp:Quota ID="addressesPerVps" runat="server" QuotaName="VPS.PrivateIPAddressesNumber" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
			    </div>
                </div>
                </div>
                </div>