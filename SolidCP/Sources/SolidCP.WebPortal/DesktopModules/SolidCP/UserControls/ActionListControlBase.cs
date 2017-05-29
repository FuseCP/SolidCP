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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;


namespace SolidCP.Portal.UserControls
{
    public abstract class ActionListControlBase<TEnum> : SolidCPControlBase 
    {
        public event CancelEventHandler ExecutingAction;

        public event EventHandler ExecutedAction;

        #region Properties

        public string GridViewID { get; set; }

        public string CheckboxesName { get; set; }

        private string _message = "ACTIONS_MESSAGE";
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        #endregion

        protected abstract DropDownList ActionsList { get; }

        protected abstract int DoAction(List<int> ids);

        public TEnum SelectedAction
        {
            get
            {
                return (TEnum)(object)Convert.ToInt32(ActionsList.SelectedValue);
            }
        }

        protected GridView GridView
        {
            get { return Parent.FindControl(GridViewID) as GridView; }
        }

        public void ResetSelection()
        {
            ActionsList.ClearSelection();
        }
        public void RemoveActionItem<TNum>(TNum value)
        {
            ActionsList.Items.Remove(ActionsList.Items.FindByValue(((int)(object)value).ToString()));
        }

        protected void FireExecuteAction()
        {
            if (ExecutingAction != null)
            {
                var e = new CancelEventArgs();
                ExecutingAction(this, e);

                if (e.Cancel) 
                    return;
            }

            DoAction();

            if (ExecutedAction != null)
                ExecutedAction(this, new EventArgs());
        }

        protected void DoAction()
        {
            if (GridView == null || String.IsNullOrWhiteSpace(CheckboxesName))
                return;

            // Get checked users
            var ids = Utils.GetCheckboxValuesFromGrid<int>(GridView, CheckboxesName);

            if ((int)(object)SelectedAction != 0)
            {
                if (ids.Count > 0)
                {
                    try
                    {
                        var result = DoAction(ids);

                        if (result < 0)
                        {
                            HostModule.ShowResultMessage(result);
                            return;
                        }

                        HostModule.ShowSuccessMessage(Message);
                    }
                    catch (Exception ex)
                    {
                        HostModule.ShowErrorMessage(Message, ex);
                    }

                    // Refresh users grid
                    GridView.DataBind();
                }
                else
                {
                    HostModule.ShowWarningMessage(Message);
                }
            }
        }

    }
}
