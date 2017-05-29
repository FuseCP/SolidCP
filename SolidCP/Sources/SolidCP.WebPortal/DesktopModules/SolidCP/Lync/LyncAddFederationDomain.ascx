<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LyncAddFederationDomain.ascx.cs" Inherits="SolidCP.Portal.LyncAddFederationDomain" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

			<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="LyncLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
              </h3>
                          </div>
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    <table id="AddFederationDomain"   runat="server" width="100%"> 					    
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locDomainName" runat="server" meta:resourcekey="locDomainName" Text="Domain Name: *"></asp:Localize>
                            </td>
                            <td>                                
                                <asp:TextBox ID="DomainName" runat="server" Width="300" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator id="DomainRequiredValidator" runat="server" meta:resourcekey="DomainRequiredValidator"
                                    ControlToValidate="DomainName" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"></asp:RequiredFieldValidator>
		                        <asp:RegularExpressionValidator id="DomainFormatValidator" runat="server" meta:resourcekey="DomainFormatValidator"
		                            ControlToValidate="DomainName" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"
		                            ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,15}$"></asp:RegularExpressionValidator>
                            </td>
					    </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locProxyFQDN" runat="server" meta:resourcekey="locProxyFQDN" Text="Proxy FQDN: "></asp:Localize>
                            </td>
                            <td>                                
                                <asp:TextBox ID="ProxyFQDN" runat="server" Width="300" CssClass="form-control"></asp:TextBox>
		                        <asp:RegularExpressionValidator id="ProxyFqdnFormatValidator" runat="server" meta:resourcekey="ProxyFqdnFormatValidator"
		                            ControlToValidate="ProxyFQDN" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"
		                            ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,15}$"></asp:RegularExpressionValidator>
                            </td>
					    </tr>
                    </table>
                    <br />

                </div>
                    <div class="panel-footer text-right">
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding Domain...');" ValidationGroup="Domain"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>&nbsp;
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="Domain" />
                    </div>