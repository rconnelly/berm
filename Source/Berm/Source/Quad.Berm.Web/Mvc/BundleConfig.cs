namespace Quad.Berm.Web.Mvc
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.FileSetOrderList.Clear();

            bundles.Add(new StyleBundle("~/styles/general").Include(
                "~/css/bootstrap.css",
                "~/css/bootstrap-responsive.css",
                "~/css/jquery.dataTables.css",
                "~/css/main.css"));

            bundles.Add(new ScriptBundle("~/scripts/modernizr").Include(
                "~/js/vendor/modernizr*"));

            bundles.Add(new ScriptBundle("~/scripts/general").Include(
                "~/js/vendor/jquery-1.*",
                "~/js/vendor/jquery-migrate*",
                "~/js/vendor/bootstrap*",
                "~/js/vendor/jquery.unobtrusive*",
                "~/js/vendor/jquery.validate-*",
                "~/js/vendor/jquery.validate.unobtrusive*",
                "~/js/vendor/mvcfoolproof.unobtrusive*",
                "~/js/vendor/jquery.dataTables.js",
                "~/js/vendor/jquery.dataTables.bootstrap.js",
                "~/js/site.bootstrap.validate.js",
                "~/js/app.js"));
        }
    }
}