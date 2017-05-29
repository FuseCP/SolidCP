using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CPCC
{
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class StyleButton : Button
    {
        protected override string TagName
        {
            get { return "button"; }
        }
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Button; }
        }
        public new string Text
        {
            get { return ViewState["NewText"] as string; }
            set { ViewState["NewText"] = HttpUtility.HtmlDecode(value); }
        }
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
           
            LiteralControl lc = new LiteralControl(this.Text);
            Controls.Add(lc);
            base.Text = UniqueID;
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            RenderChildren(writer);
        }
    }
}
