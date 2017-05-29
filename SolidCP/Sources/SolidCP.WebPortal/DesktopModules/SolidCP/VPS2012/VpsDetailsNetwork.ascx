<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsNetwork" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp"  %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<script type="text/javascript">
    function SelectAllCheckboxes(box)
    {
		var state = box.checked;
        var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName("INPUT");
        for(i = 0; i < elm.length; i++)
            if(elm[i].type == "checkbox" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
		        elm[i].checked = state;
    }
</script>


	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_network" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    
				    <scp:CollapsiblePanel id="secExternalNetwork" runat="server"
                        TargetControlID="ExternalNetworkPanel" meta:resourcekey="secExternalNetwork" Text="External Network">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="ExternalNetworkPanel" runat="server" Height="0" style="overflow:hidden;">
                    
                        <table cellspacing="3">
                            <tr>
                                <td><asp:Localize ID="locExtAddress" runat="server"
                                    meta:resourcekey="locExtAddress" Text="Server address:"/></td>
                                <td class="NormalBold"><asp:Literal ID="litExtAddress" runat="server" Text=""/></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locExtSubnet" runat="server"
                                    meta:resourcekey="locExtSubnet" Text="Subnet mask:"/></td>
                                <td class="NormalBold"><asp:Literal ID="litExtSubnet" runat="server" Text=""/></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locExtGateway" runat="server"
                                    meta:resourcekey="locExtGateway" Text="Default gateway:"/></td>
                                <td class="NormalBold"><asp:Literal ID="litExtGateway" runat="server" Text=""/></td>
                            </tr>
                        </table>
                    
                        <div style="width:400px;">
				            <asp:GridView ID="gvExternalAddresses" runat="server" AutoGenerateColumns="False"
					                EmptyDataText="gvExternalAddresses" CssSelectorClass="NormalGridView"
					                DataKeyNames="AddressID">
					            <Columns>
					                <asp:TemplateField>
					                    <HeaderTemplate>
					                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
					                    </HeaderTemplate>
					                    <ItemTemplate>
					                        <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# !(bool)Eval("IsPrimary") %>' />
					                    </ItemTemplate>
                                        <ItemStyle Width="10px" />
					                </asp:TemplateField>
						            <asp:BoundField DataField="IPAddress" HeaderText="gvIpAddress" meta:resourcekey="gvIpAddress" />
						            <asp:BoundField DataField="SubnetMask" HeaderText="gvSubnetMask" meta:resourcekey="gvSubnetMask" />
						            <asp:BoundField DataField="DefaultGateway" HeaderText="gvDefaultGateway" meta:resourcekey="gvDefaultGateway" />
						            <asp:TemplateField HeaderText="gvPrimary" meta:resourcekey="gvPrimary" ItemStyle-Width="50">
							            <ItemTemplate>
							                <div style="text-align:center">
							                    &nbsp;
								                <asp:Image ID="Image2" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />
								            </div>
							            </ItemTemplate>
						            </asp:TemplateField>
					            </Columns>
				            </asp:GridView>
				        </div>
				        
				        <div style="margin-top: 4px;">
				            <asp:Button ID="btnAddExternalAddress" runat="server" meta:resourcekey="btnAddExternalAddress"
                                Text="Add" CssClass="SmallButton" onclick="btnAddExternalAddress_Click" />
				            <asp:Button id="btnSetPrimaryExternal" runat="server" Text="Set As Primary"
				                meta:resourcekey="btnSetPrimaryExternal" CssClass="SmallButton" 
                                CausesValidation="false" onclick="btnSetPrimaryExternal_Click"></asp:Button>
				            <asp:Button id="btnDeleteExternal" runat="server" Text="Delete Selected"
				                meta:resourcekey="btnDeleteExternal" CssClass="SmallButton" CausesValidation="false" 
                                onclick="btnDeleteExternal_Click"></asp:Button>
                        </div>

				        <br />
			            <asp:Localize ID="locTotalExternal" runat="server"
			                meta:resourcekey="locTotalExternal" Text="IP addresses:"></asp:Localize>
			            <asp:Label ID="lblTotalExternal" runat="server" CssClass="NormalBold">0</asp:Label>
                        <br />
                        <br />
                        <br />
                    </asp:Panel>
				    
				    
				    <scp:CollapsiblePanel id="secPrivateNetwork" runat="server"
                        TargetControlID="PrivateNetworkPanel" meta:resourcekey="secPrivateNetwork" Text="Private Network">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="PrivateNetworkPanel" runat="server" Height="0" style="overflow:hidden;">
                    
                        <table cellspacing="3">
                            <tr>
                                <td><asp:Localize ID="locPrivAddress" runat="server"
                                    meta:resourcekey="locPrivAddress" Text="Server address:"/></td>
                                <td class="NormalBold"><asp:Literal ID="litPrivAddress" runat="server" Text=""/></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locPrivFormat" runat="server"
                                    meta:resourcekey="locPrivFormat" Text="Network format:"/></td>
                                <td class="NormalBold"><asp:Literal ID="litPrivFormat" runat="server" Text=""/></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locPrivSubnet" runat="server"
                                    meta:resourcekey="locPrivSubnet" Text="Subnet mask:"/></td>
                                <td class="NormalBold"><asp:Literal ID="litPrivSubnet" runat="server" Text=""/></td>
                            </tr>
                        </table>
                    
                        <asp:Panel ID="PrivateAddressesPanel" runat="server">
                        
                            <div style="width:260px;">
				                <asp:GridView ID="gvPrivateAddresses" runat="server" AutoGenerateColumns="False"
					                    EmptyDataText="gvPrivateAddresses" CssSelectorClass="NormalGridView"
					                    DataKeyNames="AddressID">
					                <Columns>
					                    <asp:TemplateField>
					                        <HeaderTemplate>
					                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
					                        </HeaderTemplate>
					                        <ItemTemplate>
					                            <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# !(bool)Eval("IsPrimary") %>' />
					                        </ItemTemplate>
                                            <ItemStyle Width="10px" />
					                    </asp:TemplateField>
						                <asp:TemplateField HeaderText="gvIpAddress" meta:resourcekey="gvIpAddress">
							                <ItemTemplate>
								                <%# Eval("IPAddress")%>
							                </ItemTemplate>
						                </asp:TemplateField>
						                <asp:TemplateField HeaderText="gvPrimary" meta:resourcekey="gvPrimary" ItemStyle-Width="50">
							                <ItemTemplate>
							                    <div style="text-align:center">
							                        &nbsp;
								                    <asp:Image ID="Image2" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />
								                </div>
							                </ItemTemplate>
						                </asp:TemplateField>
					                </Columns>
				                </asp:GridView>
				            </div>
    				        
				            <div style="margin-top: 4px;">
                                <asp:Button ID="btnAddPrivateAddress" runat="server" meta:resourcekey="btnAddPrivateAddress"
                                    Text="Add" CssClass="btn btn-primary" onclick="btnAddPrivateAddress_Click" />
				                <asp:Button id="btnSetPrimaryPrivate" runat="server" Text="Set As Primary"
				                    meta:resourcekey="btnSetPrimaryPrivate" CssClass="btn btn-success" 
                                    CausesValidation="false" onclick="btnSetPrimaryPrivate_Click"></asp:Button>
				                <asp:Button id="btnDeletePrivate" runat="server" Text="Delete Selected"
				                    meta:resourcekey="btnDeletePrivate" CssClass="btn btn-danger" CausesValidation="false" 
                                    onclick="btnDeletePrivate_Click"></asp:Button>
                            </div>
                            
				            <br />
			                <asp:Localize ID="locTotalPrivate" runat="server"
			                    meta:resourcekey="locTotalPrivate" Text="IP addresses:"></asp:Localize>
			                <asp:Label ID="lblTotalPrivate" runat="server" CssClass="NormalBold">0</asp:Label>
                        </asp:Panel>
                    
                        <br />
                    </asp:Panel>
				    
			    </div>
		    </div>
	    </div>
    	
