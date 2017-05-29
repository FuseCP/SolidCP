using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.RDS.UserControls
{
    public partial class RDSSessionLimit : System.Web.UI.UserControl
    {
        public int SelectedLimit
        {
            get
            {
                return Convert.ToInt32(SessionLimit.SelectedItem.Value);
            }
            set
            {
                SessionLimit.SelectedValue = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}