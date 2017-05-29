<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcPermissions.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcPermissions" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
    			
			        <scp:SimpleMessageBox id="messageBox" runat="server" />
    			
			        <scp:CollapsiblePanel id="secVdcPermissions" runat="server"
                        TargetControlID="VdcPermissionsPanel" meta:resourcekey="secVdcPermissions" Text="Virtual Data Center Permissions">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="VdcPermissionsPanel" runat="server" Height="0" style="overflow:hidden;">
                    
			            <asp:GridView ID="gvVdcPermissions" runat="server" AutoGenerateColumns="False"
				            EmptyDataText="gvVdcPermissions" CssSelectorClass="NormalGridView">
				            <Columns>
					            <asp:BoundField HeaderText="columnUsername" DataField="Username" />
					            <asp:TemplateField HeaderText="columnCreateVps" ItemStyle-HorizontalAlign="Center">
					                <ItemTemplate>
					                    <asp:CheckBox ID="chkCreateVps" runat="server" />
					                </ItemTemplate>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="columnExternalNetwork" ItemStyle-HorizontalAlign="Center">
					                <ItemTemplate>
					                    <asp:CheckBox ID="chkExternalNetwork" runat="server" />
					                </ItemTemplate>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="columnPrivateNetwork" ItemStyle-HorizontalAlign="Center">
					                <ItemTemplate>
					                    <asp:CheckBox ID="chkPrivateNetwork" runat="server" />
					                </ItemTemplate>
					            </asp:TemplateField>
					            <asp:TemplateField HeaderText="columnManagePermissions" ItemStyle-HorizontalAlign="Center">
					                <ItemTemplate>
					                    <asp:CheckBox ID="chkManagePermissions" runat="server" />
					                </ItemTemplate>
					            </asp:TemplateField>
				            </Columns>
			            </asp:GridView>
			            <br />
                        <CPCC:StyleButton id="btnUpdateVdcPermissions" CssClass="btn btn-success" runat="server" OnClick="btnUpdateVdcPermissions_Click" CausesValidation="false"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateVdcPermissionsText"/> </CPCC:StyleButton>
                        <br />
                        <br />
                    </asp:Panel>
                    
                    
			        <scp:CollapsiblePanel id="secVpsPermissions" runat="server"
                        TargetControlID="VpsPermissionsPanel" meta:resourcekey="secVpsPermissions" Text="Virtual Private Server Permissions">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="VpsPermissionsPanel" runat="server" Height="0" style="overflow:hidden;">
                    
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
                        <CPCC:StyleButton id="btnUpdateVpsPermissions" CssClass="btn btn-success" runat="server" onclick="btnUpdateVpsPermissions_Click" CausesValidation="false"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateVpsPermissionsText"/> </CPCC:StyleButton>
                        <br />
                        
                    </asp:Panel>

			    </div>
		    </div>
	    </div>
