<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSServersEditServer.ascx.cs" Inherits="SolidCP.Portal.RDSServersEditServer" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
	<scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="form-group">
        <asp:Label runat="server" CssClass="control-label col-sm-4" AssociatedControlID="lblServerName">
            <asp:Localize ID="locServerName" runat="server" meta:resourcekey="locServerName" Text="Server Fully Qualified Domain Name:"></asp:Localize>
        </asp:Label>
        <div class="col-sm-8">
            <asp:TextBox ID="lblServerName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" CssClass="control-label col-sm-4" AssociatedControlID="txtServerComments">
            <asp:Localize ID="locServerComments" runat="server" meta:resourcekey="locServerComments" Text="Server Comments:"></asp:Localize>
        </asp:Label>
        <div class="col-sm-8">
            <asp:TextBox ID="txtServerComments" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <scp:CollapsiblePanel id="secServerInfo" runat="server" TargetControlID="panelHardwareInfo" meta:resourcekey="secServerInfo" IsCollapsed="true" Text=""/>
    <asp:Panel runat="server" ID="panelHardwareInfo">
        <table>
            <tr>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="locStatus" runat="server" Text="Status:"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="litStatus" runat="server"/>
                </td>
                <td/>
                <td/>
            </tr>
            <tr>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="locProcessor" runat="server" Text="Processor:"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="litProcessor" runat="server"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="locLoadPercentage" Text="Load Percentage:" runat="server"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="litLoadPercentage" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="locMemoryAllocated" runat="server" Text="Allocated Memory:"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="litMemoryAllocated" runat="server"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="locFreeMemory" Text="Free Memory:" runat="server"/>
                </td>
                <td class="FormLabel150" style="width: 150px;">
                    <asp:Literal ID="litFreeMemory" runat="server"/>
                </td>
            </tr>
        </table>
        <table>
            <asp:Repeater ID="rpServerDrives" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Localize ID="locDeviceID" runat="server" meta:resourcekey="locDeviceID" />
                        </td>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="litDeviceId" runat="server" Text='<%# Eval("DeviceId") %>'/>
                        </td>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="locVolumeName" Text="Volume Name:" runat="server"/>
                        </td>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="litVolumeName" Text='<%# Eval("VolumeName") %>' runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="locSize" Text="Size:" runat="server"/>
                        </td>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="litSize" Text='<%# Eval("SizeMb") + " MB" %>' runat="server"/>
                        </td>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="locFreeSpace" Text="Free Space:" runat="server"/>
                        </td>
                        <td class="FormLabel150" style="width: 150px;">
                            <asp:Literal ID="litFreeSpace" Text='<%# Eval("FreeSpaceMb") + " MB" %>' runat="server"/>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
    &nbsp;
	<CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating server...');" useSubmitBehavior="false">
        <i class="fa fa-refresh">&nbsp;
        </i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/>
	</CPCC:StyleButton>
</div>