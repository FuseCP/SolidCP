
using System.Web.UI.WebControls;

namespace CPCC
{
    public class H1Label : Label
    {
        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.RenderBeginTag("H1");
        }
    }
}
