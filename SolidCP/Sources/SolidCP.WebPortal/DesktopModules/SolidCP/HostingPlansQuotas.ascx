<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlansQuotas.ascx.cs" Inherits="SolidCP.Portal.HostingPlansQuotas" %>
<%@ Register Src="UserControls/QuotaEditor.ascx" TagName="QuotaEditor" TagPrefix="uc1" %>

<asp:Repeater ID="dlGroups" runat="server">
    <ItemTemplate>
        <asp:Panel ID="GroupPanel" runat="server" CssClass="panel panel-info">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-xs-6">
                        <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# (bool)Eval("Enabled") & (bool)Eval("ParentEnabled") %>' AutoPostBack="true" Enabled='<%# Eval("ParentEnabled") %>' Text='<%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>' />
                        <asp:Literal ID="groupId" runat=server Text='<%# Eval("GroupID") %>' Visible=false></asp:Literal>
                    </div>
                    <div class="col-xs-6 text-right">
                        <asp:CheckBox ID="chkCountDiskspace" runat="server" meta:resourcekey="chkCountDiskspace" Checked='<%# Eval("CalculateDiskspace") %>' Visible='<%# IsPlan %>' Text="Count Diskspace" />&nbsp;
                        <asp:CheckBox ID="chkCountBandwidth" runat="server" meta:resourcekey="chkCountBandwidth" Checked='<%# Eval("CalculateBandwidth") %>' Visible='<%# IsPlan %>' Text="Count Bandwidth" />&nbsp;
                    </div>
                </div>
            </div>
            <asp:Panel ID="QuotaPanel" runat="server" CssClass="panel-body">
                <asp:DataList ID="dlQuotas" runat="server" CssClass="table table-hover" DataSource='<%# GetGroupQuotas((int)Eval("GroupID")) %>' RepeatColumns="1" Width="100%">
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-xs-6 col-md-3">
                                <%# GetSharedLocalizedStringNotEmpty((string)Eval("QuotaName"), Eval("QuotaDescription"))%>:
                            </div>
                            <div class="col-xs-6 col-md-9">
                                <uc1:QuotaEditor id="quotaEditor" runat="server" QuotaID='<%# Eval("QuotaID") %>' QuotaTypeID='<%# Eval("QuotaTypeID") %>' QuotaValue='<%# Eval("QuotaValue") %>' ParentQuotaValue='<%# Eval("ParentQuotaValue") %>'>
                                </uc1:QuotaEditor>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </asp:Panel>
        </asp:Panel>
    </ItemTemplate>
</asp:Repeater>