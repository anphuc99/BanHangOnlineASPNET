using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace DoAn.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundle)
        {
            // Add Style to bundle
            bundle.Add(new StyleBundle("~/Admincss/css").Include("~/Asset/Ad/plugins/fontawesome-free/css/all.min.css",
                                                               "~/Asset/Ad/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                                                               "~/Asset/Ad/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                                                               "~/Asset/Ad/plugins/jqvmap/jqvmap.min.css",
                                                               "~/Asset/Ad/dist/css/adminlte.min.css",
                                                               "~/Asset/Ad/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                                                               "~/Asset/Ad/plugins/daterangepicker/daterangepicker.css"));
            // Add Script to bundle 
            bundle.Add(new ScriptBundle("~/Adminjs/js").Include("~/Asset/Ad/plugins/jquery/jquery.min.js",
                                                              "~/Asset/Ad/plugins/jquery-ui/jquery-ui.min.js",
                                                              "~/Asset/Ad/plugins/bootstrap/js/bootstrap.bundle.min.js",
                                                              "~/Asset/Ad/plugins/chart.js/Chart.min.js",
                                                              "~/Asset/Ad/plugins/sparklines/sparkline.js",
                                                              "~/Asset/Ad/plugins/jqvmap/jquery.vmap.min.js",
                                                              "~/Asset/Ad/plugins/jqvmap/maps/jquery.vmap.usa.js",
                                                              "~/Asset/Ad/plugins/jquery-knob/jquery.knob.min.js",
                                                              "~/Asset/Ad/plugins/moment/moment.min.js",
                                                              "~/Asset/Ad/plugins/daterangepicker/daterangepicker.js",
                                                              "~/Asset/Ad/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                                                              "~/Asset/Ad/plugins/summernote/summernote-bs4.min.js",
                                                              "~/Asset/Ad/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                                                              "~/Asset/Ad/dist/js/adminlte.js",
                                                              "~/Asset/Ad/dist/js/demo.js",
                                                              "~/Asset/Ad/dist/js/pages/dashboard.js"));
        }
    }
}