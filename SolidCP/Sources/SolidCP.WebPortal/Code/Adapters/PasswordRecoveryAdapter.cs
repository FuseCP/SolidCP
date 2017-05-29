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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class PasswordRecoveryAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private enum State
        {
            UserName,
            VerifyingUser,
            UserLookupError,
            Question,
            VerifyingAnswer,
            AnswerLookupError,
            SendMailError,
            Success
        }
        State _state = State.UserName;
        string _currentErrorText = "";

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

        private MembershipProvider PasswordRecoveryMembershipProvider
        {
            get
            {
                MembershipProvider provider = Membership.Provider;
                PasswordRecovery passwordRecovery = Control as PasswordRecovery;
                if ((passwordRecovery != null) && (passwordRecovery.MembershipProvider != null) && (!String.IsNullOrEmpty(passwordRecovery.MembershipProvider)) && (Membership.Providers[passwordRecovery.MembershipProvider] != null))
                {
                    provider = Membership.Providers[passwordRecovery.MembershipProvider];
                }
                return provider;
            }
        }

        public PasswordRecoveryAdapter()
        {
            _state = State.UserName;
            _currentErrorText = "";
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PROTECTED        
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PasswordRecovery passwordRecovery = Control as PasswordRecovery;
            if (Extender.AdapterEnabled && (passwordRecovery != null))
            {
                RegisterScripts();
                passwordRecovery.AnswerLookupError += OnAnswerLookupError;
                passwordRecovery.SendMailError += OnSendMailError;
                passwordRecovery.UserLookupError += OnUserLookupError;
                passwordRecovery.VerifyingAnswer += OnVerifyingAnswer;
                passwordRecovery.VerifyingUser += OnVerifyingUser;
                _state = State.UserName;
                _currentErrorText = "";
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            PasswordRecovery passwordRecovery = Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                if ((passwordRecovery.UserNameTemplate != null) && (passwordRecovery.UserNameTemplateContainer != null))
                {
                    passwordRecovery.UserNameTemplateContainer.Controls.Clear();
                    passwordRecovery.UserNameTemplate.InstantiateIn(passwordRecovery.UserNameTemplateContainer);
                    passwordRecovery.UserNameTemplateContainer.DataBind();
                }

                if ((passwordRecovery.QuestionTemplate != null) && (passwordRecovery.QuestionTemplateContainer != null))
                {
                    passwordRecovery.QuestionTemplateContainer.Controls.Clear();
                    passwordRecovery.QuestionTemplate.InstantiateIn(passwordRecovery.QuestionTemplateContainer);
                    passwordRecovery.QuestionTemplateContainer.DataBind();
                }

                if ((passwordRecovery.SuccessTemplate != null) && (passwordRecovery.SuccessTemplateContainer != null))
                {
                    passwordRecovery.SuccessTemplateContainer.Controls.Clear();
                    passwordRecovery.SuccessTemplate.InstantiateIn(passwordRecovery.SuccessTemplateContainer);
                    passwordRecovery.SuccessTemplateContainer.DataBind();
                }
            }
        }

        protected void OnAnswerLookupError(object sender, EventArgs e)
        {
            _state = State.AnswerLookupError;
            PasswordRecovery passwordRecovery = Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                _currentErrorText = passwordRecovery.GeneralFailureText;
                if (!String.IsNullOrEmpty(passwordRecovery.QuestionFailureText))
                {
                    _currentErrorText = passwordRecovery.QuestionFailureText;
                }
            }
        }

        protected void OnSendMailError(object sender, SendMailErrorEventArgs e)
        {
            if (!e.Handled)
            {
                _state = State.SendMailError;
                _currentErrorText = e.Exception.Message;
            }
        }

        protected void OnUserLookupError(object sender, EventArgs e)
        {
            _state = State.UserLookupError;
            PasswordRecovery passwordRecovery = Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                _currentErrorText = passwordRecovery.GeneralFailureText;
                if (!String.IsNullOrEmpty(passwordRecovery.UserNameFailureText))
                {
                    _currentErrorText = passwordRecovery.UserNameFailureText;
                }
            }
        }

        protected void OnVerifyingAnswer(object sender, LoginCancelEventArgs e)
        {
            _state = State.VerifyingAnswer;
        }

        protected void OnVerifyingUser(object sender, LoginCancelEventArgs e)
        {
            _state = State.VerifyingUser;
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                Extender.RenderBeginTag(writer, "AspNet-PasswordRecovery");
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            PasswordRecovery passwordRecovery = Control as PasswordRecovery;
            if (passwordRecovery != null)
            {
                string provider = passwordRecovery.MembershipProvider;
            }

            //  By this time we have finished doing our event processing.  That means that if errors have
            //  occurred, the event handlers (OnAnswerLookupError, OnSendMailError or 
            //  OnUserLookupError) will have been called.  So, if we were, for example, verifying the
            //  user and didn't cause an error then we know we can move on to the next step, getting
            //  the answer to the security question... if the membership system demands it.

            switch (_state)
            {
                case State.AnswerLookupError:
                    // Leave the state alone because we hit an error.
                    break;
                case State.Question:
                    // Leave the state alone. Render a form to get the answer to the security question.
                    _currentErrorText = "";
                    break;
                case State.SendMailError:
                    // Leave the state alone because we hit an error.
                    break;
                case State.Success:
                    // Leave the state alone. Render a concluding message.
                    _currentErrorText = "";
                    break;
                case State.UserLookupError:
                    // Leave the state alone because we hit an error.
                    break;
                case State.UserName:
                    // Leave the state alone. Render a form to get the user name.
                    _currentErrorText = "";
                    break;
                case State.VerifyingAnswer:
                    // Success! We did not encounter an error while verifying the answer to the security question.
                    _state = State.Success;
                    _currentErrorText = "";
                    break;
                case State.VerifyingUser:
                    // We have a valid user. We did not encounter an error while verifying the user.
                    if (PasswordRecoveryMembershipProvider.RequiresQuestionAndAnswer)
                    {
                        _state = State.Question;
                    }
                    else
                    {
                        _state = State.Success;
                    }
                    _currentErrorText = "";
                    break;
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
                PasswordRecovery passwordRecovery = Control as PasswordRecovery;
                if (passwordRecovery != null)
                {
                    if ((_state == State.UserName) || (_state == State.UserLookupError))
                    {
                        if ((passwordRecovery.UserNameTemplate != null) && (passwordRecovery.UserNameTemplateContainer != null))
                        {
                            foreach (Control c in passwordRecovery.UserNameTemplateContainer.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                        else
                        {
                            WriteTitlePanel(writer, passwordRecovery);
                            WriteInstructionPanel(writer, passwordRecovery);
                            WriteHelpPanel(writer, passwordRecovery);
                            WriteUserPanel(writer, passwordRecovery);
                            if (_state == State.UserLookupError)
                            {
                                WriteFailurePanel(writer, passwordRecovery);
                            }
                            WriteSubmitPanel(writer, passwordRecovery);
                        }
                    }
                    else if ((_state == State.Question) || (_state == State.AnswerLookupError))
                    {
                        if ((passwordRecovery.QuestionTemplate != null) && (passwordRecovery.QuestionTemplateContainer != null))
                        {
                            foreach (Control c in passwordRecovery.QuestionTemplateContainer.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                        else
                        {
                            WriteTitlePanel(writer, passwordRecovery);
                            WriteInstructionPanel(writer, passwordRecovery);
                            WriteHelpPanel(writer, passwordRecovery);
                            WriteUserPanel(writer, passwordRecovery);
                            WriteQuestionPanel(writer, passwordRecovery);
                            WriteAnswerPanel(writer, passwordRecovery);
                            if (_state == State.AnswerLookupError)
                            {
                                WriteFailurePanel(writer, passwordRecovery);
                            }
                            WriteSubmitPanel(writer, passwordRecovery);
                        }
                    }
                    else if (_state == State.SendMailError)
                    {
                        WriteFailurePanel(writer, passwordRecovery);
                    }
                    else if (_state == State.Success)
                    {
                        if ((passwordRecovery.SuccessTemplate != null) && (passwordRecovery.SuccessTemplateContainer != null))
                        {
                            foreach (Control c in passwordRecovery.SuccessTemplateContainer.Controls)
                            {
                                c.RenderControl(writer);
                            }
                        }
                        else
                        {
                            WriteSuccessTextPanel(writer, passwordRecovery);
                        }
                    }
                    else
                    {
                        //  We should never get here.
                        System.Diagnostics.Debug.Fail("The PasswordRecovery control adapter was asked to render a state that it does not expect.");
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

        /////////////////////////////////////////////////////////
        // Step 1: user name
        /////////////////////////////////////////////////////////

        private void WriteTitlePanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((_state == State.UserName) || (_state == State.UserLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.UserNameTitleText))
                {
                    string className = (passwordRecovery.TitleTextStyle != null) && (!String.IsNullOrEmpty(passwordRecovery.TitleTextStyle.CssClass)) ? passwordRecovery.TitleTextStyle.CssClass + " " : "";
                    className += "AspNet-PasswordRecovery-UserName-TitlePanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className);
                    WebControlAdapterExtender.WriteSpan(writer, "", passwordRecovery.UserNameTitleText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((_state == State.Question) || (_state == State.AnswerLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.QuestionTitleText))
                {
                    string className = (passwordRecovery.TitleTextStyle != null) && (!String.IsNullOrEmpty(passwordRecovery.TitleTextStyle.CssClass)) ? passwordRecovery.TitleTextStyle.CssClass + " " : "";
                    className += "AspNet-PasswordRecovery-Question-TitlePanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className);
                    WebControlAdapterExtender.WriteSpan(writer, "", passwordRecovery.QuestionTitleText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        private void WriteInstructionPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((_state == State.UserName) || (_state == State.UserLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.UserNameInstructionText))
                {
                    string className = (passwordRecovery.InstructionTextStyle != null) && (!String.IsNullOrEmpty(passwordRecovery.InstructionTextStyle.CssClass)) ? passwordRecovery.InstructionTextStyle.CssClass + " " : "";
                    className += "AspNet-PasswordRecovery-UserName-InstructionPanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className);
                    WebControlAdapterExtender.WriteSpan(writer, "", passwordRecovery.UserNameInstructionText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((_state == State.Question) || (_state == State.AnswerLookupError))
            {
                if (!String.IsNullOrEmpty(passwordRecovery.QuestionInstructionText))
                {
                    string className = (passwordRecovery.InstructionTextStyle != null) && (!String.IsNullOrEmpty(passwordRecovery.InstructionTextStyle.CssClass)) ? passwordRecovery.InstructionTextStyle.CssClass + " " : "";
                    className += "AspNet-PasswordRecovery-Question-InstructionPanel";
                    WebControlAdapterExtender.WriteBeginDiv(writer, className);
                    WebControlAdapterExtender.WriteSpan(writer, "", passwordRecovery.QuestionInstructionText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        private void WriteFailurePanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if (!String.IsNullOrEmpty(_currentErrorText))
            {
                string className = (passwordRecovery.FailureTextStyle != null) && (!String.IsNullOrEmpty(passwordRecovery.FailureTextStyle.CssClass)) ? passwordRecovery.FailureTextStyle.CssClass + " " : "";
                className += "AspNet-PasswordRecovery-FailurePanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className);
                WebControlAdapterExtender.WriteSpan(writer, "", _currentErrorText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteHelpPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((!String.IsNullOrEmpty(passwordRecovery.HelpPageIconUrl)) || (!String.IsNullOrEmpty(passwordRecovery.HelpPageText)))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-HelpPanel");
                WebControlAdapterExtender.WriteImage(writer, passwordRecovery.HelpPageIconUrl, "Help");
                WebControlAdapterExtender.WriteLink(writer, passwordRecovery.HyperLinkStyle.CssClass, passwordRecovery.HelpPageUrl, "Help", passwordRecovery.HelpPageText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteUserPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((_state == State.UserName) || (_state == State.UserLookupError))
            {
                Control container = passwordRecovery.UserNameTemplateContainer;
                TextBox textBox = (container != null) ? container.FindControl("UserName") as TextBox : null;
                RequiredFieldValidator rfv = (textBox != null) ? container.FindControl("UserNameRequired") as RequiredFieldValidator : null;
                string id = (rfv != null) ? container.ID + "_" + textBox.ID : "";
                if (!String.IsNullOrEmpty(id))
                {
                    Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);
                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-UserName-UserPanel");
                    Extender.WriteTextBox(writer, false, passwordRecovery.LabelStyle.CssClass, passwordRecovery.UserNameLabelText, passwordRecovery.TextBoxStyle.CssClass, id, passwordRecovery.UserName);
                    WebControlAdapterExtender.WriteRequiredFieldValidator(writer, rfv, passwordRecovery.ValidatorTextStyle.CssClass, "UserName", passwordRecovery.UserNameRequiredErrorMessage);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((_state == State.Question) || (_state == State.AnswerLookupError))
            {
                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-Question-UserPanel");
                Extender.WriteReadOnlyTextBox(writer, passwordRecovery.LabelStyle.CssClass, passwordRecovery.UserNameLabelText, passwordRecovery.TextBoxStyle.CssClass, passwordRecovery.UserName);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        private void WriteSubmitPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if ((_state == State.UserName) || (_state == State.UserLookupError))
            {
                Control container = passwordRecovery.UserNameTemplateContainer;
                string id = (container != null) ? container.ID + "_Submit" : "Submit";

                string idWithType = WebControlAdapterExtender.MakeIdWithButtonType("Submit", passwordRecovery.SubmitButtonType);
                Control btn = (container != null) ? container.FindControl(idWithType) as Control : null;

                if (btn != null)
                {
                    Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                    PostBackOptions options = new PostBackOptions(btn, "", "", false, false, false, false, true, passwordRecovery.UniqueID);
                    string javascript = "javascript:" + Page.ClientScript.GetPostBackEventReference(options);
                    javascript = Page.Server.HtmlEncode(javascript);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-UserName-SubmitPanel");
                    Extender.WriteSubmit(writer, passwordRecovery.SubmitButtonType, passwordRecovery.SubmitButtonStyle.CssClass, id, passwordRecovery.SubmitButtonImageUrl, javascript, passwordRecovery.SubmitButtonText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
            else if ((_state == State.Question) || (_state == State.AnswerLookupError))
            {
                Control container = passwordRecovery.QuestionTemplateContainer;
                string id = (container != null) ? container.ID + "_Submit" : "Submit";
                string idWithType = WebControlAdapterExtender.MakeIdWithButtonType("Submit", passwordRecovery.SubmitButtonType);
                Control btn = (container != null) ? container.FindControl(idWithType) as Control : null;

                if (btn != null)
                {
                    Page.ClientScript.RegisterForEventValidation(btn.UniqueID);

                    PostBackOptions options = new PostBackOptions(btn, "", "", false, false, false, false, true, passwordRecovery.UniqueID);
                    string javascript = "javascript:" + Page.ClientScript.GetPostBackEventReference(options);
                    javascript = Page.Server.HtmlEncode(javascript);

                    WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-Question-SubmitPanel");
                    Extender.WriteSubmit(writer, passwordRecovery.SubmitButtonType, passwordRecovery.SubmitButtonStyle.CssClass, id, passwordRecovery.SubmitButtonImageUrl, javascript, passwordRecovery.SubmitButtonText);
                    WebControlAdapterExtender.WriteEndDiv(writer);
                }
            }
        }

        /////////////////////////////////////////////////////////
        // Step 2: question
        /////////////////////////////////////////////////////////

        private void WriteQuestionPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-QuestionPanel");
            Extender.WriteReadOnlyTextBox(writer, passwordRecovery.LabelStyle.CssClass, passwordRecovery.QuestionLabelText, passwordRecovery.TextBoxStyle.CssClass, passwordRecovery.Question);
            WebControlAdapterExtender.WriteEndDiv(writer);
        }

        private void WriteAnswerPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            Control container = passwordRecovery.QuestionTemplateContainer;
            TextBox textBox = (container != null) ? container.FindControl("Answer") as TextBox : null;
            RequiredFieldValidator rfv = (textBox != null) ? container.FindControl("AnswerRequired") as RequiredFieldValidator : null;
            string id = (rfv != null) ? container.ID + "_" + textBox.ID : "";
            if (!String.IsNullOrEmpty(id))
            {
                Page.ClientScript.RegisterForEventValidation(textBox.UniqueID);

                WebControlAdapterExtender.WriteBeginDiv(writer, "AspNet-PasswordRecovery-AnswerPanel");
                Extender.WriteTextBox(writer, false, passwordRecovery.LabelStyle.CssClass, passwordRecovery.AnswerLabelText, passwordRecovery.TextBoxStyle.CssClass, id, "");
                WebControlAdapterExtender.WriteRequiredFieldValidator(writer, rfv, passwordRecovery.ValidatorTextStyle.CssClass, "Answer", passwordRecovery.AnswerRequiredErrorMessage);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }

        /////////////////////////////////////////////////////////
        // Step 3: success
        /////////////////////////////////////////////////////////

        private void WriteSuccessTextPanel(HtmlTextWriter writer, PasswordRecovery passwordRecovery)
        {
            if (!String.IsNullOrEmpty(passwordRecovery.SuccessText))
            {
                string className = (passwordRecovery.SuccessTextStyle != null) && (!String.IsNullOrEmpty(passwordRecovery.SuccessTextStyle.CssClass)) ? passwordRecovery.SuccessTextStyle.CssClass + " " : "";
                className += "AspNet-PasswordRecovery-SuccessTextPanel";
                WebControlAdapterExtender.WriteBeginDiv(writer, className);
                WebControlAdapterExtender.WriteSpan(writer, "", passwordRecovery.SuccessText);
                WebControlAdapterExtender.WriteEndDiv(writer);
            }
        }
    }
}
