namespace Quad.Berm.Web.App_Start
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.FileSetOrderList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/general").Include(
                "~/Scripts/jquery-1.*",
                "~/Scripts/modernizr-*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/site.bootstrap.validate.js",
                "~/Scripts/mvcfoolproof.unobtrusive*"));

            bundles.Add(new StyleBundle("~/Content/css/general").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/site.css"));
        }
    }
}