<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsExchangeRetentionPolicyTag.ascx.cs" Inherits="SolidCP.Portal.SettingsExchangeRetentionPolicyTag" %>
<%@ Register Src="ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="ExchangeServer/UserControls/DaysBox.ascx" TagName="DaysBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/QuotaEditor.ascx" TagName="QuotaEditor" TagPrefix="uc1" %>
<%@ Import Namespace="SolidCP.Portal" %>

    <scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <scp:SimpleMessageBox id="messageBox" runat="server" />
	<asp:GridView id="gvPolicy" runat="server"  EnableViewState="true" AutoGenerateColumns="false"
		Width="100%" EmptyDataText="gvPolicy" CssSelectorClass="NormalGridView" OnRowCommand="gvPolicy_RowCommand" >
		<Columns>
            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="EditItem" AlternateText="Edit record" CommandArgument='<%# Eval("TagId") %>' ></asp:ImageButton>
                </ItemTemplate>
             </asp:TemplateField>
			<asp:TemplateField HeaderText="Tag">
				<ItemStyle Width="70%"></ItemStyle>
				<ItemTemplate>
					<asp:Label id="lnkDisplayPolicy" runat="server" EnableViewState="true" ><%# PortalAntiXSS.Encode((string)Eval("TagName"))%></asp:Label>
                 </ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<CPCC:StyleButton id="imgDelPolicy" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("TagId") %>' OnClientClick="return confirm('Are you sure you want to delete selected policy tag?')"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
    	<scp:CollapsiblePanel id="secPolicy" runat="server"
            TargetControlID="Policy" meta:resourcekey="secPolicy" Text="Policy">
        </scp:CollapsiblePanel>
        <asp:Panel ID="Policy" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td class="FormLabel200" align="right">
									
					</td>
					<td>
						<asp:TextBox ID="txtPolicy" runat="server"  CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequirePolicy" runat="server" meta:resourcekey="valRequirePolicy" ControlToValidate="txtPolicy"
						ErrorMessage="Enter policy tag name" ValidationGroup="CreatePolicy" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>

		<scp:CollapsiblePanel id="secPolicyFeatures" runat="server"
            TargetControlID="PolicyFeatures" meta:resourcekey="secPolicyFeatures" Text="Policy Tag Features">
        </scp:CollapsiblePanel>
        <asp:Panel ID="PolicyFeatures" runat="server" Height="0" style="overflow:hidden;">
			<table>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locType" runat="server" meta:resourcekey="locType" Text="Type :"></asp:Localize></td>
					<td>
                        <asp:DropDownList ID="ddTagType"  CssClass="form-control" runat="server"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locAgeLimitForRetention" runat="server" meta:resourcekey="locAgeLimitForRetention" Text="Age limit for retention :"></asp:Localize></td>
					<td>
                        <div class="Right">
                            <uc1:QuotaEditor id="ageLimitForRetention" runat="server"
                                QuotaTypeID="2"
                                QuotaValue="0"
                                ParentQuotaValue="-1">
                            </uc1:QuotaEditor>
                        </div>
					</td>
				</tr>
				<tr>
					<td class="FormLabel200" align="right"><asp:Localize ID="locRetentionAction" runat="server" meta:resourcekey="locRetentionAction" Text="Retention action :"></asp:Localize></td>
					<td>
                        <asp:DropDownList ID="ddRetentionAction"  CssClass="form-control" runat="server"></asp:DropDownList>
					</td>
				</tr>
			</table>
			<br />
		</asp:Panel>


    <table>
        <tr>
            <td>
                <div class="FormButtonsBarClean">
                    <CPCC:StyleButton id="btnUpdatePolicy" CssClass="btn btn-warning" runat="server" OnClick="btnUpdatePolicy_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdatePolicyText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnAddPolicy" CssClass="btn btn-success" runat="server" OnClick="btnAddPolicy_Click"> <i class="fa fa-file-text-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddPolicyText"/> </CPCC:StyleButton>
                </div>
            </td>
        </tr>
    </table>

    <br />

    <asp:TextBox ID="txtStatus" runat="server"  CssClass="form-control" MaxLength="128" ReadOnly="true"></asp:TextBox>
    