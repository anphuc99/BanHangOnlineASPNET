using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DoAn.Models;
namespace DoAn.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private onlineTradeEntities1 db;
        public UserController()
        {
            db = new onlineTradeEntities1();
        }
        
        public ActionResult Login()
        {
            User user = (User)Session.Contents["Account"];
            if (user.daDangNhap)
            {
                Response.Redirect(Url.Action("Index", "Home"));
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            User user = (User)Session.Contents["Account"];
            if (user.daDangNhap)
            {
                Response.Redirect(Url.Action("Index", "Home"));
            }
            onlineTradeEntities1 db = new onlineTradeEntities1();
            if (user.Login(TaiKhoan, Class1.MD5Hash(MatKhau)))
            {
                string u = user.Account.vaiTro;
                AuthorizeAttribute authorize = new AuthorizeAttribute();
                authorize.Roles = "admin";
                Response.Redirect(Url.Action("Index", "Home"));
            }else
                ViewBag.msg = "Tên tài khoản hoặc mật khẩu không chính xác";
            bool dcSD = db.taiKhoans.Where(x => x.tenTK == TaiKhoan).First().duocSD;
            if (!dcSD)
            {
                ViewBag.msg = "Tài khoản này đã bị khóa";
                return View();
            }
            return View();
        }
        public void Logout()
        {
            User user = (User)Session.Contents["Account"];
            user.Logout();
            Response.Redirect("/");
        }
        [HttpPost]
        public ActionResult Register(taiKhoan kh, string xnMatKhau)
        {
            if (!xnMatKhau.Equals(kh.matKhau))
            {
                ViewData["LoiMK"] = "Xác thực mật khẩu không chính xác";
                return View("Login");
            }
            List<taiKhoan> kt = db.taiKhoans.Where(x => x.tenTK == kh.tenTK).ToList();
            bool luu = true;
            if (kt.Count > 0)
            {
                ModelState["tenTK"].Errors.Add("Tên tài khoản đã tồn tại");
                luu = false;
            }

            kt = db.taiKhoans.Where(x => x.email == kh.email).ToList();
            if (kt.Count > 0)
            {
                ModelState["email"].Errors.Add("Email đã tồn tại");
                luu = false;
            }
            kt = db.taiKhoans.Where(x => x.SDT == kh.SDT).ToList();
            if (kt.Count > 0)
            {
                ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                luu = false;
            }
            if (luu)
            {
                try
                {
                    kh.matKhau = Class1.MD5Hash(kh.matKhau);
                    kh.duocSD = true;
                    kh.ngayCap = DateTime.Now;
                    kh.vaiTro = "user";
                    db.taiKhoans.Add(kh);
                    db.SaveChanges();
                    User user = (User)Session.Contents["Account"];
                    user.Login(kh.tenTK, kh.matKhau);
                    Response.Redirect(Url.Action("Index", "Home"));
                }
                catch (Exception e)
                {
                    ViewData["LoiMK"] = e.Message;
                }

            }
            return View("Login");
        }
        public ActionResult ProfileUser()
        {
            User user = (User)Session.Contents["Account"];
            return View(user.Account);
        }
        [HttpPost]
        public ActionResult ProfileUser(int id,string MatKhau, string xnMK)
        {
            taiKhoan taiKhoan1 = db.taiKhoans.Where(x => x.ID == id).First();
            if (!MatKhau.Equals(xnMK))
            {
                ViewBag.xnMK = "Xác nhận mật khẩu không chính xác";
                return View(taiKhoan1);
            }            
            taiKhoan1.matKhau = Class1.MD5Hash(MatKhau);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(taiKhoan1);
        }
    }
}