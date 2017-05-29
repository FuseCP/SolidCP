<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesCustomErrorsControl.ascx.cs" Inherits="SolidCP.Portal.WebSitesCustomErrorsControl" %>


<div style="padding: 20; margin-bottom: 10px;">
<table cellpadding="4">
  <tr id="rowAspNet" runat="server">
    <td class="SubHead">
        <asp:Label ID="lblErrorMode" runat="server" meta:resourcekey="lblErrorMode"></asp:Label>:
    </td>
    <td class="Normal">
        <asp:DropDownList ID="ddlErrorMode" runat="server" CssClass="form-control"></asp:DropDownList>
    </td>
  </tr>
</table>
</div>

<div class="FormButtonsBar">
    <asp:Button id="btnAdd" runat="server" meta:resourcekey="btnAddError" Text="Add Custom Error" CssClass="btn btn-primary" OnClick="btnAdd_Click" CausesValidation="False"></asp:Button>
</div>
<asp:GridView id="gvErrorPages" Runat="server" AutoGenerateColumns="False"
    CssSelectorClass="NormalGridView"
    OnRowCommand="gvErrorPages_RowCommand" OnRowDataBound="gvErrorPages_RowDataBound"
    EmptyDataText="gvErrorPages">
    <columns>
	    <asp:TemplateField HeaderText="gvErrorPagesCode" ItemStyle-Width="60px">
		    <itemtemplate>
			    <asp:TextBox id="txtErrorCode" Runat="server" Width="55px" CssClass="form-control" Text='<%# Eval("ErrorCode") %>'></asp:TextBox>
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="5px">
            <itemtemplate>
			    . 
            </itemtemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="60px">
            <itemtemplate>
            <asp:TextBox id="txtErrorSubcode" Runat="server" Width="55px" CssClass="form-control" Text='<%# GetSubCode(Eval("ErrorSubcode")) %>'>
			    </asp:TextBox>
		    </itemtemplate>
	    </asp:TemplateField>
	    <asp:TemplateField HeaderText="gvErrorPagesHandlerType" ItemStyle-Width="100px">
		    <itemtemplate>
			    <asp:dropdownlist id="ddlHandlerType" Width="100px" Runat="server"
			        CssClass="form-control">
			    </asp:dropdownlist>
		    </itemtemplate>
	    </asp:TemplateField>
	    <asp:TemplateField HeaderText="gvErrorPagesErrorContent" ItemStyle-Width="100%">
		    <itemtemplate>
			    <asp:TextBox id="txtErrorContent" Runat="server" Width="100%" CssClass="form-control" Text='<%# Eval("ErrorContent") %>'>
			    </asp:TextBox>
		    </itemtemplate>
	    </asp:TemplateField>
	    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="65px">
		    <itemtemplate>
			    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='delete_item' CausesValidation="false"> 
                    &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                </CPCC:StyleButton>
		    </itemtemplate>
	    </asp:TemplateField>
    </columns>
</asp:GridView>