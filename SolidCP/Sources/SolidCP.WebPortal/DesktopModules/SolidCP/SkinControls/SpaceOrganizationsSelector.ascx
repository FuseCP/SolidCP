<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceOrganizationsSelector.ascx.cs" Inherits="SolidCP.Portal.SkinControls.SpaceOrganizationsSelector" %>
<div class="col-xs-4">
    <div id="spanOrgsSelector" class="OrgsSelector" runat="server" >
    <div class="input-group col-xs-12">
            <asp:DropDownList ID="ddlSpaceOrgs" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSpaceOrgs_SelectedIndexChanged" EnableViewState="true" AutoPostBack="true">
            </asp:DropDownList>
        <span class="input-group-btn">
            <asp:HyperLink ID="lnkOrgnsList" runat="server" CssClass="btn btn-primary"><i class="fa fa-pencil">&nbsp</i>&nbsp;Edit</asp:HyperLink>
        </span>
    </div>
    </div>
</div>