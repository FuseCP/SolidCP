<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignedRDSServers.ascx.cs" Inherits="SolidCP.Portal.RDS.AssignedRDSServers" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <asp:Image ID="imgRDSServers" SkinID="EnterpriseRDSServers48" runat="server" />
    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Assigned RDS Servers"></asp:Localize>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnAddServerToOrg" runat="server" CssClass="btn btn-primary" OnClick="btnAddServerToOrg_Click">
        <i class="fa fa-plus">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddServerToOrg" />
    </CPCC:StyleButton>
</div>
<div class="panel-body">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="row">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-7 col-md-offset-5 text-right form-inline">
            <asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlPageSize_SelectedIndexChanged">   
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem Selected="True">20</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                    </asp:DropDownList>  
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-btn">
                        <CPCC:StyleButton ID="cmdSearch" runat="server" CausesValidation="false" CssClass="btn btn-primary">
                            <i class="fa fa-search" aria-hidden="true"></i>
                        </CPCC:StyleButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>
<asp:GridView ID="gvRDSAssignedServers" runat="server" AutoGenerateColumns="False" EnableViewState="true" Width="100%" EmptyDataText="gvRDSAssignedServers" CssSelectorClass="NormalGridView" OnRowCommand="gvRDSAssignedServers_RowCommand" AllowPaging="True" AllowSorting="True" DataSourceID="odsRDSAssignedServersPaged" PageSize="20">
    <Columns>
        <asp:TemplateField HeaderText="gvRDSServerName" SortExpression="Name">
            <ItemStyle Width="80%"></ItemStyle>
            <ItemTemplate>
                <asp:Label id="litRDSServerName" runat="server">
                    <%# Eval("Name") %>
                </asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemStyle Width="10%"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton ID="EnableLinkButton" CssClass="btn btn-success" runat="server" Visible='<%# Eval("RdsCollectionId") != null && !Convert.ToBoolean(Eval("ConnectionEnabled")) %>' CommandName="EnableItem" CommandArgument='<%# Eval("Id") %>' meta:resourcekey="cmdEnable"></asp:LinkButton>
                <asp:LinkButton ID="DisableLinkButton" CssClass="btn btn-danger" runat="server" Visible='<%# Eval("RdsCollectionId") != null && Convert.ToBoolean(Eval("ConnectionEnabled")) %>' CommandName="DisableItem" CommandArgument='<%# Eval("Id") %>' meta:resourcekey="cmdDisable"></asp:LinkButton>                                    
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton id="imgRemove1" CssClass="btn btn-sm btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>' Visible='<%# Eval("RdsCollectionId") == null %>' OnClientClick="return confirm('Are you sure you want to remove selected server?')"> 
                    &nbsp;<i class="fa fa-trash-o"></i>&nbsp;
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<div class="panel-footer">
    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="RDS Servers:"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="rdsServersQuota" runat="server" QuotaTypeId="2" DisplayGauge="true"/>
</div>
<asp:ObjectDataSource ID="odsRDSAssignedServersPaged" runat="server" EnablePaging="True" SelectCountMethod="GetOrganizationRdsServersPagedCount" SelectMethod="GetOrganizationRdsServersPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.RDSHelper">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>
