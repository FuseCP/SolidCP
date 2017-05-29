<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditSystemProcesses.ascx.cs" Inherits="SolidCP.Portal.ServersEditSystemProcesses" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server">
</uc1:ServerHeaderControl>

<div class="FormButtonsBar">
    <div class="Left" style="padding: 5px;">
        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
    </div>
    <div class="Right">
        <asp:UpdateProgress ID="updatePanelProgress" runat="server"
            AssociatedUpdatePanelID="ItemsUpdatePanel" DynamicLayout="false">
            <ProgressTemplate>
                <asp:Image ID="imgSep" runat="server" SkinID="MediumAjaxIndicator" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</div>

<asp:UpdatePanel ID="ItemsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:Timer runat="server" Interval="10000" ID="itemsTimer" />
        <asp:GridView ID="gvProcesses" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvProcesses" EnableViewState="false"
            CssSelectorClass="NormalGridView"
            DataKeyNames="Pid" OnRowDeleting="gvProcesses_RowDeleting">
            <Columns>
                <asp:BoundField DataField="Pid" HeaderText="gvProcessesPID" />
                <asp:BoundField DataField="Name" HeaderText="gvProcessesImageName" ItemStyle-Wrap="false" ItemStyle-Width="100%"/>
                <asp:BoundField DataField="Username" HeaderText="gvProcessesUserName" ItemStyle-Wrap="false" />
                <asp:TemplateField HeaderText="gvProcessesMemoryUsage" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <%# PanelFormatter.GetDisplaySizeInBytes((long)Eval("MemUsage")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="cmdTerminate" runat="server" SkinID="DeleteSmall" meta:resourcekey="cmdTerminate"
                            CommandName="delete" OnClientClick="return confirm('Terminate?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>