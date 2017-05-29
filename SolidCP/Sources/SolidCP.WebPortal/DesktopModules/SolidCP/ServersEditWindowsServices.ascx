<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditWindowsServices.ascx.cs" Inherits="SolidCP.Portal.ServersEditWindowsServices" %>
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
        <asp:GridView ID="gvServices" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvServices"
            CssSelectorClass="NormalGridView" EnableViewState="false"
            OnRowCommand="gvServices_RowCommand" OnRowDataBound="gvServices_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="gvServicesId" />
                <asp:BoundField DataField="Name" HeaderText="gvServicesName" ItemStyle-Wrap="false" ItemStyle-Width="100%"/>
                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="gvServicesStatus">
                    <ItemTemplate>
                        <%# GetLocalizedString("Status." + Eval("Status").ToString()) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Wrap="false">
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:ImageButton ID="cmdStart" runat="server" meta:resourcekey="cmdStart" SkinID="StartSmall"
                            CommandName="Running" CommandArgument='<%# Eval("Id") %>' />
                        <asp:ImageButton ID="cmdPause" runat="server" meta:resourcekey="cmdPause" SkinID="PauseSmall"
                            CommandName="Paused" CommandArgument='<%# Eval("Id") %>' />
                        <asp:ImageButton ID="cmdContinue" runat="server" meta:resourcekey="cmdContinue" SkinID="ContinueSmall"
                            CommandName="ContinuePending" CommandArgument='<%# Eval("Id") %>' />
                        <asp:ImageButton ID="cmdStop" runat="server" meta:resourcekey="cmdStop" SkinID="StopSmall"
                            CommandName="Stopped" CommandArgument='<%# Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>