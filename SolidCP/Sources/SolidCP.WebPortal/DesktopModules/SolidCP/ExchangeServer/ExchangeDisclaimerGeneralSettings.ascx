<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDisclaimerGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeDisclaimerGeneralSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<script src='/tinymce/tinymce.min.js'></script>
<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />

<script type="text/javascript">
    tinymce.init({
        selector: ".tinymce",
        plugins: ['active_directory advlist autolink lists link image charmap preview hr anchor pagebreak searchreplace htmlchar_count visualblocks visualchars code fullscreen insertdatetime media nonbreaking save table contextmenu directionality template paste textcolor colorpicker textpattern imagetools codesample'],
        toolbar: false,
        custom_undo_redo_levels: 10,
        height: 250,
        max_chars: 5000,
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
            { title: 'Default Exchange Disclaimer', description: 'Exchange Disclaimer Default Template', url: '/tinymce/plugins/template/exchange_disclaimer_default.htm' }
        ],
    });
</script>
                <div class="panel-heading">
                    <h3 class="panel-title">
                        <asp:Image ID="Image1" SkinID="ExchangeDisclaimers48" runat="server" />
                        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Distribution List"></asp:Localize>
                        -
                        <asp:Literal ID="litDisplayName" runat="server" Text="" />
                    </h3>
                </div>
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox ID="messageBox" runat="server" />
                    <div class="form-group">
                        <asp:Label ID="locDisplayName" meta:resourcekey="locDisplayName" runat="server" Text="Display Name:" CssClass="col-sm-2" AssociatedControlID="txtDisplayName"></asp:Label>
                        <div class="col-sm-10 form-inline">
                            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" ValidationGroup="CreateMailbox" Style="width: 100%;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
                                ErrorMessage="Enter Display Name" ValidationGroup="EditList" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label ID="locNotes" meta:resourcekey="locNotes" runat="server" Text="Text:" CssClass="col-sm-2" AssociatedControlID="txtNotes"></asp:Label>
                        <div class="col-sm-10 form-inline">
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="tinymce" Rows="15" cols="20" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="panel-footer text-right">
                    <CPCC:StyleButton ID="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditList"><i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText" />
                    </CPCC:StyleButton>
                    &nbsp;
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditList" />
                </div>
