<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSUserSessions.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSUserSessions" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <asp:Image ID="imgEditRDSCollection" SkinID="EnterpriseRDSCollections48" runat="server" />
    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit RDS Collection"></asp:Localize>
    -
    <asp:Literal ID="litCollectionName" runat="server" Text="" />
</div>
<div class="panel-body form-horizontal">
    <scp:CollectionTabs id="tabs" runat="server" SelectedTab="rds_collection_user_sessions" />
    <div class="panel panel-default tab-content">
        <div class="panel-body form-horizontal">
            <scp:SimpleMessageBox id="messageBox" runat="server" />  
            <asp:UpdatePanel ID="RDAppsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="FormButtonsBarCleanRight">
                        <div class="FormButtonsBarClean">
                            <CPCC:StyleButton id="btnRefresh" CssClass="btn btn-warning" runat="server" OnClick="btnRefresh_Click" OnClientClick="ShowProgressDialog('Loading'); return true;">
                                <i class="fa fa-refresh">&nbsp;</i>&nbsp;
                                <asp:Localize runat="server" meta:resourcekey="btnRefreshText"/>
                            </CPCC:StyleButton>
                            &nbsp;
                            <CPCC:StyleButton id="btnRecentMessages" CssClass="btn btn-primary" runat="server" OnClick="btnRecentMessages_Click" OnClientClick="ShowProgressDialog('Loading'); return true;">
                                <i class="fa fa-comments-o">&nbsp;</i>&nbsp;
                                <asp:Localize runat="server" meta:resourcekey="btnRecentMessagesText"/>
                            </CPCC:StyleButton>
                            &nbsp;
                            <CPCC:StyleButton id="btnSendMessage" CssClass="btn btn-success" runat="server" OnClick="btnSendMessage_Click">
                                <i class="fa fa-commenting-o">&nbsp;</i>&nbsp;
                                <asp:Localize runat="server" meta:resourcekey="cmdSendMessageText"/>
                            </CPCC:StyleButton>
                        </div>
                    </div>
                    <scp:CollapsiblePanel id="secRdsUserSessions" runat="server" TargetControlID="panelRdsUserSessions" meta:resourcekey="secRdsUserSessions" Text=""></scp:CollapsiblePanel>
                    <asp:Panel runat="server" ID="panelRdsUserSessions">
                        <div style="padding: 10px;">
                            <asp:GridView ID="gvRDSUserSessions" runat="server" AutoGenerateColumns="False" EnableViewState="true" Width="100%" EmptyDataText="No Sessions available" CssSelectorClass="NormalGridView" OnRowCommand="gvRDSCollections_RowCommand" AllowPaging="True" AllowSorting="True">
                                <Columns>
                                    <asp:TemplateField meta:resourcekey="gvUserName" HeaderText="gvUserName">
                                        <ItemStyle Width="20%" Wrap="false"/>
                                        <ItemTemplate>
                                            <asp:Image ID="vipImage" runat="server" ImageUrl='<%# GetAccountImage(Convert.ToBoolean(Eval("IsVip"))) %>' ImageAlign="AbsMiddle"/>
                                            <asp:Literal ID="litUserName" runat="server" Text='<%# Eval("UserName") %>'/>
                                            <asp:HiddenField ID="hfUnifiedSessionId" runat="server"  Value='<%# Eval("UnifiedSessionId") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField meta:resourcekey="gvHostServer" HeaderText="gvHostServer">
                                        <ItemStyle Width="20%" Wrap="false"/>
                                        <ItemTemplate>
                                            <asp:Literal ID="litHostServer" runat="server" Text='<%# Eval("HostServer") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField meta:resourcekey="gvSessionState" HeaderText="gvSessionState">
                                        <ItemStyle Width="20%" Wrap="false"/>
                                        <ItemTemplate>
                                            <asp:Literal ID="litSessionState" runat="server" Text='<%# Eval("SessionState") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <CPCC:StyleButton ID="lnkViewSession" runat="server" CssClass="btn btn-primary" Text="View" CommandName="View" CommandArgument='<%# Eval("UnifiedSessionId") + ";" + Eval("HostServer") %>' meta:resourcekey="cmdViewSession" OnClientClick="ShowProgressDialog('Loading'); return true;">
                                            </CPCC:StyleButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <CPCC:StyleButton ID="lnkControlSession" runat="server" CssClass="btn btn-primary" Text="Control" CommandName="Control" CommandArgument='<%# Eval("UnifiedSessionId") + ";" + Eval("HostServer") %>' meta:resourcekey="cmdControlSession" OnClientClick="ShowProgressDialog('Loading'); return true;">
                                            </CPCC:StyleButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <CPCC:StyleButton ID="lnkLogOff" runat="server" CssClass="btn btn-danger" Text="Log Off" CommandName="LogOff" CommandArgument='<%# Eval("UnifiedSessionId") + ";" + Eval("HostServer") %>' meta:resourcekey="cmdLogOff" OnClientClick="return confirm('Are you sure you want to log off selected user?')">
                                            </CPCC:StyleButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <CPCC:StyleButton ID="lnkSendMessage" runat="server" CssClass="btn btn-primary" Text="Send Message" CommandName="SendMessage" CommandArgument='<%# Eval("HostServer") + ":" + Eval("UserName") + ":" + Eval("UnifiedSessionId") %>' meta:resourcekey="cmdSendMessage">
                                            </CPCC:StyleButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <div class="text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="SaveRDSCollection" OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
                    </div>
                    <asp:Panel ID="MessagesHistoryPanel" runat="server" style="display:none">
                        <div class="widget">
                            <div class="widget-header clearfix">
                                <h3>
                                    <i class="fa-envelope-o"></i>
                                    <asp:Localize ID="headerMessagesHistory" runat="server" meta:resourcekey="headerMessagesHistory">
                                    </asp:Localize>
                                </h3>
                            </div>
                            <div class="widget-content Popup">
                                <asp:UpdatePanel ID="MessagesHistoryUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <div class="Popup-Scroll">
                                            <asp:GridView ID="gvMessagesHistory" runat="server" meta:resourcekey="gvMessagesHistory" AutoGenerateColumns="False" Width="100%" CssSelectorClass="NormalGridView" DataKeyNames="Id">
                                                <Columns>
                                                    <asp:TemplateField meta:resourcekey="gvMessageText">
                                                        <ItemStyle Width="70%"/>
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litMessage" runat="server" Text='<%# Eval("MessageText") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField meta:resourcekey="gvUser" HeaderText="gvUser">
                                                        <ItemStyle Width="15%" Wrap="false"/>
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litUserName" runat="server" Text='<%# Eval("UserName") %>'/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToShortDateString() %>'/>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="popup-buttons text-right">
                                <CPCC:StyleButton id="btnCancelMessagesHistory" CssClass="btn btn-warning" runat="server" CausesValidation="False">
                                    <i class="fa fa-times">&nbsp;</i>&nbsp;
                                    <asp:Localize runat="server" meta:resourcekey="btnCancelText"/>
                                </CPCC:StyleButton>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="EnterMessagePanel" runat="server" style="display:none">
                        <div class="widget">
                            <div class="widget-header clearfix">
                                <h3>
                                    <i class="fa fa-pencil"></i>
                                    <asp:Localize ID="headerEnterMessage" runat="server" meta:resourcekey="headerEnterMessage"></asp:Localize>
                                </h3>
                            </div>
                            <div class="widget-content Popup">
                                <asp:TextBox id="txtMessage" TextMode="multiline" Columns="70" Rows="15" runat="server" CssClass="form-control" />
                            </div>
                            <div class="popup-buttons text-right">
                                <CPCC:StyleButton id="btnAddMessage" CssClass="btn btn-success" runat="server" OnClick="btnAddMessage_Click" CausesValidation="false">
                                    <i class="fa fa-check">&nbsp;</i>&nbsp;
                                    <asp:Localize runat="server" meta:resourcekey="btnAddText"/>
                                </CPCC:StyleButton>
                                &nbsp;
                                <CPCC:StyleButton id="btnCancelEnterMessage" CssClass="btn btn-warning" runat="server" CausesValidation="False">
                                    <i class="fa fa-times">&nbsp;</i>&nbsp;
                                    <asp:Localize runat="server" meta:resourcekey="btnCancelText"/>
                                </CPCC:StyleButton>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Button ID="btnEnterMessageFake" runat="server" style="display:none;" />
                    <ajaxToolkit:ModalPopupExtender ID="EnterMessageModal" runat="server" TargetControlID="btnEnterMessageFake" PopupControlID="EnterMessagePanel" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelEnterMessage"/>
                    <asp:Button ID="btnMessagesHistoryFake" runat="server" style="display:none;" />
                    <ajaxToolkit:ModalPopupExtender ID="MessagesHistoryModal" runat="server" TargetControlID="btnMessagesHistoryFake" PopupControlID="MessagesHistoryPanel" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelMessagesHistory"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>