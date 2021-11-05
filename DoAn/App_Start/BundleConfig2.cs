using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace DoAn.App_Start
{
    public class BundleConfig2
    {
        public static void RegisterBundles(BundleCollection bundle)
        {
            bundle.Add(new StyleBundle("~/Pubilc/css").Include("~/Asset/css/bootstrap.min.css",
                                                                "~/Asset/css/prettyPhoto.css",
                                                                "~/Asset/css/price-range.css",
                                                                "~/Asset/css/animate.css",
                                                                "~/Asset/css/main.css",
                                                                "~/Asset/css/responsive.css"));

            bundle.Add(new ScriptBundle("~/Public/js").Include("~/Asset/js/jquery.js",
                                                                "~/Asset/js/bootstrap.min.js",
                                                                "~/Asset/js/jquery.scrollup.min.js",
                                                                "~/Asset/js/price-range.js",
                                                                "~/Asset/js/jquery.prettyPhoto.js",
                                                                "~/Asset/jquery.scrollUp.min.js",
                                                                "~/Asset/js/main.js",
                                                                "~/Asset/js/contact.js",
                                                                "~/Asset/js/gmaps.js"));
        }
    }
}