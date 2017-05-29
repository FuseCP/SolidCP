<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserCreateUserAccount.ascx.cs"
    Inherits="SolidCP.Portal.UserCreateUserAccount" %>
<%@ Register TagPrefix="dnc" TagName="UserContact" Src="UserControls/ContactDetails.ascx" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
<asp:BulletedList ID="blLog" runat="server" CssClass="ErrorText">
</asp:BulletedList>
<asp:Panel ID="UserCreatUserAccountPanel" runat="server" DefaultButton="btnCreate">
    <div class="panel-body form-horizontal">

            <fieldset class="form-horizontal">
                
                        <div class="form-group">
                            <label ID="lblUsername1" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="locFirstName" runat="server" meta:resourcekey="lblUsername" Text="Username:" />
                            </label>
                            <div class="col-sm-10">
                                <div class="input-group">
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon" title="Required"><i class="fa fa-asterisk" aria-hidden="true"></i></span>
                                    <asp:RequiredFieldValidator ID="usernameValidator" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtUsername" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                   
            </fieldset>
            <uc2:PasswordControl ID="userPassword" runat="server" AllowGeneratePassword="true" />
            <fieldset class="form-horizontal">
        
                        <div class="form-group">
                            <label ID="lblFirstName" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocalFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="firstNameValidator" runat="server" ErrorMessage="*"
                                        Display="Dynamic" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                
            </fieldset>
            <fieldset class="form-horizontal">
           
                        <div class="form-group">
                            <label ID="lblLastName" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocLastName" runat="server" meta:resourcekey="lblLastName" Text="Last Name:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="lastNameValidator" runat="server" ErrorMessage="*"
                                        Display="Dynamic" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                  

         
                        <div class="form-group">
                            <label ID="lblSubscriberNumber" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocSubscriberNumber" runat="server" meta:resourcekey="lblSubscriberNumber" Text="Account Number:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:TextBox ID="txtSubscriberNumber" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
             

        
                        <div class="form-group">
                            <label ID="lblEmail" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="Localize4" runat="server" meta:resourcekey="lblEmail" Text="E-mail:" />
                            </label>
                            <div class="col-sm-10">
                                    <uc2:EmailControl ID="txtEmail" runat="server"></uc2:EmailControl>
                            </div>
                        </div>
             

         
                        <div class="form-group">
                            <label ID="lblSecondaryEmail" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="Localize5" runat="server" meta:resourcekey="lblSecondaryEmail" Text="Secondary e-mail:" />
                            </label>
                            <div class="col-sm-10">
                                    <uc2:EmailControl ID="txtSecondaryEmail" runat="server" RequiredEnabled="false"></uc2:EmailControl>
                            </div>
                        </div>
                

            
                        <div class="form-group">
                            <label ID="lblMailFormat" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocMailFormat" runat="server" meta:resourcekey="lblMailFormat" Text="Mail Format:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:DropDownList ID="ddlMailFormat" runat="server" CssClass="form-control" resourcekey="ddlMailFormat">
                        <asp:ListItem Value="Text">PlainText</asp:ListItem>
                        <asp:ListItem Value="HTML" Selected="True">HTML</asp:ListItem>
                    </asp:DropDownList>
                            </div>
                        </div>
            
    
              
                        <div class="form-group">
                            <label ID="lblRole" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocRole" runat="server" meta:resourcekey="lblRole" Text="Role:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:DropDownList ID="role" runat="server" resourcekey="role" AutoPostBack="true" CssClass="form-control">
                        <asp:ListItem Value="User"></asp:ListItem>
                        <asp:ListItem Value="Reseller"></asp:ListItem>
                    </asp:DropDownList>
                            </div>
                        </div>
                 
                        <div class="form-group">
                            <label ID="lblStatus" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocStatus" runat="server" meta:resourcekey="lblStatus" Text="Status:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:DropDownList ID="status" runat="server" resourcekey="ddlStatus" CssClass="form-control">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="2">Suspended</asp:ListItem>
                        <asp:ListItem Value="3">Cancelled</asp:ListItem>
                        <asp:ListItem Value="4">Pending</asp:ListItem>
                    </asp:DropDownList>
                            </div>
                        </div>
                    
                        <div class="form-group">
                            <label ID="lblDemoAccount" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocDemoAccount" runat="server" meta:resourcekey="lblDemoAccount" Text="Demo Account:" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:CheckBox ID="chkDemo" runat="server" meta:resourcekey="chkDemo" Text="Yes">
                    </asp:CheckBox>
                            </div>
                        </div>
                   
                        <div class="form-group">
                            <label ID="lblAccountLetter" runat="server" class="control-label col-sm-2">
                                <asp:Localize ID="LocAccountLetter" runat="server" meta:resourcekey="chkAccountLetter" Text="Send Account Summary Letter" />
                            </label>
                            <div class="col-sm-10">
                                    <asp:CheckBox ID="chkAccountLetter" runat="server" Text="YES" Checked="False" />
                                </div>
                                <asp:Panel runat="server" ID="pnlDisabledSummaryLetterHint" runat="server" Visible="false" class="alert alert-info">
                                    <label ID="lblDisabledSummaryLetterHint" runat="server" meta:resourcekey="lblDisabledSummaryLetterHint"
                            Text="To enable account summary letter please go to Mail Templates\User Account Summary Letter." />
                                </asp:Panel>
                        </div>
           
            </fieldset>


        <scp:CollapsiblePanel id="headContact" runat="server" IsCollapsed="true" TargetControlID="pnlContact" meta:resourcekey="secContact" Text="Contact">
        </scp:CollapsiblePanel>
        <asp:Panel ID="pnlContact" runat="server" Height="0" Style="overflow: hidden;">
            <dnc:usercontact id="contact" runat="server"></dnc:usercontact>
        </asp:Panel>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
        <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreate"/> </CPCC:StyleButton>
    </div>
</asp:Panel>
