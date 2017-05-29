
using System.Web.UI.WebControls;

namespace CPCC
{
    public class H3Label : Label
    {
        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.RenderBeginTag("H3");
        }
    }
}