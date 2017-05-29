<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAccountVLanAdd.ascx.cs"
    Inherits="SolidCP.Portal.VdcAccountVLanAdd" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
 
        <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Image ID="imgIcon" SkinID="VLanNetwork" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Add VLan to user"></asp:Localize>
                </div>
            <div class="panel-body form-horizontal">
            <scp:menu id="menu" runat="server" selecteditem="vdc_account_vlan_network" />
            <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <table cellspacing="0" cellpadding="2" width="100%">
                        <tr>
                            <td class="SubHead" style="width: 150px;">
                                <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
                            </td>
                            <td class="Huge">
                                <asp:Literal ID="lblUsername" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top">
                                <asp:Label ID="lblVLanID" runat="server" meta:resourcekey="lblVLanID" Text="VLan:" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbVLanID" runat="server" CssClass="NormalTextBox" />
                                <asp:RequiredFieldValidator ID="VLanIDValidator" runat="server" ErrorMessage="*"
                                    Display="Dynamic" ControlToValidate="tbVLanID" />
                                <asp:RegularExpressionValidator  ID="VLanIDRegExpValidator" runat="server" ErrorMessage="*"
                                    Display="Dynamic" ControlToValidate="tbVLanID" ValidationExpression="^\d+$" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top">
                                <asp:Label ID="lblComment" runat="server" meta:resourcekey="lblComment" Text="Comment:" />
                            </td>
                            <td class="NormalBold">
                                <asp:TextBox ID="tbComment" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                    </table>
     
                </div>
               <div class="panel-footer text-right">
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btAddVLan" CssClass="btn btn-success" runat="server" OnClick="btAddVLan_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddVLan"/> </CPCC:StyleButton>
                    </div>
            </div>
            </div>
            </div>
            <div class="alert alert-info">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>