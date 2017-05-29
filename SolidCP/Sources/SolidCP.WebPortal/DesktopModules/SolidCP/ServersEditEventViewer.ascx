<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditEventViewer.ascx.cs" Inherits="SolidCP.Portal.ServersEditEventViewer" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server">
</uc1:ServerHeaderControl>

<asp:UpdatePanel runat="server" ID="updatePanelUsers">
    <ContentTemplate>
        <div class="panel-body form-inline">
            <div class="input-group">
                <asp:DropDownList ID="ddlLogNames" runat="server" CssClass="form-control" onSelectedIndexChanged="LogNameSelected" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="input-group pull-right">
                <CPCC:StyleButton id="btnClearLog" CssClass="btn btn-danger" runat="server" meta:resourcekey="btnClearLog" OnClick="btnClearLog_Click">
                    <i class="fa fa-trash-o">&nbsp;</i>&nbsp;
                </CPCC:StyleButton>
            </div>
        </div>
   	    <scp:SimpleMessageBox id="messageBox" runat="server" />
        <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvEntries" AllowPaging="true" DataSourceID="odsLogEntries"
            CssSelectorClass="NormalGridView" EnableViewState="false" ondatabound="gvEntries_DataBound">
            <Columns>
                <asp:TemplateField HeaderText="gvEntriesType">
                    <ItemTemplate>
                        <asp:Image ID="imgType" runat="server"
                            ImageUrl='<%# PortalUtils.GetThemedImage(Eval("EntryType").ToString() + "_icon_small.gif") %>'
                            ImageAlign="AbsMiddle" />
                        <%# GetLocalizedString("EventType." + Eval("EntryType").ToString()) %>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="Created" HeaderText="gvEntriesCreated" >
                </asp:BoundField>
                <asp:BoundField DataField="Source" HeaderText="gvEntriesSource" >
                </asp:BoundField>
                <asp:BoundField DataField="Category" HeaderText="gvEntriesCategory" >
                </asp:BoundField>
                <asp:BoundField DataField="EventID" HeaderText="gvEntriesEvent" >
                </asp:BoundField>
                <asp:BoundField DataField="UserName" HeaderText="gvEntriesUserName" >
                </asp:BoundField>
                <asp:BoundField DataField="MachineName" HeaderText="gvEntriesMachineName" >
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a data-toggle="collapse" href="#infoRow-<%# Container.DataItemIndex %>" class="accordion-toggle collapsed"><i class="fa fa-plus-circle"></i></a>
                        <tr>
                            <td colspan="8" class="hiddenRow">
                                <div class="accordion-body collapse" id="infoRow-<%# Container.DataItemIndex %>">
                                    <pre><%# Eval("Message") %></pre>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsLogEntries" runat="server" SelectMethod="GetEventLogEntriesPaged" SelectCountMethod="GetEventLogEntriesPagedCount" TypeName="SolidCP.Portal.ServersHelper" OnSelected="odsLogEntries_Selected" EnablePaging="true">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlLogNames" Name="logName" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
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
