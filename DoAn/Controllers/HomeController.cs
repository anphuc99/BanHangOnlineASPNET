using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            onlineTradeEntities1 on = new onlineTradeEntities1();
            var sanPhams = on.sanPhams;
            var loaiSPs = on.loaiSPs;
            ViewBag.sanPhams = (from sp in sanPhams where sp.dangSP == true orderby sp.ngayDangSP descending select sp).Take(6);
            ViewBag.loaiSPs = loaiSPs;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}