<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsServiceLevels.ascx.cs" Inherits="SolidCP.Portal.SettingsServiceLevels" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Import Namespace="SolidCP.Portal" %>

    <scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <scp:SimpleMessageBox id="messageBox" runat="server" />
	<asp:GridView id="gvServiceLevels" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
		Width="100%" EmptyDataText="gvServiceLevels" CssSelectorClass="NormalGridView" OnRowCommand="gvServiceLevels_RowCommand">
		<Columns>
            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="EditItem" AlternateText="Edit record" CommandArgument='<%# Eval("LevelId") %>' ></asp:ImageButton>
                </ItemTemplate>
             </asp:TemplateField>
			<asp:TemplateField HeaderText="Service Level">
				<ItemStyle Width="30%"></ItemStyle>
				<ItemTemplate>
					<asp:Label id="lnkServiceLevel" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Eval("LevelName"))%></asp:Label>
                 </ItemTemplate>
			</asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
				<ItemStyle Width="60%"></ItemStyle>
				<ItemTemplate>
					<asp:Label id="lnkServiceLevelDescription" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Eval("LevelDescription"))%></asp:Label>
                 </ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<CPCC:StyleButton id="imgDelMailboxPlan" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("LevelId") %>' OnClientClick="return confirm('Are you sure you want to delete selected service level?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />

	<scp:CollapsiblePanel id="secServiceLevel" runat="server"
        TargetControlID="ServiceLevel" meta:resourcekey="secServiceLevel" Text="Service Level">
    </scp:CollapsiblePanel>
    <asp:Panel ID="ServiceLevel" runat="server" Height="0" style="overflow:hidden;">
		<table>
            <tr>
                <td class="FormLabel200" align="right">
                    <asp:Label ID="lblServiceLevelName" runat="server" meta:resourcekey="lblServiceLevelName" Text="Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtServiceLevelName" runat="server" Width="720px" CssClass="form-control" MaxLength="255"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireServiceLevelName" runat="server" meta:resourcekey="valRequireServiceLevelName" ControlToValidate="txtServiceLevelName"
					ErrorMessage="Enter service level name" ValidationGroup="CreateServiceLevel" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="FormLabel200" align="right">
                    <asp:Label ID="lblServiceLevelDescr" runat="server" meta:resourcekey="lblServiceLevelDescr" Text="Description:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtServiceLevelDescr" runat="server" Rows="7" TextMode="MultiLine" Width="720px" CssClass="form-control" Wrap="False" MaxLength="511"></asp:TextBox>
                </td>
            </tr>
		</table>
	</asp:Panel>
    <br />

    <table>
        <tr>
            <td>
                <div class="FormButtonsBarClean">
                    <CPCC:StyleButton id="btnAddServiceLevel" CssClass="btn btn-success" runat="server" OnClick="btnAddServiceLevel_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddServiceLevelText"/> </CPCC:StyleButton>
                </div>
            </td>
            <td>
                <div class="FormButtonsBarClean">
                        <CPCC:StyleButton id="btnUpdateServiceLevel" CssClass="btn btn-success" runat="server" OnClick="btnUpdateServiceLevel_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateServiceLevelText"/> </CPCC:StyleButton>
            </td>
        </tr>
    </table>
    <br />

    <asp:TextBox ID="txtStatus" runat="server" CssClass="TextBox400" MaxLength="128" ReadOnly="true"></asp:TextBox>
    