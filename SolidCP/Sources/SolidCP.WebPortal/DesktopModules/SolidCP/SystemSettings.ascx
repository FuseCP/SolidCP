<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemSettings.ascx.cs" Inherits="SolidCP.Portal.SystemSettings" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EditFeedsList.ascx" TagName="EditFeedsList" TagPrefix="uc1" %>
<div class="panel-body">
    <div class="container">
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <div class="panel-heading panel-heading-link">
                    <span><i class="fa fa-envelope-o" aria-hidden="true">&nbsp;</i>&nbsp;</span>
                    <a data-toggle="collapse" data-parent="#accordion" href="#lclSmtpSettings" aria-expanded="false" class="collapsed">
                        <asp:Localize runat="server" meta:resourcekey="HeaderSmtpSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                    </a>
                </div>
                <div id="lclSmtpSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                    <div class="panel-body">
                        <fieldset>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <CPCC:H5Label runat="server" for="txtSmtpServer" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblSmtpServer" runat="server" meta:resourcekey="SettinglblSmtpServer" />
                                        </CPCC:H5Label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" CssClass="form-control" meta:resourcekey="SettingPlcSmtpServer" ID="txtSmtpServer" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <CPCC:H5Label runat="server" for="txtSmtpPort" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblSmtpPort" runat="server" meta:resourcekey="SettinglblSmtpPort" />
                                        </CPCC:H5Label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" CssClass="form-control" meta:resourcekey="SettingPlcSmtpPort" ID="txtSmtpPort" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <CPCC:H5Label runat="server" for="txtSmtpUser" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblSmtpUser" runat="server" meta:resourcekey="SettinglblSmtpUser" />
                                        </CPCC:H5Label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" CssClass="form-control" meta:resourcekey="SettingPlcSmtpUser" ID="txtSmtpUser" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <CPCC:H5Label runat="server" for="txtSmtpPassword" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblSmtpUserPassword" runat="server" meta:resourcekey="SettinglblSmtpUserPassword" />
                                        </CPCC:H5Label>
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSmtpPassword" meta:resourcekey="SettingPlcSmtpUserPassword" TextMode="Password" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <CPCC:H5Label runat="server" for="chkEnableSsl" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblSmtpEnableSSL" runat="server" meta:resourcekey="SettinglblSmtpEnableSSL" />
                                        </CPCC:H5Label>
                                        <div class="col-sm-6">
                                            <asp:CheckBox ID="chkEnableSsl" runat="server" CssClass="form-control" Text="Yes" meta:resourcekey="SettingchkSmtpEnableSSL" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <p><asp:Localize ID="configuremailtemplates" runat="server" meta:resourcekey="Settingsconfiguremailtemplates" /><br />
                               <asp:HyperLink ID="MailTemplates" runat="server" NavigateUrl="/Default.aspx?mid=25&ctl=mail_templates&UserID=1" Text="Serveradmin - Home"></asp:HyperLink></p>
                            </div>
                        </div>
                            </div>
                        <hr />
                        <CPCC:StyleButton ID="StyleButton1" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveSMTP_Click" />
                    </div>
                </div>
            </div>
        </div>


        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-hdd-o" aria-hidden="true">&nbsp;</i>&nbsp;&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#lclBackupSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize runat="server" meta:resourcekey="HeaderBackupSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="lclBackupSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="txtBackupsPath" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblBackupFolderPath" runat="server" meta:resourcekey="SettinglblBackupFolderPath" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtBackupsPath" meta:resourcekey="SettingPlcBackupFolderPath" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="StyleButton2" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveBACKUP_Click" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-windows" aria-hidden="true">&nbsp;</i>&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#lclWpiSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize runat="server" meta:resourcekey="HeaderWpiCustomFeeds" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="lclWpiSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="txtMainFeedUrl" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblWpiMainFeedUrl" runat="server" meta:resourcekey="SettinglblWpiMainFeedUrl" Text="Main feed URL:" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtMainFeedUrl" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="wpiEditFeedsList" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettingBtnWpiAddCustomFeeds" runat="server" meta:resourcekey="SettinglblWpiAddCustomFeeds" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <uc1:EditFeedsList ID="wpiEditFeedsList" runat="server" DisplayNames="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="StyleButton3" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveWPI_Click" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-file-text" aria-hidden="true">&nbsp;</i>&nbsp;&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#PanelFileManagereSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize runat="server" meta:resourcekey="HeaderFileManagerSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="PanelFileManagereSettings" class="panel-collapse collapse" style="overflow: hidden; height: 0px;" aria-expanded="false">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="txtFileManagerEditableExtensions" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblFileManagerEditableExtensions" runat="server" meta:resourcekey="SettinglblFileManagerEditableExtensions" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox TextMode="MultiLine" Rows="10" runat="server" ID="txtFileManagerEditableExtensions" CssClass="form-control" />
                                        <asp:Literal ID="SettinglitFileManagerEditableExtensions" runat="Server" meta:resourcekey="SettinglitFileManagerEditableExtensions" Text=" (One (1) extension per line)"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="StyleButton4" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveFILEMANAGER_Click" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-server" aria-hidden="true">&nbsp;</i>&nbsp;&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#RdsSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize runat="server" meta:resourcekey="HeaderRdsSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="RdsSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="ddlRdsController" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblRdsController" runat="server" meta:resourcekey="SettinglblRdsController" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlRdsController" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="StyleButton5" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveRDS_Click" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-stack-exchange" aria-hidden="true">&nbsp;</i>&nbsp;&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#OwaSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize runat="server" meta:resourcekey="HeaderOwaSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="OwaSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="chkEnableOwa" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblEnableOwa" runat="server" meta:resourcekey="SettinglblEnableOwa" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:CheckBox ID="chkEnableOwa" runat="server" CssClass="form-control" Text="Yes" meta:resourcekey="SettingchkEnableOwa" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="txtOwaUrl" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblOwaUrl" runat="server" meta:resourcekey="SettinglblOwaUrl" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtOwaUrl" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="StyleButton6" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveOWA_Click" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-cloud" aria-hidden="true">&nbsp;</i>&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse-764" aria-expanded="false" class="collapsed">
                    <asp:Localize runat="server" meta:resourcekey="HeaderCloudStorageSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="collapse-764" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <div class="panel-group" id="collapse-765">
                        <div class="panel panel-default">
                            <div class="panel-heading panel-heading-link">
                                <span><i class="fa fa-comments-o" aria-hidden="true">&nbsp;</i>&nbsp;</span>
                                <a data-toggle="collapse" data-parent="#collapse-765" href="#TwilioSettings" aria-expanded="false" class="collapsed">
                                    <asp:Localize runat="server" meta:resourcekey="HeaderTwilioSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                                </a>
                            </div>
                            <div id="TwilioSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                                <div class="panel-body">
                                    <div class="alert alert-info">
                                        <p>Visit <a href="https://www.twilio.com">https://www.twilio.com</a> to get your Twilio Information.</p>
                                    </div>
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <CPCC:H5Label runat="server" for="txtAccountSid" class="col-sm-2 control-label">
                                                                <asp:Localize ID="SettinglblTwilioAccountSid" runat="server" meta:resourcekey="SettinglblTwilioAccountSid" />
                                                    </CPCC:H5Label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox runat="server" ID="txtAccountSid" CssClass="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <CPCC:H5Label runat="server" for="txtAuthToken" class="col-sm-2 control-label">
                                                                <asp:Localize ID="SettinglblTwilioAuthToken" runat="server" meta:resourcekey="SettinglblTwilioAuthToken" />
                                                    </CPCC:H5Label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox runat="server" ID="txtAuthToken" CssClass="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <CPCC:H5Label runat="server" for="SettingNoteTwilioAccount" class="col-sm-2 control-label">
                                                                <asp:Localize ID="SettinglblTwilioPhoneFrom" runat="server" meta:resourcekey="SettinglblTwilioPhoneFrom" />
                                                    </CPCC:H5Label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox runat="server" ID="txtPhoneFrom" meta:resourcekey="SettingPlcTwilioPhoneFrom" CssClass="form-control" />
                                                        <asp:Localize ID="SettingNoteTwilioAccount" runat="server" meta:resourcekey="SettingNoteTwilioAccount" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                                </div>
                                            <div class="col-md-4">
                                    <CPCC:StyleButton ID="btnTwilioDisable" CssClass="btn btn-danger" runat="server" meta:resourcekey="btnTwillioDisable" OnClick="btnDisableTWILIO_Click" />
                                            </div>
                                            </div>
                                    </fieldset>
                                    <hr />
                                    <CPCC:StyleButton ID="StyleButton7" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveTWILIO_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading panel-heading-link">
                                <span><i class="fa fa-cloud" aria-hidden="true">&nbsp;</i>&nbsp;</span>
                                <a data-toggle="collapse" data-parent="#collapse-765" href="#WebdavPortalSettings" aria-expanded="false" class="collapsed">
                                    <asp:Localize runat="server" meta:resourcekey="HeaderCloudStorageSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                                </a>
                            </div>
                            <div id="WebdavPortalSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                                <div class="panel-body">
                                    <fieldset>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <CPCC:H5Label runat="server" for="chkEnablePasswordReset" class="col-sm-2 control-label">
                                                                <asp:Localize ID="SettinglblEnablePasswordReset" runat="server" meta:resourcekey="SettinglblEnablePasswordReset" />
                                                    </CPCC:H5Label>
                                                    <div class="col-sm-6">
                                                        <asp:CheckBox ID="chkEnablePasswordReset" runat="server" CssClass="form-control" Text="Yes" meta:resourcekey="SettingchkEnablePasswordReset" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <CPCC:H5Label runat="server" for="SettingNotePasswordResetLinkLifeSpan" class="col-sm-2 control-label">
                                                                <asp:Localize ID="SettinglblPasswordResetLinkLifeSpan" runat="server" meta:resourcekey="SettinglblPasswordResetLinkLifeSpan" />
                                                    </CPCC:H5Label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox runat="server" ID="txtPasswordResetLinkLifeSpan" CssClass="form-control" /><br />
                                                        <asp:Localize ID="SettingNotePasswordResetLinkLifeSpan" runat="server" meta:resourcekey="SettingNotePasswordResetLinkLifeSpan" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <CPCC:H5Label runat="server" for="txtWebdavPortalUrl" class="col-sm-2 control-label">
                                                                <asp:Localize ID="SettinglblWebdavPortalUrl" runat="server" meta:resourcekey="SettinglblWebdavPortalUrl" />
                                                    </CPCC:H5Label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox runat="server" ID="txtWebdavPortalUrl" meta:resourcekey="SettingPlcWebdavPortalUrl" CssClass="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <hr />
                                    <CPCC:StyleButton ID="StyleButton8" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveCLOUD_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-lock" aria-hidden="true">&nbsp;</i>&nbsp;&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#AccessIPsSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize ID="HeaderIpRestrictionSettings" runat="server" meta:resourcekey="HeaderIpRestrictionSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="AccessIPsSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="txtIPAddress" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettinglblIpAddressRestriction" runat="server" meta:resourcekey="SettinglblIpAddressRestriction" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" TextMode="MultiLine" Rows="10" ID="txtIPAddress" CssClass="form-control" />
                                        <div>
                                            <p class="text-info">
                                                Use this to Restrict administrator access from specific IP addresses, you can use single IP's or subnets (/26 /24 /22, etc..) Put one IP or Subnet per line and comma separate them.
                                                            <br />
                                                <strong>Examples:</strong><br />
                                                192.168.0.100,<br />
                                                10.0.1.0/24,<br />
                                                10.1.1.0/30<br />
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="StyleButton9" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnSaveRESTRICT_Click" />
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading panel-heading-link">
                <span><i class="fa fa-lock" aria-hidden="true">&nbsp;</i>&nbsp;&nbsp;</span>
                <a data-toggle="collapse" data-parent="#accordion" href="#AuthenticationSettings" aria-expanded="false" class="collapsed">
                    <asp:Localize ID="HeaderAuthenticationSettings" runat="server" meta:resourcekey="HeaderAuthenticationSettings" /><span class='fa fa-plus pull-right' aria-hidden='true'> </span>
                </a>
            </div>
            <div id="AuthenticationSettings" class="panel-collapse collapse" aria-expanded="false" style="height: 0px;">
                <div class="panel-body">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="txtMfaTokenAppDisplayName" class="col-sm-2 control-label">
                                                <asp:Localize ID="SettingtxtMfaTokenAppDisplayName" runat="server" meta:resourcekey="SettingtxtMfaTokenAppDisplayName" />
                                    </CPCC:H5Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" Rows="10" ID="txtMfaTokenAppDisplayName" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <CPCC:H5Label runat="server" for="chkCanPeerChangeMFa" class="col-sm-2 control-label">
                                        <asp:Localize ID="SettingchkCanPeerChangeMFa" runat="server" meta:resourcekey="SettingchkCanPeerChangeMFa" />
                                    </CPCC:H5Label>
                                <div class="col-sm-6">
                                    <asp:CheckBox ID="chkCanPeerChangeMFa" runat="server" CssClass="form-control" Text="Yes" meta:resourcekey="SettingchkCanPeerChangeMFa" />
                                </div>
                            </div>
                        </div>
                        </div>
                    </fieldset>
                    <hr />
                    <CPCC:StyleButton ID="btnAuthenticationSettings" CssClass="btn btn-success btn-block" runat="server" meta:resourcekey="SettingbtnSaveSettings" OnClick="btnAuthenticationSettings_Click" />
                </div>
            </div>
        </div>
    </div>
</div>
