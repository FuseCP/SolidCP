<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceQuotasControl.ascx.cs" Inherits="SolidCP.Portal.SpaceQuotasControl" %>
<%@ Register Src="UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="uc1" %>

<asp:Repeater ID="dlGroups" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div class="panel panel-info">
            <div class="panel-heading">
                <asp:Panel ID="GroupPanel" runat="server" visible='<%# IsGroupVisible((int)Eval("GroupID")) %>'>
                    <strong><%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %></strong>
                </asp:Panel>
            </div>
            <div class="panel-body">
                <asp:Repeater ID="dlQuotas" runat="server" DataSource='<%# GetGroupQuotas((int)Eval("GroupID")) %>' EnableViewState="false">
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-xs-6 text-right">
                                <%# GetQuotaTitle((string)Eval("QuotaName"), (object)Eval("QuotaDescription"))%>:
                            </div>
                            <div class="col-xs-6">
                                <uc1:QuotaViewer ID="quota" runat="server" QuotaTypeId='<%# Eval("QuotaTypeId") %>' QuotaUsedValue='<%# Eval("QuotaUsedValue") %>' QuotaValue='<%# Eval("QuotaValue") %>' QuotaAvailable='<%# Eval("QuotaAvailable") %>'/>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>