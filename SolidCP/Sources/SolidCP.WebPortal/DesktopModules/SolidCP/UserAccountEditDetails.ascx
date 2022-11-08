<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountEditDetails.ascx.cs" Inherits="SolidCP.Portal.UserAccountEditDetails" %>
<%@ Register TagPrefix="dnc" TagName="UserContact" Src="UserControls/ContactDetails.ascx" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
  <div class="panel-body form-horizontal">
<asp:BulletedList ID="blLog" runat="server" CssClass="ErrorText">
</asp:BulletedList>
    <div class="form-group">
        <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername" Text="Username:" CssClass="col-sm-2"></asp:Label>
        <div class="col-sm-10">
            <span class="form-control"><asp:Literal ID="lblUsername" Runat="server"></asp:Literal></span>
        </div>
    </div>
    <div class="form-group">
				<asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:" AssociatedControlID="txtFirstName" CssClass="col-sm-2"></asp:Label>
		<div class="col-sm-10">	
        <asp:TextBox id="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
				<asp:RequiredFieldValidator id="firstNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
					ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
            </div>
    </div>
    <div class="form-group">
				<asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last Name:" AssociatedControlID="txtLastName" CssClass="col-sm-2"></asp:Label>
<div class="col-sm-10">	
				<asp:TextBox id="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator id="lastNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
					ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
            </div>
    </div>
    <div class="form-group">
				<asp:Label ID="lblSubscriberNumber" runat="server" meta:resourcekey="lblSubscriberNumber" Text="Account Number:" AssociatedControlID="txtSubscriberNumber" CssClass="col-sm-2"></asp:Label>
<div class="col-sm-10">	
				<asp:TextBox id="txtSubscriberNumber" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
    </div>
    <div class="form-group">
				<asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Text="E-mail:" AssociatedControlID="txtEmail" CssClass="col-sm-2"></asp:Label>
<div class="col-sm-10">	
                <uc2:EmailControl id="txtEmail" runat="server" CssClass="form-control">
                </uc2:EmailControl>
            </div>
    </div>
    <div class="form-group">
				<asp:Label ID="lblSecondaryEmail" runat="server" meta:resourcekey="lblSecondaryEmail" Text="Secondary e-mail:" AssociatedControlID="txtSecondaryEmail" CssClass="col-sm-2"></asp:Label>
<div class="col-sm-10">	
                <uc2:EmailControl id="txtSecondaryEmail" runat="server" RequiredEnabled="false" CssClass="form-control">
                </uc2:EmailControl>
            </div>
    </div>
    <div class="form-group">
				<asp:Label ID="lblMailFormat" runat="server" meta:resourcekey="lblMailFormat" Text="Mail Format:"  AssociatedControlID="ddlMailFormat" CssClass="col-sm-2"></asp:Label>
            <div class="col-sm-10">	
				<asp:DropDownList ID="ddlMailFormat" runat="server"
				    CssClass="form-control" resourcekey="ddlMailFormat">
				    <asp:ListItem Value="Text">PlainText</asp:ListItem>
				    <asp:ListItem Value="HTML">HTML</asp:ListItem>
				</asp:DropDownList>
            </div>
    </div>
	<div id="rowRole" runat="server" class="form-group">
			    <asp:Label ID="lblRole" runat="server" meta:resourcekey="lblRole" Text="Role:"  AssociatedControlID="role" CssClass="col-sm-2"></asp:Label>
<div class="col-sm-10">	
				<asp:DropDownList id="role" runat="server" resourcekey="role" AutoPostBack="true" CssClass="form-control">
			        <asp:ListItem Value="User"></asp:ListItem>
			        <asp:ListItem Value="Reseller"></asp:ListItem>
				    <asp:ListItem Value="Administrator"></asp:ListItem>
				</asp:DropDownList>
			</div>
		</div>


		<div id="rowDemo" runat="server" class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
			    <div class="checkbox">
                    <asp:Label ID="lblDemoAccount" runat="server" AssociatedControlID="chkDemo">
                        <asp:CheckBox id="chkDemo" runat="server"></asp:CheckBox>
                        <asp:Localize runat="server" meta:resourcekey="lblDemoAccount"> Demo Account</asp:Localize>
                    </asp:Label>  
                </div>
            </div>
		</div>
        <div id="rowMfa" runat="server" class="form-group">
          <asp:Label ID="lblMfa" runat="server" meta:resourcekey="lblMfa" Text="Enable MFA:"  AssociatedControlID="role" CssClass="col-sm-2"></asp:Label>
            <div class="col-sm-10">
                <asp:CheckBox id="cbxMfaEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxMfaEnabled_CheckedChanged"></asp:CheckBox>
                <asp:Label ID="lblMfaEnabled" runat="server" meta:resourcekey="lblMfaEnabled" Text="When you log in, a validation code is sent to the primary email address. Enabling an authentication app stops the validation code from being sent by email."></asp:Label>
            </div>
		</div>

		<div id="roleLoginStatus" runat="server" class="form-group">
	
			    <asp:Label ID="lblLoginStatus" runat="server" meta:resourcekey="lblLoginStatus" Text="Login Status:" AssociatedControlID="loginStatus" CssClass="col-sm-2"></asp:Label>
                <div class="col-sm-10">	
				<asp:DropDownList id="loginStatus" runat="server" resourcekey="loginStatus"  CssClass="form-control">
			        <asp:ListItem Value="Enabled"></asp:ListItem>
			        <asp:ListItem Value="Disabled"></asp:ListItem>
				    <asp:ListItem Value="Locked Out"></asp:ListItem>
				</asp:DropDownList>
			</div>
		</div>
    

	<br/>

    <scp:CollapsiblePanel id="headContact" runat="server" IsCollapsed="true"
        TargetControlID="pnlContact" meta:resourcekey="secContact" Text="Contact">
    </scp:CollapsiblePanel>
	<asp:Panel ID="pnlContact" runat="server" Height="0" style="overflow:hidden;">
	    <dnc:usercontact id="contact" runat="server"></dnc:usercontact>
	</asp:Panel>

</div>
<div class="panel-footer text-right">
	<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/>
	</CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" useSubmitBehavior="false">
         <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/>
     </CPCC:StyleButton>
</div>