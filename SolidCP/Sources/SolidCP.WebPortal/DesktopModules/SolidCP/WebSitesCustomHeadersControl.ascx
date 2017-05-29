<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesCustomHeadersControl.ascx.cs" Inherits="SolidCP.Portal.WebSitesCustomHeadersControl" %>
<div style="width:400px">
    <div class="FormButtonsBar">
        <asp:Button id="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add Custom Header" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnAdd_Click"/>
    </div>
    <asp:GridView id="gvCustomHeaders" Runat="server" AutoGenerateColumns="False"
        CssSelectorClass="NormalGridView"
	    OnRowCommand="gvCustomHeaders_RowCommand" OnRowDataBound="gvCustomHeaders_RowDataBound"
	    EmptyDataText="gvCustomHeaders">
	    <columns>
		    <asp:TemplateField HeaderText="gvCustomHeadersName">
			    <itemtemplate>
				    <asp:TextBox id="txtName" Runat="server" Width="150" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Key") %>'>
				    </asp:TextBox>
			    </itemtemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="gvCustomHeadersValue">
			    <itemtemplate>
				    <asp:TextBox id="txtValue" Runat="server" Width="150" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Value") %>'>
				    </asp:TextBox>
			    </itemtemplate>
		    </asp:TemplateField>
	        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
		        <itemtemplate>
			        <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='delete_item' CausesValidation="false"> 
                        &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                    </CPCC:StyleButton>
		        </itemtemplate>
	        </asp:TemplateField>
	    </columns>
    </asp:GridView>
</div>