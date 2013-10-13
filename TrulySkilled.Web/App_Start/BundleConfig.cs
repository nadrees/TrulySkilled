using System.Web.Optimization;

namespace TrulySkilled.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/bundles/Content/css").Include(
                "~/Content/site.css",
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/bootstrap/bootstrap-responsive.css"));
            bundles.Add(new StyleBundle("~/bundles/Content/zocial").Include(
                "~/Content/zocial/zocial.css",
                "~/Content/zocial/zocial-regular-webfont.eot",
                "~/Content/zocial/zocial-regular-webfont.svg",
                "~/Content/zocial/zocial-regular-webfont.tff",
                "~/Content/zocial/zocial-regular-webfont.woff"));

            bundles.Add(new ScriptBundle("~/bundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js")
                .Include("~/Scripts/jquery/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/LobbyBundle").Include(
                "~/Scripts/signalr/jquery.signalR-{version}.js",
                "~/Scripts/knockout/knockout-{version}.js",
                "~/Scripts/chat/trulyskilled.chat.js"));
            bundles.Add(new ScriptBundle("~/bundles/TicTacToeBundle").Include(
                "~/Scripts/signalr/jquery.signalR-{version}.js",
                "~/Scripts/knockout/knockout-{version}.js",
                "~/Scripts/chat/trulyskilled.chat.js",
                "~/Scripts/kinetic/kinetic-v{version}.js",
                "~/Scripts/games/trulyskilled.tictactoe.js"));
        }
    }
}