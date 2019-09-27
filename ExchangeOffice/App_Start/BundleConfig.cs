using System.Web.Optimization;

namespace ExchangeOffice
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Content/jquery/jquery.min.js",
                "~/Content/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/jquery-easing/jquery.easing.min.js",
                "~/Content/chart.js/Chart.min.js",
                "~/Content/datatables/jquery.dataTables.js",
                "~/Content/datatables/dataTables.bootstrap4.js",
                "~/js/sb-admin.min.js",
                "~/Scripts/jquery-{version}.js",
                "~/Content/Datatables/datatables.js",
                "~/Content/Datatables/Select-1.2.6/js/dataTables.select.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/css/bootstrap.min.css",
                "~/Content/fontawesome-free/css/all.min.css",
                "~/Content/datatables/dataTables.bootstrap4.css",
                "~/Content/Datatables/Select-1.2.6/css/select.dataTables.css",
                "~/Content/css/sb-admin.css",
                "~/Content/site.css"));
        }
    }
}
