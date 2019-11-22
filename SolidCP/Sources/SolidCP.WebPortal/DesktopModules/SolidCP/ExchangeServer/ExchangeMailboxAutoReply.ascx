<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxAutoReply.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxAutoReply" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<script src='/tinymce/tinymce.min.js'></script>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<script type="text/javascript">
    $(document).ready(function () {
        tinymce.init({
            selector: ".tinymce",
            plugins: ['active_directory advlist autolink lists link image charmap preview hr anchor pagebreak searchreplace htmlchar_count visualblocks visualchars code fullscreen insertdatetime media nonbreaking save table contextmenu directionality template paste textcolor colorpicker textpattern imagetools codesample'],
            toolbar: false,
            elementpath: false,
            custom_undo_redo_levels: 10,
            height: 250,
            max_chars: 8000,
            content_style: ".mce-content-body {font-size:12pt;font-family:Calibri,Arial,Helvetica,sans-serif;}",
            menu: {
                edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
                insert: { title: 'Insert', items: 'template active_directory | media image link | hr charmap' },
                view: { title: 'View', items: 'visualaid' },
                format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
                table: { title: 'Table', items: 'inserttable tableprops deletetable | cell row column' },
                tools: { title: 'Tools', items: 'code preview' },
            },
            toolbar1: 'undo redo | bold italic underline | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media',
            templates: [
                {}
            ],
        });
    });
</script>

<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image2" SkinID="ExchangeMailbox48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Auto Reply"></asp:Localize>
        -
		<asp:Literal ID="litDisplayName" runat="server" Text="" />
    </h3>
</div>
<div class="panel-body form-horizontal">
    <div class="nav nav-tabs" style="padding-bottom: 7px !important;">
        <scp:MailboxTabs ID="MailboxTabs" runat="server" SelectedTab="mailbox_autoreply" />
    </div>
    <div class="panel panel-default tab-content">
        <scp:SimpleMessageBox ID="messageBox" runat="server" />
        <div class="form-group">
            <div class="col-sm-10 form-inline">
                <table style="width: 950px;">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblSetAutoreply" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblSetAutoreply_SelectedIndexChanged">
                                <asp:ListItem Text="Don't send automatic replies" meta:resourcekey="rblSetAutoreplyOff" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Send automatic replies" meta:resourcekey="rblSetAutoreplyOn"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <asp:CheckBox ID="chkAutoReplyTime" runat="server" meta:resourcekey="chkAutoReplyTime" Text="Send replies only during this time period:" AutoPostBack="true" OnCheckedChanged="chkAutoReplyTime_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 30px; padding-bottom: 5px; padding-top: 5px;">
                            <asp:Label ID="locStartTime" runat="server" meta:resourcekey="locStartTime" Text="Start time:" Width="100"></asp:Label>
                            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>
                            <asp:TextBox ID="txtStartTime" runat="server" TextMode="Time"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 30px; padding-bottom: 10px; padding-top: 5px;">
                            <asp:Label ID="locEndTime" runat="server" meta:resourcekey="locEndTime" Text="End time:" Width="100"></asp:Label>
                            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
                            <asp:TextBox ID="txtEndTime" runat="server" TextMode="Time"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <asp:Localize ID="locIntReply" runat="server" meta:resourcekey="locIntReply" Text="Send a reply once to each sender inside my organization with the following message:"></asp:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <asp:TextBox ID="txtIntReply" runat="server" CssClass="tinymce" Rows="15" cols="20" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px; padding-top: 20px;">
                            <asp:CheckBox ID="chkOutsideOrganization" runat="server" meta:resourcekey="chkOutsideOrganization" Text="Send replies outside my organization" AutoPostBack="true" OnCheckedChanged="chkOutsideOrganization_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 30px; padding-bottom: 10px;">
                            <asp:RadioButtonList ID="rblExternalAudience" runat="server" AutoPostBack="False">
                                <asp:ListItem Text="Only to senders in my Contact list" meta:resourcekey="rblExtContact"></asp:ListItem>
                                <asp:ListItem Text="Send to all external senders" meta:resourcekey="rblExtAll" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 30px;">
                            <asp:Localize ID="locExtReply" runat="server" meta:resourcekey="locExtReply" Text="Send a reply once to each sender outside my organization with the following message:"></asp:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 30px;">
                            <asp:TextBox ID="txtExtReply" runat="server" CssClass="tinymce" Rows="15" cols="20" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<div class="panel-footer text-right">
    <scp:ItemButtonPanel ID="buttonPanel" runat="server" ValidationGroup="EditMailbox"
        OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
</div>
