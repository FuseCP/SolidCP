<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditVirtualApps.ascx.cs" Inherits="SolidCP.Portal.WebSitesEditVirtualApps" %>
<%@ Register Src="WebSitesExtensionsControl.ascx" TagName="WebSitesExtensionsControl" TagPrefix="uc6" %>
<%@ Register Src="WebSitesCustomErrorsControl.ascx" TagName="WebSitesCustomErrorsControl" TagPrefix="uc4" %>
<%@ Register Src="WebSitesMimeTypesControl.ascx" TagName="WebSitesMimeTypesControl" TagPrefix="uc5" %>
<%@ Register Src="WebSitesHomeFolderControl.ascx" TagName="WebSitesHomeFolderControl" TagPrefix="uc1" %>
<%@ Register Src="WebSitesCustomHeadersControl.ascx" TagName="WebSitesCustomHeadersControl" TagPrefix="uc6" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<div class="panel-body form-horizontal">
    <table width="100%">
        <tr>
            <td width="100%" valign="top">
                <table>
                    <tr>
                        <td class="Big">
                            <asp:HyperLink ID="lnkSiteName" runat="server" NavigateUrl="#" Target="_blank">domain.com/vapp</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div class="widget">
                    <div class="widget-header clearfix">
                        <ul class="nav nav-tabs">
                            <asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="dlTabs_SelectedIndexChanged" RepeatLayout="Flow">
                                <ItemStyle Wrap="False" />
                                <ItemTemplate>
                                    <CPCC:StyleButton ID="cmdSelectTab" runat="server" CommandName="select" CssClass="Tab">
                                        <%# Eval("Name") %>
                                    </CPCC:StyleButton>
                                </ItemTemplate>
                                <SelectedItemStyle Wrap="False" />
                                <SelectedItemTemplate>
                                    <CPCC:StyleButton ID="cmdSelectTab" runat="server" CommandName="select" CssClass="ActiveTab">
                                        <%# Eval("Name") %>
                                    </CPCC:StyleButton>
                                </SelectedItemTemplate>
                            </asp:DataList>
                        </ul>
                        <script type="text/javascript">
                            $(document).ready(function () {
                                $('.nav-tabs li').unwrap();
                                $('.nav-tabs li').unwrap();
                            });
                        </script>
                    </div>
                    <div class="widget-content tab-content">
                        <div class="panel-body form-horizontal">
                            <asp:MultiView ID="tabs" runat="server" ActiveViewIndex="0">
                                <asp:View ID="tabHomeFolder" runat="server">
                                    <uc1:WebSitesHomeFolderControl ID="webSitesHomeFolderControl" runat="server" IsAppVirtualDirectory="true" />
                                </asp:View>

                                <asp:View ID="tabExtensions" runat="server">
                                    <uc6:WebSitesExtensionsControl ID="webSitesExtensionsControl" runat="server" IsAppVirtualDirectory="true" />
                                </asp:View>

                                <asp:View ID="tabErrors" runat="server">
                                    <uc4:WebSitesCustomErrorsControl ID="webSitesCustomErrorsControl" runat="server" />
                                </asp:View>

                                <asp:View ID="tabHeaders" runat="server">
                                    <uc6:WebSitesCustomHeadersControl ID="webSitesCustomHeadersControl" runat="server" />
                                </asp:View>

                                <asp:View ID="tabMimes" runat="server">
                                    <uc5:WebSitesMimeTypesControl ID="webSitesMimeTypesControl" runat="server" />
                                </asp:View>

                            </asp:MultiView>
                        </div>
                        <div class="panel-footer text-right">
                            <CPCC:StyleButton ID="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click" CausesValidation="false" OnClientClick="return confirm('Delete this virtual Application?');"><i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText" />
                            </CPCC:StyleButton>
                            &nbsp;
                    <CPCC:StyleButton ID="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"><i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel" />
                    </CPCC:StyleButton>
                            &nbsp;
                    <CPCC:StyleButton ID="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Please Wait! Updating virtual Application...');"><i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText" />
                    </CPCC:StyleButton>
                        </div>
                    </div>
            </td>
        </tr>
    </table>
</div>
