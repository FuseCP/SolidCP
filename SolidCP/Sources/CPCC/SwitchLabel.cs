using System;

using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CustomControls
{
    public class SwitchLabel : Label
    {
        protected void RadioButton_FixAutoPostBack_OnPreRender(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            HtmlGenericControl label = (HtmlGenericControl)radioButton.Parent;

            // Set onclick handler of the parent label to be the same as the radio button.
            //   This fixes issues caused by bootstrap javascript which adds labels.
            label.Attributes.Add("onclick",
                "javascript:setTimeout('__doPostBack(\\'" +
                radioButton.UniqueID + "\\',\\'\\')', 0)");
        }
    }
}
