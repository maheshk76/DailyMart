using System.Web;
using System.Web.Optimization;

namespace DailyMart
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Extra styles
            bundles.Add(new StyleBundle("~/Content/stylesheets/css").Include(
                "~/Content/stylesheets/bootstrap.css",
                "~/Content/stylesheets/style.css",
                "~/Content/stylesheets/responsive.css",
                "~/Content/stylesheets/colors/color3.css",
                "~/Content/stylesheets/animate.css",
                "~/Content/toastr.css"
                ));


            //Extra scripts
            bundles.Add(new ScriptBundle("~/bundles/javascript/js").Include(
                
                    "~/Content/javascript/tether.min.js",
                    "~/Content/javascript/bootstrap.min.js",
                    "~/Content/javascript/jquery.easing.js",
                    "~/Content/javascript/parallax.js",
                    "~/Content/javascript/jquery.flexslider-min.js",
                    "~/Content/javascript/images-loaded.js",
                    "~/Content/javascript/jquery.isotope.min.js",
                    "~/Content/javascript/magnific.popup.min.js",
                    "~/Content/javascript/jquery.hoverdir.js",
                    "~/Content/javascript/owl.carousel.min.js",
                    "~/Content/javascript/equalize.min.js",
                    "~/Content/javascript/gmap3.min.js",
                    "~/Content/javascript/jquery.cookie.js",
                    "~/Content/javascript/main.js",
                    "~/Content/javascript/rev-slider.js",
                    "~/Content/javascript/jquery-ui.js",
                    "~/Content/javascript/jquery-waypoints.js",
                    "~/Content/javascript/jquery-countTo.js",
                    "~/Content/javascript/jquery.countdown.js",
                    "~/Scripts/toastr.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/rev-slider/js").Include(
                "~/Content/rev-slider/js/jquery.themepunch.tools.min.js",
    "~/Content/rev-slider/js/jquery.themepunch.revolution.min.js",
    "~/Content/javascript/rev-slider.js",
   "~/Content/rev-slider/js/extensions/revolution.extension.actions.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.carousel.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.kenburn.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.layeranimation.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.migration.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.navigation.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.parallax.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.slideanims.min.js",

   "~/Content/rev-slider/js/extensions/revolution.extension.video.min.js"


               ));

        }
    }
}
