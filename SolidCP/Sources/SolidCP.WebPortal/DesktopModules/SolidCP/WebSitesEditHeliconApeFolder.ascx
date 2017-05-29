<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditHeliconApeFolder.ascx.cs"
	Inherits="SolidCP.Portal.WebSitesEditHeliconApeFolder" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>
<link rel="stylesheet" href="/JavaScript/codemirror/codemirror.css" />
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>
<script type="text/javascript" src="/JavaScript/codemirror/codemirror.js"></script>
<script type="text/javascript" src="/JavaScript/codemirror/htaccess.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.LinkDebuggingPage').click(function () {
            //$find('').hide();
            return true;
        });
    });
</script>
<style type="text/css">
.CodeMirror {
    border: 1px solid #444;
    padding: 2px;
    font-family: Consolas, monospace;
    font-size: 14px;
}
</style>
<div class="panel-body form-horizontal">
<table cellspacing="0" cellpadding="5" width="100%">
	<tr>
		<td class="SubHead" style="white-space: nowrap;">
			<asp:Label ID="lblFolderName" runat="server" meta:resourcekey="lblFolderName"></asp:Label>
		</td>
		<td class="NormalBold" style="white-space: nowrap;">
		    <asp:Label runat="server" ID="LabelWebSiteName"></asp:Label>

			<uc1:FileLookup id="folderPath" runat="server" Width="400">
			</uc1:FileLookup>
			<asp:HiddenField ID="contentPath" runat="server" />
			<asp:HiddenField ID="DebuggerUrlField" runat="server" />
		</td>
        <td style="width: 40%">
            <CPCC:StyleButton id="ButtonDebuggerStop" CssClass="btn btn-warning" runat="server" OnClick="DebugStopClick"> <i class="fa fa-stop-circle">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnApeDebuggerStopText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="ButtonDebuggerStart" CssClass="btn btn-success" runat="server" OnClick="DebugStartClick"> <i class="fa fa-play-circle">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnApeDebuggerStartText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="BUttonShowDebuggingPageLinkModal" CssClass="btn btn-success Hidden" runat="server"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnShowDebuggingPageLinkModalText"/> </CPCC:StyleButton>
        </td>
	</tr>
</table>

<asp:Panel runat="server" ID="DebuggerFramePanel" Visible="False">
    <iframe runat="server" ID="DebuggerFrame" width="100%" height="400px"></iframe>
</asp:Panel>

<asp:TextBox ID="htaccessContent" runat="server" TextMode="MultiLine" class="CodeEditor"></asp:TextBox>

</div>

<div class="panel-footer text-right">
	<CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" > <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>&nbsp;
	<<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="BtnCancelClick"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSave"/> </CPCC:StyleButton>
</div>

<asp:Panel ID="DebuggingPageLinkPanel" runat="server" CssClass="PopupContainer" style="display:none" DefaultButton="btnCancelDebuggingPageLinkPanel">
    <div class="widget">
        <div class="widget-header clearfix">
            <h3><i class="fa fa-list"></i> <scp:PopupHeader runat="server" meta:resourcekey="lblDebuggingPageLink" Text="Debugging Page Link" /></h3>
        </div>
        <div class="widget-content Popup">
			<div class="FormRow">
				<asp:Label ID="LabelClickLink" runat="server" meta:resourcekey="lblCLickLink" Text="Click this link to open debugging page"></asp:Label>:
                <br/>
                <br/>
				<asp:HyperLink runat="server" ID="LinkDebuggingPage" Target="ape-debugging-page" CssClass="LinkDebuggingPage"></asp:HyperLink>
			</div>
			</div>
					<div class="popup-buttons text-right">
            <CPCC:StyleButton id="btnCancelDebuggingPageLinkPanel" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCloseText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="DebuggingPageLinkModal" runat="server"
    PopupControlID="DebuggingPageLinkPanel" TargetControlID="BUttonShowDebuggingPageLinkModal"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelDebuggingPageLinkPanel" />


<script>
    $(document).ready(function() {
        CodeMirror.fromTextArea(($('.CodeEditor')[0]),
            {
                lineNumbers: true,
                autofocus: true
            });
    });
</script>
