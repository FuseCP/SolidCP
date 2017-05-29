<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsPermissions.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsPermissions" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_permissions" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <div class="FormButtonsBarClean">
                        <asp:CheckBox ID="chkOverride" runat="server" meta:resourcekey="chkOverride"
                                Text="Override space permissions" AutoPostBack="true" />
                    </div>
                    
		            <asp:GridView ID="gvVpsPermissions" runat="server" AutoGenerateColumns="False"
			            EmptyDataText="gvVpsPermissions" CssSelectorClass="NormalGridView">
			            <Columns>
				            <asp:BoundField HeaderText="columnUsername" DataField="Username" />
				            <asp:TemplateField HeaderText="columnChangeState" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkChangeState" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnChangeConfig" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkChangeConfig" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnManageSnapshots" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkManageSnapshots" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnDeleteVps" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkDeleteVps" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
				            <asp:TemplateField HeaderText="columnReinstallVps" ItemStyle-HorizontalAlign="Center">
				                <ItemTemplate>
				                    <asp:CheckBox ID="chkReinstallVps" runat="server" />
				                </ItemTemplate>
				            </asp:TemplateField>
			            </Columns>
		            </asp:GridView>
		            <br />
                    <CPCC:StyleButton id="btnUpdateVpsPermissions" CssClass="btn btn-success" runat="server" CausesValidation="false"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateVpsPermissionsText"/> </CPCC:StyleButton>
                    <br />
				    
			    </div>
		    </div>
	    </div>
