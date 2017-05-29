using System.Web.Optimization;

namespace SolidCP.WebDavPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var jQueryBundle = new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.cookie.js");

            jQueryBundle.IncludeDirectory("~/Scripts", "jquery.dataTables.min.js", true);
            jQueryBundle.IncludeDirectory("~/Scripts", "dataTables.bootstrap.js", true);

            bundles.Add(jQueryBundle);

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/appScripts/validation/passwordeditor.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/appScripts").Include(
                "~/Scripts/appScripts/messages.js",
                "~/Scripts/appScripts/fileBrowsing.js",
                "~/Scripts/appScripts/dialogs.js",
                "~/Scripts/appScripts/SCP.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/appScripts-webdav").Include(
                "~/Scripts/appScripts/SCP-webdav.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/bigIconsScripts").Include(
                "~/Scripts/appScripts/recalculateResourseHeight.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/authScripts").Include(
               "~/Scripts/appScripts/authentication.js"));

            bundles.Add(new ScriptBundle("~/bundles/file-upload").Include(
                "~/Scripts/jquery.ui.widget.js",
                "~/Scripts/jQuery.FileUpload/tmpl.min.js",
                "~/Scripts/jQuery.FileUpload/load-image.min.js",
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-image.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js"
               ));

            var styleBundle = new StyleBundle("~/Content/css");

            styleBundle.Include(
                "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                "~/Content/jQuery.FileUpload/css/jquery.fileupload-ui.css",
                "~/Content/bootstrap.css",
                "~/Content/site.css");

            styleBundle.IncludeDirectory("~/Content", "jquery.datatables.css", true);
            styleBundle.IncludeDirectory("~/Content", "dataTables.bootstrap.css", true);

            bundles.Add(styleBundle);

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}