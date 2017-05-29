// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// Material sourced from the bluePortal project (http://blueportal.codeplex.com).
// Licensed under the Microsoft Public License (available at http://www.opensource.org/licenses/ms-pl.html).

using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class LoginAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private enum State
        {
            LoggingIn,
            Failed,
            Success,
        }
        State _state = State.LoggingIn;

        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                    ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        public LoginAdapter()
        {
            _state = State.LoggingIn;
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Login login = Control as Login;
            if (Extender.AdapterEnabled && (login != null))
            {
                RegisterScripts();
                login.LoggedIn += OnLoggedIn;
                login.LoginError += OnLoginError;
                _state = State.LoggingIn;
            }
        }

        protected override void  CreateChildControls()
        {
            base.CreateChildControls();
            Login login = Control as Login;
            if ((login != null) && (login.Controls.Count == 1) && (login.LayoutTemplate != null))
            {
                Control container = login.Controls[0];
                if (container != null)
                {
                    container.Controls.Clear();
                    login.LayoutTemplate.InstantiateIn(container);
                    container.DataBind();
                }
            }
        }

        protected void OnLoggedIn(object sender, EventArgs e)
        {
            _state = State.Success;
        }

        protected void OnLoginError(object sender, EventArgs e)
        {
            _state = State.Failed;
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-Login");
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Login login = Control as Login;
                if (login != null)
                {
                    if (login.LayoutTemplate != null)
                    {
                        if (login.Controls.Count == 1)
                        {
                            Control container = login.Controls[0];
                            if (container != null)
                            {
                                foreach (Control c in container.Controls)
                                {
                                    c.RenderControl(writer);
                                }
                            }
                        }
                    }
                    else
                    {
                        WriteTitlePanel(writer, login);
                        WriteInstructionPanel(writer, login);
                        WriteHelpPanel(writer, login);
                        WriteUserPanel(writer, login);
                        WritePasswordPanel(writer, login);
                        WriteRememberMePanel(writer, login);
                        if (_state == State.Failed)
                        {
                            WriteFailurePanel(writer, login);
                        }
                        WriteSubmitPanel(writer, login);
                        WriteCreateUserPanel(writer, login);
                        WritePasswordRecoveryPanel(writer, login);
                    }
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PRIVATE        

        private void RegisterScripts()
        {
        }

        private void WriteTitlePanel(HtmlTextWriter writer, Login login)
        {
            if (!String.IsNullOrEmpty(login.TitleText))
            {
                string className = (login.TitleTextStyle != null) && (!String.IsNullOrEmpty(login.TitleTextStyle.CssClass)) ? login.TitleTextStyle.CssClass + " " : "";
                className += "AspNet-Login-TitlePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className);
                WebControlAdapterExtender.WriteSpan(writer, "", login.TitleText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteInstructionPanel(HtmlTextWriter writer, Login login)
        {
            if (!String.IsNullOrEmpty(login.InstructionText))
            {
                string className = (login.InstructionTextStyle != null) && (!String.IsNullOrEmpty(login.InstructionTextStyle.CssClass)) ? login.InstructionTextStyle.CssClass + " " : "";
                className += "AspNet-Login-InstructionPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className);
                WebControlAdapterExtender.WriteSpan(writer, "", login.InstructionText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteFailurePanel(HtmlTextWriter writer, Login login)
        {
            if (!String.IsNullOrEmpty(login.FailureText))
            {
                string className = (login.FailureTextStyle != null) && (!String.IsNullOrEmpty(login.FailureTextStyle.CssClass)) ? login.FailureTextStyle.CssClass + " " : "";
                className += "AspNet-Login-FailurePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className);
                WebControlAdapterExtender.WriteSpan(writer, "", login.FailureText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteHelpPanel(HtmlTextWriter writer, Login login)
        {
            if ((!String.IsNullOrEmpty(login.HelpPageIconUrl)) || (!String.IsNullOrEmpty(login.HelpPageText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-HelpPanel");
                WebControlAdapterExtender.WriteImage(writer, login.HelpPageIconUrl, "Help");
                WebControlAdapterExtender.WriteLink(writer, login.HyperLinkStyle.CssClass, login.HelpPageUrl, "Help", login.HelpPageText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteUserPanel(HtmlTextWriter writer, Login login)
        {
            Page.ClientScript.RegisterForEventValidation(login.FindControl("UserName").UniqueID);
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-UserPanel");
            Extender.WriteTextBox(writer, false, login.LabelStyle.CssClass, login.UserNameLabelText, login.TextBoxStyle.CssClass, "UserName", login.UserName);
            WebControlAdapterExtender.WriteRequiredFieldValidator(writer, login.FindControl("UserNameRequired") as RequiredFieldValidator, login.ValidatorTextStyle.CssClass, "UserName", login.UserNameRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WritePasswordPanel(HtmlTextWriter writer, Login login)
        {
            Page.ClientScript.RegisterForEventValidation(login.FindControl("Password").UniqueID);
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-PasswordPanel");
            Extender.WriteTextBox(writer, true, login.LabelStyle.CssClass, login.PasswordLabelText, login.TextBoxStyle.CssClass, "Password", "");
            WebControlAdapterExtender.WriteRequiredFieldValidator(writer, login.FindControl("PasswordRequired") as RequiredFieldValidator, login.ValidatorTextStyle.CssClass, "Password", login.PasswordRequiredErrorMessage);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WriteRememberMePanel(HtmlTextWriter writer, Login login)
        {
            if (login.DisplayRememberMe)
            {
                Page.ClientScript.RegisterForEventValidation(login.FindControl("RememberMe").UniqueID);
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-RememberMePanel");
                Extender.WriteCheckBox(writer, login.LabelStyle.CssClass, login.RememberMeText, login.CheckBoxStyle.CssClass, "RememberMe", login.RememberMeSet);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteSubmitPanel(HtmlTextWriter writer, Login login)
        {
            string id = "Login";
            string idWithType = WebControlAdapterExtender.MakeIdWithButtonType(id, login.LoginButtonType);
            Control btn = login.FindControl(idWithType);
            if (btn != null)
            {
                Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-SubmitPanel");

                PostBackOptions options = new PostBackOptions(btn, "", "", false, false, false, false, true, login.UniqueID);
                string javascript = "javascript:" + Page.ClientScript.GetPostBackEventReference(options);
                javascript = Page.Server.HtmlEncode(javascript);

                Extender.WriteSubmit(writer, login.LoginButtonType, login.LoginButtonStyle.CssClass, id, login.LoginButtonImageUrl, javascript, login.LoginButtonText);

                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteCreateUserPanel(HtmlTextWriter writer, Login login)
        {
            if ((!String.IsNullOrEmpty(login.CreateUserUrl)) || (!String.IsNullOrEmpty(login.CreateUserText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-CreateUserPanel");
                WebControlAdapterExtender.WriteImage(writer, login.CreateUserIconUrl, "Create user");
                WebControlAdapterExtender.WriteLink(writer, login.HyperLinkStyle.CssClass, login.CreateUserUrl, "Create user", login.CreateUserText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WritePasswordRecoveryPanel(HtmlTextWriter writer, Login login)
        {
            if ((!String.IsNullOrEmpty(login.PasswordRecoveryUrl)) || (!String.IsNullOrEmpty(login.PasswordRecoveryText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-Login-PasswordRecoveryPanel");
                WebControlAdapterExtender.WriteImage(writer, login.PasswordRecoveryIconUrl, "Password recovery");
                WebControlAdapterExtender.WriteLink(writer, login.HyperLinkStyle.CssClass, login.PasswordRecoveryUrl, "Password recovery", login.PasswordRecoveryText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }
    }
}
