<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationUserResetPassword.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationUserResetPassword" %>

<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>


				<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Reset Password"></asp:Localize>
                    -
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>

                <div class="panel-body form-horizontal">
                    <asp:UpdatePanel ID="PasswrodResetUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            
                            <table>
                                <tr>
                                    <td class="FormLabel150">
                                        <asp:Localize ID="locSendTo" runat="server" meta:resourcekey="locSendTo" Text="Send to:"></asp:Localize></td>
                                    <td class="FormRBtnL">
                                        <asp:RadioButton ID="rbtnEmail" runat="server" meta:resourcekey="rbtnEmail" Text="Email" GroupName="SendToGroup" AutoPostBack="true"  Checked="true" OnCheckedChanged="SendToGroupCheckedChanged" />
                                        <asp:RadioButton ID="rbtnMobile" runat="server" meta:resourcekey="rbtnMobile" Text="Mobile" GroupName="SendToGroup" AutoPostBack="true" OnCheckedChanged="SendToGroupCheckedChanged" />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr id="EmailRow" runat="server">
                                    <td class="FormLabel150" valign="top">
                                        <asp:Localize ID="locEmailAddress" runat="server" meta:resourcekey="locEmailAddress"></asp:Localize></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtEmailAddress" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="valEmailAddress" runat="server" ErrorMessage="*" ControlToValidate="txtEmailAddress" ValidationGroup="ResetUserPassword"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="ResetUserPassword" ControlToValidate="txtEmailAddress" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr id="MobileRow" runat="server" visible="False">
                                    <td class="FormLabel150" valign="top">
                                        <asp:Localize ID="locMobile" runat="server" meta:resourcekey="locMobile"></asp:Localize></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMobile" CssClass="form-control" />
                                        <asp:CheckBox ID="chkSaveAsMobile" runat="server" Text="Save as mobile" meta:resourcekey="chkSaveAsMobile" Checked="False" />
                                        <asp:RequiredFieldValidator ID="valMobile" runat="server" ErrorMessage="*" ControlToValidate="txtMobile" ValidationGroup="ResetUserPassword"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regexMobileValid" runat="server" ValidationExpression="^\+?\d+$" ValidationGroup="ResetUserPassword" ControlToValidate="txtMobile" ErrorMessage="Invalid Mobile Format"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FormLabel150">
                                        <asp:Localize ID="locReason" runat="server" meta:resourcekey="locReason" Text="Reason:"></asp:Localize></td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="valReason" runat="server" ErrorMessage="*" ControlToValidate="txtReason" ValidationGroup="ResetUserPassword"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

                    <div class="panel-footer text-right">
                        <CPCC:StyleButton id="btnResetPassoword" CssClass="btn btn-success" runat="server" OnClick="btnResetPassoword_Click" ValidationGroup="ResetUserPassword"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnResetPassowordText"/> </CPCC:StyleButton>
                    </div>