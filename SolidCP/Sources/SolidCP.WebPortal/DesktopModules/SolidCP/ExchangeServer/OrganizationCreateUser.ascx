<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCreateUser.ascx.cs" Inherits="SolidCP.Portal.HostedSolution.OrganizationCreateUser" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SendToControl.ascx" TagName="SendToControl" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div runat="server" id="divWrapper">
    <script language="javascript" type="text/javascript">
        function buildDisplayName() {
            document.getElementById("<%= txtDisplayName.ClientID %>").value = '';

            if (document.getElementById("<%= txtFirstName.ClientID %>").value != '')
                document.getElementById("<%= txtDisplayName.ClientID %>").value = document.getElementById("<%= txtFirstName.ClientID %>").value + ' ';

            if (document.getElementById("<%= txtInitials.ClientID %>").value != '')
                document.getElementById("<%= txtDisplayName.ClientID %>").value = document.getElementById("<%= txtDisplayName.ClientID %>").value + document.getElementById("<%= txtInitials.ClientID %>").value + ' ';

            if (document.getElementById("<%= txtLastName.ClientID %>").value != '')
                document.getElementById("<%= txtDisplayName.ClientID %>").value = document.getElementById("<%= txtDisplayName.ClientID %>").value + document.getElementById("<%= txtLastName.ClientID %>").value;
        }
    </script>
</div>
<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="OrganizationUserAdd48" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create User"></asp:Localize>
                </h3>
                        </div>
<asp:UpdatePanel ID="CreateUserUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="panel-body form-horizontal">
        <scp:SimpleMessageBox id="messageBox" runat="server" />
            <div id="NewUserDiv" runat="server">
             
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtFirstName">
                                        <asp:Localize ID="locName" runat="server" meta:resourcekey="locName" Text="Name:" />
                                    </asp:Label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" onKeyUp="buildDisplayName();" placeholder="First Name"></asp:TextBox>
                                            <span class="input-group-addon" title="Required"><i class="fa fa-asterisk" aria-hidden="true"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                            <asp:TextBox ID="txtInitials" runat="server" MaxLength="6" CssClass="form-control" onKeyUp="buildDisplayName();" placeholder="Initials"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" onKeyUp="buildDisplayName();" placeholder="Last Name"></asp:TextBox>
                                            <span class="input-group-addon" title="Required"><i class="fa fa-asterisk" aria-hidden="true"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtDisplayName">
                                        <asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name:" />
                                    </asp:Label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span class="input-group-addon" title="Required"><i class="fa fa-asterisk" aria-hidden="true"></i></span>
                                        </div>
                                        <asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
                                            ErrorMessage="Enter Display Name" ValidationGroup="CreateMailbox" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                  
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtSubscriberNumber">
                                        <asp:Localize ID="locSubscriberNumber" runat="server" meta:resourcekey="locSubscriberNumber" Text="Account Number: *" />
                                    </asp:Label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtSubscriberNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="valRequireSubscriberNumber" runat="server" meta:resourcekey="valRequireSubscriberNumber" ControlToValidate="txtSubscriberNumber"
                                            ErrorMessage="Enter Account Number" ValidationGroup="CreateMailbox" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="email">
                                        <asp:Localize ID="locAccount" runat="server" meta:resourcekey="locAccount" Text="E-mail Address: *" />
                                    </asp:Label>
                                    <div class="col-sm-10">
                                        <scp:EmailAddress ID="email" runat="server" ValidationGroup="CreateMailbox"></scp:EmailAddress>
                                    </div>
                                </div>
              
                </div>
                <scp:SendToControl ID="sendToControl" runat="server" ValidationGroup="CreateMailbox" ControlToHide="PasswordBlock"></scp:SendToControl>
                <div id="PasswordBlock" runat="server">
                    <scp:PasswordControl ID="password" runat="server" ValidationGroup="CreateMailbox" AllowGeneratePassword="true"></scp:PasswordControl>
                    <asp:CheckBox ID="chkUserMustChangePassword" runat="server" meta:resourcekey="chkUserMustChangePassword" Text="User must change password at next login" Visible="False" />
                </div>
                <div class="container">
                    <fieldset>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                        <label for="chkSendInstructions" class="col-sm-2 control-label">
                                        <asp:CheckBox ID="chkSendInstructions" runat="server" meta:resourcekey="chkSendInstructions" Text="Send Setup Instructions" Checked="true" />
                                        </label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <scp:EmailControl id="sendInstructionEmail" runat="server" RequiredEnabled="true" ValidationGroup="CreateMailbox"></scp:EmailControl></td>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
</ContentTemplate>
</asp:UpdatePanel>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="CreateMailbox" OnClientClick="ShowProgressDialog('Creating user...');"> <i class="fa fa-user">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateText"/> </CPCC:StyleButton>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateMailbox" />
</div>