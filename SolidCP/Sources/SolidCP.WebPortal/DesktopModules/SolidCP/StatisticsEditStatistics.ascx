<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatisticsEditStatistics.ascx.cs" Inherits="SolidCP.Portal.StatisticsEditStatistics" %>
<div class="panel-body form-horizontal">
    <div class="row" id="newRow" runat="server">
        <div class="form-group col-sm-8">
            <asp:Label runat="server" CssClass="control-label col-sm-3" AssociatedControlID="lblDomainName">
                <asp:Localize ID="lblWebSite" runat="server" meta:resourcekey="lblWebSite" Text="Web Site:"></asp:Localize>
            </asp:Label>
            <div class="form-inline">
                <asp:Label ID="lblDomainName" runat="server" CssClass="h3"></asp:Label>
                <asp:DropDownList ID="ddlWebSites" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="Name"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="valRequireWebSite" runat="server" ErrorMessage="*" ControlToValidate="ddlWebSites"></asp:RequiredFieldValidator></td>
			</div>
        </div>
    </div>
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete Site?');">
        <i class="fa fa-trash-o">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnDeleteText"/>
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" useSubmitBehavior="false">
        <i class="fa fa-refresh">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnUpdate"/>
    </CPCC:StyleButton>
</div>