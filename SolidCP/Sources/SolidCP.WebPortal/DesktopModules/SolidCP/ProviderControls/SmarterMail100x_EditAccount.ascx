<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail100x_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail100x_EditAccount" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<script src='/tinymce/tinymce.min.js'></script>

<script type="text/javascript">
    tinymce.init({
        selector: ".tinymce",
        plugins: ['advlist autolink lists link image charmap preview hr anchor pagebreak searchreplace htmlchar_count visualblocks visualchars code fullscreen insertdatetime media nonbreaking save table contextmenu directionality template paste textcolor colorpicker textpattern imagetools codesample'],
        toolbar: false,
        custom_undo_redo_levels: 10,
        height: 250,
        menu: {
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'media image link | hr charmap' },
            view: { title: 'View', items: 'visualaid' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
            table: { title: 'Table', items: 'inserttable tableprops deletetable | cell row column' },
            tools: { title: 'Tools', items: 'code preview' },
        },
        toolbar1: 'undo redo | bold italic underline | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media',
    });
</script>

<div runat="server" id="passwordRow" class="form-group hide">
    <asp:Label ID="Label1" runat="server" CssClass="control-label col-sm-2" >
        <asp:Localize ID="Localize1" runat="server" meta:resourcekey="cbChangePassword" Text="Change password"/>
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:CheckBox runat="server" meta:resourcekey="cbEnabled" ID="cbChangePassword" Text="Check to Enable Password Change" Checked ="true"/>
    </div>
</div>
<div class="form-group">
    <asp:Label ID="Label2" runat="server" meta:resourcekey="cbEnableAccount" Text="Enable Account:" CssClass="control-label col-sm-2" >
        <asp:Localize ID="Localize2" runat="server" meta:resourcekey="cbEnableAccount" Text="Enable Account:" />
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:CheckBox runat="server" meta:resourcekey="cbEnabled" ID="cbEnableAccount" Text="Check to Enable Account" Checked="True" />
    </div>
</div>
<div id="domainAdminRow" runat="server" class="form-group">
    <asp:Label ID="Label3" runat="server" meta:resourcekey="cbDomainAdmin" Text="Domain Administrator:" CssClass="control-label col-sm-2" >
        <asp:Localize ID="Localize3" runat="server" meta:resourcekey="cbDomainAdmin" Text="Domain Administrator:" />
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:CheckBox runat="server" meta:resourcekey="cbEnabled" ID="cbDomainAdmin" Text="Check to set account as Domain Administrator" />
    </div>
</div>
<div class="form-group">
    <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:" CssClass="control-label col-sm-2" >
        <asp:Localize ID="locFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:" />
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:TextBox ID="txtFirstName" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last Name:" CssClass="control-label col-sm-2">
        <asp:Localize ID="LocLastName" runat="server" meta:resourcekey="lblLastName" Text="Last Name:" />
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:TextBox ID="txtLastName" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <asp:Label ID="lblReplyTo" runat="server" meta:resourcekey="lblReplyTo" Text="Reply to address:" CssClass="control-label col-sm-2">
        <asp:Localize ID="LocReplyTo" runat="server" meta:resourcekey="lblReplyTo" Text="Reply to address:" />
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:TextBox ID="txtReplyTo" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <asp:Label ID="lblSignature" runat="server" meta:resourcekey="lblSignature" Text="Signature:" CssClass="control-label col-sm-2" >
        <asp:Localize ID="LocSignature" runat="server" meta:resourcekey="lblSignature" Text="Signature:" />
    </asp:Label>
    <div runat="server" class="col-sm-10">
            <asp:TextBox ID="txtSignature" runat="server" Width="400px" TextMode="MultiLine" Rows="5" cols="20" CssClass="tinymce"></asp:TextBox>
    </div>
</div>


<scp:CollapsiblePanel id="secAutoresponder" runat="server" TargetControlID="AutoresponderPanel"
    meta:resourcekey="secAutoresponder" Text="Autoresponder">
</scp:CollapsiblePanel>
<asp:Panel ID="AutoresponderPanel" runat="server" Height="0" Style="overflow: hidden;">
    <table width="100%">
        <tr>
            <td class="SubHead" width="200" nowrap>
                <asp:Label ID="lblResponderEnabled" runat="server" meta:resourcekey="lblResponderEnabled" Text="Enable autoresponder:"></asp:Label></td>
            <td class="normal" width="100%">
                <asp:CheckBox ID="chkResponderEnabled" runat="server" meta:resourcekey="chkResponderEnabled" Text="Yes"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
            <td class="normal" valign="top">
                <asp:TextBox ID="txtSubject" runat="server" Width="400px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" Text="Message:"></asp:Label></td>
            <td class="normal">
                <asp:TextBox ID="txtMessage" runat="server" Width="400px"  TextMode="MultiLine" Rows="5" cols="20"
                    CssClass="tinymce"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<scp:CollapsiblePanel id="secForwarding" runat="server" TargetControlID="ForwardingPanel"
    meta:resourcekey="secForwarding" Text="Mail Forwarding">
</scp:CollapsiblePanel>
<asp:Panel ID="ForwardingPanel" runat="server" Height="0" Style="overflow: hidden;">
    <table width="100%">
        <tr>
            <td class="SubHead" width="200" nowrap>
                <asp:Label ID="lblForwardingEnabled" runat="server" meta:resourcekey="lblForwardingEnabled" Text="Enable Forwarding:"></asp:Label></td>
            <td class="normal" width="100%">
                <asp:CheckBox ID="chkForwardingEnabled" runat="server" meta:resourcekey="chkForwardingEnabled" Text="Yes"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap>
                <asp:Label ID="lblForwardTo" runat="server" meta:resourcekey="lblForwardTo" Text="Forward mail to address:"></asp:Label></td>
            <td class="normal" width="100%" valign="top">
                <asp:TextBox ID="txtForward" runat="server" Width="200px" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
            </td>
            <td class="Normal">
                <asp:CheckBox ID="chkDeleteOnForward" runat="server" meta:resourcekey="chkDeleteOnForward"
                    Text="Delete Message on Forward"></asp:CheckBox>
            </td>
        </tr>
    </table>
</asp:Panel>
