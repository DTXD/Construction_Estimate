using System.Web;
using System.Web.Optimization;

namespace Du_Toan_Xay_Dung
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/Angular/angular.js",
                        "~/Scripts/Angular/angular-ui-router.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Bootstrap/bootstrap.js",
                      "~/Scripts/Bootstrap/bootstrap-select.js",
                      "~/Scripts/Difference/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Style.css",
                      "~/Content/AdminLTE.css",
                      "~/Content/Bootstrap/bootstrap.css",
                      "~/Content/Bootstrap/bootstrap-select.css",
                      "~/Content/font-awesome-4.6.3/css/font-awesome.css"
                      ));
        }
    }
}
