<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Lync_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Lync_Settings" %>
<table>
    <tr>
        <td class="Normal" width="200" >
            <asp:Localize runat="server" ID="locServerName" meta:resourcekey="locServerName"/>
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtServerName"  CssClass="form-control" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtServerName" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="Normal" width="200" >
            <asp:Localize runat="server" ID="locSimpleUrlBase" meta:resourcekey="locSimpleUrlBase"/>
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtSimpleUrlBase"  CssClass="form-control" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSimpleUrlBase" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap valign="top">
            <asp:Localize ID="locLynServers" runat="server" meta:resourcekey="locLynServers"
                Text="Lync Servers:"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList ID="ddlLyncServers" runat="server" CssClass="form-control">
            </asp:DropDownList>
            <CPCC:StyleButton id="btnAddLyncServer" CssClass="btn btn-success" runat="server" OnClick="btnAddLyncServer_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
            <br />
            <asp:GridView ID="gvLyncServers" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
                CssSelectorClass="NormalGridView" OnRowCommand="gvLyncServers_RowCommand" meta:resourcekey="gvLyncServers">
                <Columns>
                    <asp:TemplateField meta:resourcekey="locServerNameColumn" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblServiceName" Text='<%#Eval("ServiceName") + "(" + Eval("ServerName") +")"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
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
