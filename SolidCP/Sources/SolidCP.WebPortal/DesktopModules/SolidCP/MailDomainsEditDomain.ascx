<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailDomainsEditDomain.ascx.cs" Inherits="SolidCP.Portal.MailDomainsEditDomain" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this Domain?")) return false; else ShowProgressDialog('Deleting Domain...');
}
</script>

<div class="panel-body form-horizontal">
    <div class="Huge">
        <asp:Literal ID="litDomainName" runat="server"></asp:Literal>
    </div>
    <div class="panel-body form-horizontal" style="width: 400px;">
        <div class="FormButtonsBar">
            <asp:Button ID="btnAddPointer" runat="server" meta:resourcekey="btnAddPointer" Text="Add Pointer" CssClass="Button2" OnClick="btnAddPointer_Click" />
        </div>
        <asp:GridView id="gvPointers" Runat="server" EnableViewState="True" AutoGenerateColumns="false"
            ShowHeader="false"
            CssSelectorClass="NormalGridView"
            EmptyDataText="gvPointers" DataKeyNames="DomainID" OnRowDeleting="gvPointers_RowDeleting">
            <Columns>
	            <asp:TemplateField HeaderText="gvPointersName">
		            <ItemStyle Wrap="false" Width="100%"></ItemStyle>
		            <ItemTemplate>
                        <%# Eval("DomainName") %>
                        <CPCC:StyleButton id="cmdDeletePointer" CssClass="btn btn-danger" runat="server" CommandName='delete' CommandArgument='<%# Eval("DomainId") %>' OnClientClick="return confirm('Remove pointer?');" Visible='<%# !(bool)Eval("IsInstantAlias") %>'> 
                            &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                        </CPCC:StyleButton>
		            </ItemTemplate>
	            </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="panel-body form-horizontal">
        <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
    </div>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirmation();"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating Domain Settings...');"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
</div>