<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsNetwork" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<script type="text/javascript">
    function SelectAllCheckboxes(box) {
        var state = box.checked;
        var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName("INPUT");
        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
                elm[i].checked = state;
    }
</script>


<div class="Content">
    <div class="Center">
        <div class="panel-body form-horizontal">
            <scp:ServerTabs ID="tabs" runat="server" SelectedTab="vps_network" />
            <scp:SimpleMessageBox ID="messageBox" runat="server" />

            <scp:CollapsiblePanel ID="secRealNetwork" runat="server"
                TargetControlID="RealNetworkPanel" meta:ResourceKey="secRealNetwork" Text="Virtual Machine Networks details"></scp:CollapsiblePanel>
            <asp:Panel ID="RealNetworkPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
                <div style="margin-top: 4px; margin-bottom: 10px;">
                    <asp:Button ID="btnRestoreExternalAddress" runat="server" meta:resourcekey="btnRestoreExternalAddress"
                        Text="Restore External IPs" CssClass="btn btn-primary" OnClick="btnRestoreExternalAddress_Click" />
                    <asp:Button ID="btnRestorePrivateAddress" runat="server" meta:resourcekey="btnRestorePrivateAddress"
                        Text="Restore Private IPs" CssClass="btn btn-primary" OnClick="btnRestorePrivateByInject_Click" />
                </div>
                <asp:Repeater ID="repVMNetwork" runat="server">
                    <ItemTemplate>
                        <div class="form-group">
                            <table style="border-collapse: separate; border-spacing: 15px 5px; padding-left: 5px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="locAdapterName" meta:resourcekey="locAdapterName" runat="server" Text="Adapter Name:" CssClass="col-sm-20" AssociatedControlID="litAdapterName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="locAdapterMAC" meta:resourcekey="locAdapterMAC" runat="server" Text="MAC:" CssClass="col-sm-20" AssociatedControlID="litAdapterMAC"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="locAdapterAddresses" meta:resourcekey="locAdapterAddresses" runat="server" Text="IP addresses:" CssClass="col-sm-20" AssociatedControlID="gvVMNetwork"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="locAdapterVLAN" meta:resourcekey="locAdapterVLAN" runat="server" Visible='<%# IsVlanEnabled(Eval("vlan")) %>' Text="VLAN:" CssClass="col-sm-20" AssociatedControlID="litAdapterVLAN"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="vertical-align: top;">
                                    <td style="width: 140px;">
                                        <asp:Literal ID="litAdapterName" runat="server" meta:resourcekey="litAdaperName" Text='<%# Eval("Name") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litAdapterMAC" runat="server" meta:resourcekey="litAdapterMAC" Text='<%# Eval("MacAddress") %>'></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:GridView ID="gvVMNetwork" runat="server" CssSelectorClass="NormalGridView" AutoGenerateColumns="false" EmptyDataText="no data">
                                            <Columns>
                                                <asp:BoundField DataField="N" HeaderText="N" />
                                                <asp:BoundField DataField="IP" HeaderText="gvIpAddress" meta:resourcekey="gvIpAddress" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litAdapterVLAN" runat="server" meta:resourcekey="litAdapterVLAN" Visible='<%# IsVlanEnabled(Eval("vlan")) %>' Text='<%# Eval("vlan") %>'></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="VMNetworkError" runat="server" Visible="false"></asp:Literal>
                <br />
            </asp:Panel>

            <scp:CollapsiblePanel ID="secExternalNetwork" runat="server"
                TargetControlID="ExternalNetworkPanel" meta:ResourceKey="secExternalNetwork" Text="External Network"></scp:CollapsiblePanel>
            <asp:Panel ID="ExternalNetworkPanel" runat="server" Height="0" Style="overflow: hidden;">

                <table style="border-collapse: separate; border-spacing: 3px 1px;">
                    <tr>
                        <td>
                            <asp:Localize ID="locExtAddress" runat="server"
                                meta:resourcekey="locExtAddress" Text="Server address:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litExtAddress" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locExtSubnet" runat="server"
                                meta:resourcekey="locExtSubnet" Text="Subnet mask:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litExtSubnet" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locExtGateway" runat="server"
                                meta:resourcekey="locExtGateway" Text="Default gateway:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litExtGateway" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locExtVLAN" runat="server"
                                meta:resourcekey="locVLAN" Text="VLAN:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litExtVLAN" runat="server" Text="" /></td>
                    </tr>
                </table>

                <div style="width: 400px;">
                    <asp:GridView ID="gvExternalAddresses" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="gvExternalAddresses" CssSelectorClass="NormalGridView"
                        DataKeyNames="AddressID">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# !(bool)Eval("IsPrimary") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="IPAddress" HeaderText="gvIpAddress" meta:resourcekey="gvIpAddress" />
                            <asp:BoundField DataField="SubnetMask" HeaderText="gvSubnetMask" meta:resourcekey="gvSubnetMask" />
                            <asp:BoundField DataField="DefaultGateway" HeaderText="gvDefaultGateway" meta:resourcekey="gvDefaultGateway" />
                            <asp:TemplateField HeaderText="gvPrimary" meta:resourcekey="gvPrimary" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        &nbsp;
								                <asp:Image ID="Image2" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div style="margin-top: 4px;">
                    <asp:Button ID="btnAddExternalAddress" runat="server" meta:resourcekey="btnAddExternalAddress"
                        Text="Add" CssClass="btn btn-primary" OnClick="btnAddExternalAddress_Click" />
                    <asp:Button ID="btnSetPrimaryExternal" runat="server" Text="Set As Primary"
                        meta:resourcekey="btnSetPrimaryExternal" CssClass="btn btn-success"
                        CausesValidation="false" OnClick="btnSetPrimaryExternal_Click"></asp:Button>
                    <asp:Button ID="btnDeleteExternal" runat="server" Text="Delete Selected"
                        meta:resourcekey="btnDeleteExternal" CssClass="btn btn-danger" CausesValidation="false"
                        OnClick="btnDeleteExternal_Click"></asp:Button>
                    <asp:Button ID="btnDeleteExternalByInject" runat="server" Text="Delete Selected By Inject"
                        meta:resourcekey="btnDeleteExternalByInject" CssClass="btn btn-danger" CausesValidation="false"
                        OnClick="btnDeleteExternalByInject_Click"></asp:Button>
                </div>

                <br />
                <asp:Localize ID="locTotalExternal" runat="server"
                    meta:resourcekey="locTotalExternal" Text="IP addresses:"></asp:Localize>
                <asp:Label ID="lblTotalExternal" runat="server" CssClass="NormalBold">0</asp:Label>
                <br />
                <br />
                <br />
            </asp:Panel>


            <scp:CollapsiblePanel ID="secPrivateNetwork" runat="server"
                TargetControlID="PrivateNetworkPanel" meta:ResourceKey="secPrivateNetwork" Text="Private Network"></scp:CollapsiblePanel>
            <asp:Panel ID="PrivateNetworkPanel" runat="server" Height="0" Style="overflow: hidden;">

                <table style="border-collapse: separate; border-spacing: 3px 1px;">
                    <tr>
                        <td>
                            <asp:Localize ID="locPrivAddress" runat="server"
                                meta:resourcekey="locPrivAddress" Text="Server address:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litPrivAddress" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locPrivSubnet" runat="server"
                                meta:resourcekey="locPrivSubnet" Text="Subnet mask:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litPrivSubnet" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locPrivGateway" runat="server"
                                meta:resourcekey="locPrivGateway" Text="Default gateway:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litPrivGateway" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="locPrivVLAN" runat="server"
                                meta:resourcekey="locVLAN" Text="VLAN:" /></td>
                        <td class="NormalBold">
                            <asp:Literal ID="litPrivVLAN" runat="server" Text="" /></td>
                    </tr>
                </table>

                <asp:Panel ID="PrivateAddressesPanel" runat="server">

                    <div style="width: 400px;">
                        <asp:GridView ID="gvPrivateAddresses" runat="server" AutoGenerateColumns="False"
                            EmptyDataText="gvPrivateAddresses" CssSelectorClass="NormalGridView"
                            DataKeyNames="AddressID">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# !(bool)Eval("IsPrimary") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="gvIpAddress" meta:resourcekey="gvIpAddress">
                                    <ItemTemplate>
                                        <%# Eval("IPAddress")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SubnetMask" HeaderText="gvSubnetMask" meta:resourcekey="gvSubnetMask" />
                                <asp:BoundField DataField="DefaultGateway" HeaderText="gvDefaultGateway" meta:resourcekey="gvDefaultGateway" />
                                <asp:TemplateField HeaderText="gvPrimary" meta:resourcekey="gvPrimary" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            &nbsp;
								                    <asp:Image ID="Image2" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div style="margin-top: 4px;">
                        <asp:Button ID="btnAddPrivateAddress" runat="server" meta:resourcekey="btnAddPrivateAddress"
                            Text="Add" CssClass="btn btn-primary" OnClick="btnAddPrivateAddress_Click" />
                        <asp:Button ID="btnSetPrimaryPrivate" runat="server" Text="Set As Primary"
                            meta:resourcekey="btnSetPrimaryPrivate" CssClass="btn btn-success"
                            CausesValidation="false" OnClick="btnSetPrimaryPrivate_Click"></asp:Button>
                        <asp:Button ID="btnDeletePrivate" runat="server" Text="Delete Selected"
                            meta:resourcekey="btnDeletePrivate" CssClass="btn btn-danger" CausesValidation="false"
                            OnClick="btnDeletePrivate_Click"></asp:Button>
                        <asp:Button ID="btnDeletePrivateByInject" runat="server" Text="Delete Selected By Inject"
                            meta:resourcekey="btnDeletePrivateByInject" CssClass="btn btn-danger" CausesValidation="false"
                            OnClick="btnDeletePrivateByInject_Click"></asp:Button>
                    </div>

                    <br />
                    <asp:Localize ID="locTotalPrivate" runat="server"
                        meta:resourcekey="locTotalPrivate" Text="IP addresses:"></asp:Localize>
                    <asp:Label ID="lblTotalPrivate" runat="server" CssClass="NormalBold">0</asp:Label>
                </asp:Panel>

                <br />
            </asp:Panel>

        </div>
    </div>
</div>

