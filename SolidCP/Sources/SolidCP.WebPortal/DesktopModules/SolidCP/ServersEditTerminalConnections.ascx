<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditTerminalConnections.ascx.cs" Inherits="SolidCP.Portal.ServersEditTerminalConnections" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server">
</uc1:ServerHeaderControl>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<asp:UpdatePanel ID="ItemsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:Timer runat="server" Interval="10000" ID="itemsTimer" />
        <asp:GridView ID="gvSessions" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvSessions"
            CssSelectorClass="NormalGridView" EnableViewState="false"
            DataKeyNames="SessionID" OnRowDeleting="gvSessions_RowDeleting">
            <Columns>
                <asp:BoundField DataField="SessionID" HeaderText="gvSessionsSessionID" />
                <asp:BoundField DataField="Username" HeaderText="gvSessionsUserName" />
                <asp:BoundField DataField="Status" HeaderText="gvSessionsStatus" />
                <asp:TemplateField HeaderText="gvSessionsReset">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="cmdReset" runat="server" SkinID="StopSmall" meta:resourcekey="cmdReset"
                            CommandName="delete" OnClientClick="return confirm('Reset session?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequest);
    });


    function EndRequest(sender, args) {
        CloseProgressDialog();
    }
</script>