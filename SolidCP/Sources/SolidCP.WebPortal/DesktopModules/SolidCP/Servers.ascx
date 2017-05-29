<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Servers.ascx.cs" Inherits="SolidCP.Portal.Servers" %>
<%@ Import Namespace="SolidCP.Portal" %>
<div class="buttons-in-panel-header">
    <CPCC:StyleButton ID="btnAddItem"  runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click">
        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem" />
    </CPCC:StyleButton>
</div>
<section>
    <div class="panel-body">
    <asp:Repeater ID="dlServers" Runat="server">
        <ItemTemplate>
            <div class="col-sm-12 col-md-6 col-lg-4">
                <div class=" panel panel-info server-panel matchHeight">
                    <div class="panel-heading" style="min-height:43px;padding: 10px 0px;">
                        <div class="col-sm-8">
                            <h3 class="panel-title" style="line-height:inherit;white-space:nowrap;overflow:hidden;" title="<%# PortalAntiXSS.EncodeOld((string)Eval("ServerName")) %>">
                                <i class="fa fa-server" aria-hidden="true">&nbsp;</i>&nbsp;
                                <%# PortalAntiXSS.EncodeOld((string)Eval("ServerName")) %>
                            </h3>
                        </div>
                        <div class="col-sm-4 text-right">
                            <asp:hyperlink id=lnkEdit runat="server" CssClass="btn btn-default btn-sm" style="margin-top:-4px; margin-left: -18px;" NavigateUrl='<%# EditUrl("ServerID", Eval("ServerID").ToString(), "edit_server") %>'>
                                <i class="fa fa-cogs" aria-hidden="true">&nbsp;</i>&nbsp;Settings
                            </asp:hyperlink>
                        </div>
                    </div>
                    <ul class="list-group">
                        <li class="list-group-item">
                            <%# PortalAntiXSS.EncodeOld((string)Eval("Comments")) %>
                        </li>
                        <asp:Repeater ID="dlServices" Runat="server" DataSource='<%# GetServerServices((int)Eval("ServerID")) %>'>
                            <ItemTemplate>
                                <li class="list-group-item">
                                    <asp:hyperlink id="lnkEditService" runat="server" NavigateUrl='<%# EditUrl("ServiceID", Eval("ServiceID").ToString(), "edit_service", "ServerID=" + Eval("ServerID").ToString()) %>' ToolTip='<%# Eval("Comments") %>'>
                                        <%# Eval("ServiceName") %>
                                    </asp:hyperlink>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    </div>
</section>
<table id="tblEmptyList" runat="server" cellpadding="10" cellspacing="0" width="100%">
    <tr>
        <td class="Normal" align="center">
            <asp:Label ID="lblEmptyList" runat="server" meta:resourcekey="lblEmptyList" Text="Empty list..."></asp:Label>
        </td>
    </tr>
</table>
<div class="panel-footer text-right">
    <CPCC:StyleButton ID="StyleButton1"  runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click">
        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem" />
    </CPCC:StyleButton>
</div>