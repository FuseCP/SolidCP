<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSpaceBreadcrumb.ascx.cs" Inherits="SolidCP.Portal.SkinControls.UserSpaceBreadcrumb" %>
<%@ Register TagPrefix="scp" TagName="SpaceOrgsSelector" Src="SpaceOrganizationsSelector.ascx" %>
<div id="Breadcrumb">
    <div class="Path">
        <div class="col-xs-8">
        <asp:Repeater ID="repUsersPath" runat="server" OnItemDataBound="repUsersPath_ItemDataBound"
            EnableViewState="false">
            <ItemTemplate>
                <asp:HyperLink ID="lnkUser" runat="server"></asp:HyperLink>
            </ItemTemplate>
            <SeparatorTemplate>
                <asp:Image ID="imgSep" runat="server" SkinID="PathSeparatorWhite" />
            </SeparatorTemplate>
        </asp:Repeater>

        <span id="spanSpace" runat="server">
            <asp:Image ID="imgSep" runat="server" SkinID="PathSeparatorWhite" />
            <asp:Image ID="Image1" runat="server" SkinID="Space16" />
            <asp:HyperLink ID="lnkSpace" runat="server" Text="SpaceName" NavigateUrl="#"></asp:HyperLink>
        </span>

        <asp:Image ID="imgSep2" runat="server" SkinID="PathSeparatorWhite" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server"></asp:HyperLink>

        <span id="spanOrgn" class="OrgSpan" runat="server">
            <asp:Image ID="imgSep3" runat="server" SkinID="PathSeparatorWhite" />
            <asp:HyperLink ID="lnkOrgn" runat="server">Organization</asp:HyperLink>
            <asp:Image ID="imgSep4" runat="server" SkinID="PathSeparatorWhite" />
            <asp:HyperLink ID="lnkOrgCurPage" runat="server">Home</asp:HyperLink>
        </span>
        </div>
        <scp:SpaceOrgsSelector ID="SpaceOrgsSelector" runat="server" />
    </div>


    <div class="CurrentNode" runat="server" id="CurrentNode">
        <asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                            <div style="display:none;">
                                <div class="input-group col-xs-12">
                                    <span class="input-group-addon input-group-md ">
                                        <asp:Label ID="lblUserAccountName" runat="server" Text="Account-" CssClass="control-label large" Style="margin-right: 4px;" />
                                    </span>
                                    <asp:Panel runat="server" ID="pnlViewSpace">
                                        <span class="input-group-addon input-group-md">
                                            <asp:Label ID="cmdSpaceName" runat="server" CssClass="control-label" />
                                        </span>
                                        <asp:Label ID="lblSpaceDescription" runat="server" Visible="false"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEditSpace" runat="server" DefaultButton="cmdSave" Visible="false">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
                                        <span class="input-group-btn input-group-md">
                                            <CPCC:StyleButton ID="cmdCancel" runat="server" Text="Cancel" CssClass="btn btn-warning" OnClick="cmdCancel_Click" CausesValidation="false"><i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel" /></CPCC:StyleButton>
                                        </span>
                                    </asp:Panel>
                                </div>
                            </div>
                    <asp:RequiredFieldValidator ID="valRequireName" runat="server" ControlToValidate="txtName" ErrorMessage="*" Display="Dynamic" ValidationGroup="SpaceName"></asp:RequiredFieldValidator>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
