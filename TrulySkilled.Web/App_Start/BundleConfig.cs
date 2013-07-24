using System.Web;
using System.Web.Optimization;

namespace TrulySkilled.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/Content/site").Include(
                "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/bootstrap/bootstrap-responsive.css"));
            bundles.Add(new StyleBundle("~/Content/zocial").Include(
                "~/Content/zocial/zocial.css",
                "~/Content/zocial/zocial-regular-webfont.eot",
                "~/Content/zocial/zocial-regular-webfont.svg",
                "~/Content/zocial/zocial-regular-webfont.tff",
                "~/Content/zocial/zocial-regular-webfont.woff"));

            bundles.Add(new ScriptBundle("~/bundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js")
                .Include("~/Scripts/jquery/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/bootstrap.js"));
        }
    }
}