<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMUserRoles.ascx.cs"
    Inherits="SolidCP.Portal.CRM.CRMUserRoles" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
				<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="CRMLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </h3>
                        </div>
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <div>

                        <div>
                            <table>
                                <tr height="23">
                                    <td class="FormLabel150"><asp:Localize runat="server" ID="locDisplayName" meta:resourcekey="locDisplayName" /></td>
                                    <td class="FormLabel150"><asp:Label runat="server" ID="lblDisplayName" /></td>
                                </tr>
                                <tr height="23">
                                    <td class="FormLabel150"><asp:Localize runat="server" ID="locEmailAddress" meta:resourcekey="locEmailAddress"/></td>
                                    <td class="FormLabel150"><asp:Label runat="server" ID="lblEmailAddress" /></td>
                                </tr>
                                <tr height="23">
                                    <td class="FormLabel150"><asp:Localize runat="server" ID="locDomainName" meta:resourcekey="locDomainName"/></td>
                                    <td class="FormLabel150"><asp:Label runat="server" ID="lblDomainName" /></td>
                                </tr>
                                <tr>
                                    <td><asp:Localize runat="server" ID="locState" meta:resourcekey="locState" /></td>
                                    <td><asp:Localize runat="server" ID="locEnabled" meta:resourcekey="locEnabled" /><asp:Localize runat="server" ID="locDisabled" meta:resourcekey="locDisabled" />&nbsp;
                                        <CPCC:StyleButton id="btnActive" CssClass="btn btn-success" runat="server" OnClick="btnActive_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnActivate"/> </CPCC:StyleButton>&nbsp;
                                        <CPCC:StyleButton id="btnDeactivate" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnDeactivate_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeactivate"/> </CPCC:StyleButton>

                                    </td>
                                </tr>

                                <tr>
                                    <td class="FormLabel150"><asp:Localize runat="server" meta:resourcekey="locLicenseType" Text="License Type:" /></td>
                                    <td>
                                        <asp:DropDownList ID="ddlLicenseType" runat="server" CssClass="NormalTextBox" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                            </table>
                            <br />
                        </div>
                        
                        <div>
                            <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                                Width="100%"  CssSelectorClass="NormalGridView" 
                                AllowPaging="False" AllowSorting="False" DataKeyNames="RoleID" >
                                <Columns>
                                    <asp:TemplateField >
                                        <ItemStyle  HorizontalAlign="Center" ></ItemStyle>
                                        <ItemTemplate >
                                            <asp:CheckBox runat="server" ID="cbSelected" Checked=<%# Eval("IsCurrentUserRole") %> />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="gvRole" DataField="RoleName" 
                                        ItemStyle-Width="100%" />
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                   </div>
                </div>
  <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-warning" runat="server" OnClick="btnUpdate_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>&nbsp;
        <CPCC:StyleButton id="btnSaveExit" CssClass="btn btn-success" runat="server" OnClick="btnSaveExit_Click"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveExit"/> </CPCC:StyleButton>
  </div>