<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OCS_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.OCS_Settings" %>
<table>
    <tr>
        <td class="Normal" width="200" >
            <asp:Localize runat="server" ID="locServerName" meta:resourcekey="locServerName"/>
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtServerName"  CssClass="form-control" Width="200px"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Localize runat="server" ID="locEdgeServices" meta:resourcekey="locEdgeServices"/>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlEdgeServers" />
            <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
            <br />
            <asp:GridView ID="gvEdgeServices" runat="server" AutoGenerateColumns="False"  
                EmptyDataText="gvRecords" CssSelectorClass="NormalGridView" 
                onrowcommand="gvEdgeServices_RowCommand"  meta:resourcekey="gvEdgeServices">
                <Columns>                                       
                    <asp:BoundField DataField="ServiceName" meta:resourcekey="locServerNameColumn" HeaderText="gvRecordsData" ItemStyle-Width="100%" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName='RemoveServer' CommandArgument='<%#Eval("ServiceId") %>' OnClientClick="return confirm('Delete?');"> 
                                &nbsp;<i class="fa fa-trash-o"></i>&nbsp; 
                            </CPCC:StyleButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
