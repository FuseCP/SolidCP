<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceDeleteSpace.ascx.cs" Inherits="SolidCP.Portal.SpaceDeleteSpace" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
    <table>
	    <tr id="rowPackageItems" runat="server">
		    <td class="Normal">
			    <asp:Label ID="lblServiceItems" runat="server" meta:resourcekey="lblServiceItems" Text="The package contains the following service items:"></asp:Label>
			    <br/>
			    <br/>
                <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="gvItems" CssSelectorClass="NormalGridView">
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="gvItemsName" ></asp:BoundField>
                        <asp:TemplateField HeaderText="gvItemsType">
                            <ItemTemplate>
                                <%# GetSharedLocalizedString("ServiceItemType." + (string)Eval("DisplayName")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
			    <br/>
		    </td>
	    </tr>
	    <tr id="rowPackagePackages" runat="server">
		    <td class="Normal">
			    <asp:Label ID="lblChildPackages" runat="server" meta:resourcekey="lblChildPackages" Text="The package contains the following child packages:"></asp:Label>
			    <br/>
			    <br/>
                <asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="gvPackages" CssSelectorClass="NormalGridView">
                    <Columns>
                        <asp:BoundField DataField="PackageName" HeaderText="gvPackagesPackageName" ></asp:BoundField>
                    </Columns>
                </asp:GridView>
			    <br/>
		    </td>
	    </tr>
	    <tr>
		    <td class="Normal">
			    <asp:Label ID="lblDeleteWarning" runat="server" meta:resourcekey="lblDeleteWarning" Text="Deleting this package also deletes all its child packages and all service items such as web sites, 
			    databases, user accounts, etc."></asp:Label>
			    <br/>
			    <br/>
		    </td>
	    </tr>
	    <tr>
		    <td class="Normal">
			    <asp:CheckBox ID="chkConfirm" Runat="server" meta:resourcekey="chkConfirm" Text="Yes, I understand it and want to delete this package"></asp:CheckBox>
			    <br/>
			    <br/>
		    </td>
	    </tr>
    </table>
</div>
<div class="panel-footer text-right">
	<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="ShowProgressDialog('Deleting...');"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>
</div>