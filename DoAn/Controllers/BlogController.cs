using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAn.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            onlineTradeEntities1 on = new onlineTradeEntities1();
            List<baiViet> baiViets = on.baiViets.Where(x => x.hienThi).OrderByDescending(x => x.ngayDang).ToList();
            return View(baiViets.ToPagedList(pageNum,3));
        }
        // Thông tin bài viết 
        public ActionResult Single(string id)
        {
            onlineTradeEntities1 on = new onlineTradeEntities1();
            var baiViets = on.baiViets;
            baiViet baiViet = (from bv in baiViets where bv.tenDuongDan == id && bv.hienThi select bv).First();
            baiViet.soLanDoc++;
            on.SaveChanges();
            return View(baiViet);
        }
    }
}