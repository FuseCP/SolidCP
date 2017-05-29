<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TasksTaskDetails.ascx.cs" Inherits="SolidCP.Portal.TasksTaskDetails" %>
<%@ Import Namespace="SolidCP.Portal" %>

<div class="panel-body form-horizontal">
<asp:Timer runat="server" Interval="4000" ID="tasksTimer" />
<asp:UpdatePanel runat="server" ID="tasksUpdatePanel" UpdateMode="Conditional">
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="tasksTimer" EventName="Tick" />
  </Triggers>
  <ContentTemplate>
  
<table width="400">
    <tr>
        <td class="MediumBold">
            <asp:Literal id="litTitle" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td>
            <fieldset>
                <table width="100%" cellpadding="3">
                    <tr>
                        <td class="NormalBold" colspan="2">
                            <asp:Literal id="litStep" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="ProgressBarContainer">
                                <asp:Panel id="pnlProgressBar" CssClass="ProgressBarIndicator" runat="server"></asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBold" width="100" nowrap>
                            <asp:Localize ID="locStarted" runat="server" meta:resourcekey="locStarted" Text="Started:"/>
                        </td>
                        <td class="Normal" width="100%">
                            <asp:Literal id="litStartTime" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBold">
                            <asp:Localize ID="locDuration" runat="server" meta:resourcekey="locDuration" Text="Duration:"/>
                        </td>
                        <td class="Normal">
                            <asp:Literal id="litDuration" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td align="center">
            <CPCC:StyleButton id="btnStop" CssClass="btn btn-danger" runat="server" OnClick="btnStop_Click" OnClientClick="return confirm('Do you really want to terminate this task?');"> <i class="fa fa-hand-paper-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnStopText"/> </CPCC:StyleButton>
        </td>
    </tr>
</table>

<table width="400">
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label ID="lblExecutionLog" runat="server" meta:resourcekey="lblExecutionLog" Text="Execution Log:"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="Normal">
            <asp:Panel ID="pnlLog" runat="server" style="border: solid 1px #e0e0e0; width: auto; height: 175px; overflow: auto; white-space: nowrap; background-color: #ffffff;padding:3px;">
                <asp:Literal ID="litLog" runat="server"></asp:Literal>
            </asp:Panel>
        </td>
    </tr>
</table>

</ContentTemplate>
</asp:UpdatePanel>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnBack" CssClass="btn btn-warning" runat="server" OnClick="btnBack_Click" CausesValidation="false"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBackText"/> </CPCC:StyleButton>
</div>