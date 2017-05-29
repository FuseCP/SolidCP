<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServersEditServer.ascx.cs" Inherits="SolidCP.Portal.VirtualServersEditServer" %>
<%@ Register Src="GlobalDnsRecordsControl.ascx" TagName="GlobalDnsRecordsControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<div class="panel-body form-horizontal">
    <asp:ValidationSummary ID="summary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="VirtualServer" />
    <div class="form-group">
        <label class="col-sm-2">
            <asp:Label ID="lblServerName" runat="server" meta:resourcekey="lblServerName"></asp:Label></label>
        <div class="col-sm-10">
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="VirtualServerNameValidator" runat="server" ControlToValidate="txtName"
                ValidationGroup="VirtualServer" meta:resourcekey="VirtualServerNameValidator"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group">
        <label class="col-sm-2">
            <asp:Label ID="lblServerComments" runat="server" meta:resourcekey="lblServerComments"></asp:Label></label>
        <div class="col-sm-10">
            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" Rows="2" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>
    <scp:CollapsiblePanel ID="secServices" runat="server"
        TargetControlID="ServicesPanel" ResourceKey="secServices" Text="Services"></scp:CollapsiblePanel>
    <asp:Panel ID="ServicesPanel" runat="server">
        <div class="form-group" id="rowPrimaryGroup" runat="server">
            <label class="col-sm-2">
                <asp:Label ID="lblPDR" runat="server" meta:resourcekey="lblPDR" Text="Primary distribution group:"></asp:Label></label>
            <div class="col-sm-10">
                <asp:DropDownList ID="ddlPrimaryGroup" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
            </div>
        </div>
        <div class="text-right">
            <CPCC:StyleButton ID="btnAddServices" runat="server" CssClass="btn btn-primary" OnClick="btnAddServices_Click">
                <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddServices" />
            </CPCC:StyleButton>
            <CPCC:StyleButton ID="btnRemoveSelected" runat="server" CssClass="btn btn-danger"  OnClick="btnRemoveSelected_Click" >
                <i class="fa fa-trash">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRemoveSelected" />
            </CPCC:StyleButton>
            <br /><br/>
        </div>
        <asp:DataList ID="dlServiceGroups" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server" DataKeyField="GroupID" OnItemDataBound="dlServiceGroups_ItemDataBound">
            <ItemTemplate>
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>
                    </div>
                    <div class="panel-body">
<fieldset  id="tblGroupDistribution" runat="server">
                        <div class="form-group" id="rowBound" runat="server">
                            <label class="col-sm-2">Distribution</label>
                            <div class="col-sm-10">
                                <asp:CheckBox ID="chkBind" runat="server" Text="Bind to primary" CssClass="form-control"
                                    AutoPostBack="true" Checked='<%# Eval("BindDistributionToPrimary") %>' />
                            </div>
                        </div>
                        <div class="form-group" id="rowDistType" runat="server">
                            <label class="col-sm-2">Distribution Type</label>
                            <div class="col-sm-10">
                                <asp:DropDownList ID="ddlDistType" runat="server" CssClass="form-control" SelectedValue='<%# Eval("DistributionType") %>'>
                                    <asp:ListItem Value="1">Balanced</asp:ListItem>
                                    <asp:ListItem Value="2">Randomized</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
						</fieldset>
                        <div class="row">
                            <asp:DataList ID="dlServices" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                DataSource='<%# GetGroupServices((int)Eval("GroupID")) %>'
                                DataKeyField="ServiceID">
                                <%--<ItemStyle CssClass="Brick" VerticalAlign="Top" HorizontalAlign="Left"></ItemStyle>--%>
                                <ItemTemplate>
                                    <div class="col-md-6">
                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                <h3 class="panel-title" style="line-height:inherit;white-space:nowrap;overflow:hidden;" title="<%# Eval("ServerName") %>">
                                                    <i class="fa fa-server">&nbsp;</i>&nbsp;<%# Eval("ServerName") %>
                                                </h3>
                                            </div>
                                            <div class="panel-body">
                                                <div class="checkbox">
                                                    <label>
                                                        <asp:CheckBox ID="chkSelected" runat="server" />
                                                        <%# Eval("ServiceName") %>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </asp:Panel>

    <scp:CollapsiblePanel ID="secDnsRecords" runat="server" IsCollapsed="true"
        TargetControlID="DnsRecordsPanel" ResourceKey="secDnsRecords" Text="DNS Records Template">
	</scp:CollapsiblePanel>

    <asp:Panel ID="DnsRecordsPanel" runat="server">
 
          <uc1:GlobalDnsRecordsControl ID="GlobalDnsRecordsControl" runat="server" ServerIdParam="ServerID" />

    </asp:Panel>


    <scp:CollapsiblePanel ID="secInstantAlias" runat="server" IsCollapsed="true"
        TargetControlID="InstantAliasPanel" ResourceKey="secInstantAlias" Text="Instant Alias">
	</scp:CollapsiblePanel>
    <asp:Panel ID="InstantAliasPanel" runat="server">
         <div class="form-inline">
      customerdomain.com.&nbsp;
       <asp:TextBox ID="txtInstantAlias" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="DomainFormatValidator" ValidationGroup="VirtualServer" runat="server" meta:resourcekey="DomainFormatValidator"
                        ControlToValidate="txtInstantAlias" Display="Dynamic" SetFocusOnError="true"
                        ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,16}[a-zA-Z]{2,15}$"></asp:RegularExpressionValidator>
             </div>
    </asp:Panel>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton ID="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete server?');"><i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText" />
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton ID="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"><i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel" />
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton ID="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" CausesValidation="true" ValidationGroup="VirtualServer"><i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate" />
    </CPCC:StyleButton>
    &nbsp;
</div>
