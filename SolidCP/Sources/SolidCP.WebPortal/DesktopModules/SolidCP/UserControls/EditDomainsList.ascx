<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDomainsList.ascx.cs" Inherits="SolidCP.Portal.UserControls.EditDomainsList" %>
<div style="width:400px;">
	<div class="FormButtonsBar">
		<asp:Button id="btnAddDomain" runat="server" meta:resourcekey="btnAddDomain" Text="Add"
			CssClass="Button2" CausesValidation="false" OnClick="BtnAddClick"/>
	</div>

	<asp:GridView id="gvDomains" Runat="server" AutoGenerateColumns="False"
		CssSelectorClass="NormalGridView"
		OnRowCommand="ListRowCommand" OnRowDataBound="ListRowDataBound"
		EmptyDataText="gvDomains" ShowHeader="False">
		<columns>
			<asp:TemplateField HeaderText="gvDomainsLabel" ItemStyle-Wrap="false">
				<itemtemplate>
					<asp:Label id="lblDomainName" Runat="server"></asp:Label>
				</itemtemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="gvDomainsName" ItemStyle-Width="100%">
				<itemtemplate>
					<asp:TextBox id="txtDomainName" Runat="server" Width="200px" CssClass="form-control">
					</asp:TextBox>
					<asp:RegularExpressionValidator id="valCorrectDomainName" runat="server" CssClass="NormalBold"
						ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,15}$"
						ErrorMessage="&nbsp;*" ControlToValidate="txtDomainName" Display="Dynamic"></asp:RegularExpressionValidator>
				</itemtemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<itemtemplate>
					<CPCC:StyleButton id="cmdDeleteDomain" CssClass="btn btn-danger" runat="server" CommandName='delete_item' CausesValidation="false"> 
                        &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                    </CPCC:StyleButton>
				</itemtemplate>
				<ItemStyle HorizontalAlign="Center" />
			</asp:TemplateField>
		</columns>
	</asp:GridView>

</div>