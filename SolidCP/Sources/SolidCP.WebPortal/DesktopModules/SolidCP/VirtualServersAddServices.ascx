<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServersAddServices.ascx.cs" Inherits="SolidCP.Portal.VirtualServersAddServices" %>
<div class="FormButtonsBar">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd"/> </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <asp:DataList ID="dlServers" Runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="15">
	    <ItemStyle Height="120px" VerticalAlign="Top"></ItemStyle>
	    <ItemTemplate>
            <div class="col-sm-12">
                <div class=" panel panel-info server-panel matchHeight">
                    <div class="panel-heading">
                        <h3 class="panel-title" style="line-height:inherit;white-space:nowrap;overflow:hidden;" title="<%# Eval("ServerName") %>">
                            <i class="fa fa-server" aria-hidden="true">&nbsp;</i>&nbsp;
                            <%# Eval("ServerName") %>
                        </h3>
                    </div>
                    <ul class="list-group">
                        <li class="list-group-item">
                            <%# Eval("Comments") %>
                        </li>
                        <li class="list-group-item">
						    <asp:DataList ID="dlServices" Runat="server" DataSource='<%# GetServerServices((int)Eval("ServerID")) %>'
						    CellPadding="1" CellSpacing="1" Width="50%" DataKeyField="ServiceID">
							    <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
							    <ItemTemplate>
				                     <asp:CheckBox ID="chkSelected" runat="server" Text='<%# Eval("ServiceName") %>' Width="100%" />
							    </ItemTemplate>
						    </asp:DataList>
                        </li>
                    </ul>
                </div>
            </div>
	    </ItemTemplate>
    </asp:DataList>
</div>