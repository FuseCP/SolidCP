<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerServicesControl.ascx.cs" Inherits="SolidCP.Portal.ServerServicesControl" %>
<asp:Repeater id="dlServiceGroups" Runat="server" >
	<ItemTemplate>
        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 class="panel-title"><%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>
                    <asp:hyperlink id="lnkAddService" runat="server"
				        NavigateUrl='<%# EditServiceUrl("GroupID", Eval("GroupID").ToString(), "add_service") %>'
				        CssClass="btn btn-default btn-sm pull-right">
                        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server"  meta:resourcekey="lnkAddService"/>
					</asp:hyperlink>
                </h3>
            </div>

            <ul class="list-group">
                <asp:Repeater ID="dlServices" Runat="server" DataSource='<%# GetGroupServices((int)Eval("GroupID")) %>'>
						<ItemTemplate>
							<li class="list-group-item">
								<asp:hyperlink id="lnkEditService" runat="server" NavigateUrl='<%# EditServiceUrl("ServiceID", Eval("ServiceID").ToString(), "edit_service") %>' Width=100% Height=100% ToolTip='<%# Eval("Comments") %>'>
									<%# Eval("ServiceName") %>
								</asp:hyperlink>
							 </li>
						</ItemTemplate>
					</asp:Repeater>
            </ul>
        </div>
	</ItemTemplate>
</asp:Repeater>
