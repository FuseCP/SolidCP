<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAccountVLanNetwork.ascx.cs"
    Inherits="SolidCP.Portal.VPSForPC.VdcAccountVLanNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>

        <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Image ID="Image1" SkinID="VLanNetwork" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Available VLans"></asp:Localize>
                </div>
                <div class="panel-body form-horizontal">
                    <div class="FormButtonsBar right">
                    <CPCC:StyleButton id="btnAddVlan" CssClass="btn btn-success" runat="server" OnClick="btnAddVlan_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddVlanText"/> </CPCC:StyleButton>
                    </div>
                    </div>
            <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="vdc_account_vlan_network" />
            <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <asp:GridView ID="gvVlans" runat="server" AutoGenerateColumns="false" CssSelectorClass="NormalGridView"
                        EmptyDataText="User has no VLANs" OnRowCommand="gvVlans_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="VLanID" HeaderText="VLan" />
                            <asp:BoundField DataField="Comment" HeaderText="Comment" ItemStyle-Wrap="true" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='DeleteItem' CommandArgument='<%# Eval("VLanID") %>' OnClientClick="return confirm('Remove this item?');"> 
                                        &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                                    </CPCC:StyleButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
                </div>
            </div>
            <div class="alert alert-info">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>