using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
using PagedList;
using PagedList.Mvc;

namespace DoAn.Controllers
{
    public class ShopController : Controller
    {
        private onlineTradeEntities1 on;

        public ShopController()
        {
            on = new onlineTradeEntities1();
        }
        // GET: Shop
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            List<sanPham> sanPhams = on.sanPhams.Where(x=>x.dangSP).ToList();
            return View(sanPhams.ToPagedList(pageNum,12));
        }
        // Nội dung sản phẩm 
        public ActionResult Product(string id)
        {
            var sanPhams = on.sanPhams;
            ViewBag.sanPham = (from sp in sanPhams where sp.tenDuongDan.Equals(id) && sp.dangSP select sp).First();
            ViewBag.khuyenMai = (from sp in sanPhams where sp.giaKM < sp.giaBan && sp.dangSP select sp).Take(4);
            int loaisp = ViewBag.sanPham.maLoai;
            ViewBag.CungLoai = (from sp in sanPhams where sp.maLoai == loaisp && sp.dangSP select sp).Take(4);
            sanPham sanPham = ViewBag.sanPham;
            sanPham.luotXem++;
            on.SaveChanges();
            return View();
        }
        // Giở hàng 
        public ActionResult Cart()
        {
            List<GioHang> gioHangs = GioHang.getGioHang();
            return View(gioHangs);
        }
        // Thanh toán 
        public ActionResult Checkout()
        {
            List<GioHang> gioHangs = GioHang.getGioHang();
            if (gioHangs.Count <= 0)
                return RedirectToAction("Cart");
            User user = (User)Session.Contents["Account"];
            if (!user.daDangNhap)
                return Redirect(Url.Action("Login", "User"));
            return View();
        }
        // ajax sử dụng ajax để lấy dữ liệu tỉnh thành, quận huyện, phường xã
        [HttpPost]
        public ActionResult TinhThanh(int id, int type)
        {
            List<string> value = new List<string>();
            List<int> ID = new List<int>();
            if (type == 1)
            {
                foreach (QuanHuyen phuongXa in on.QuanHuyens.Where(x=>x.TinhThanh == id).OrderBy(x=>x.Name))
                {
                    value.Add(phuongXa.Name);
                    ID.Add(phuongXa.Id);
                }
            }else if (type == 2)
            {
                foreach (PhuongXa phuongXa in on.PhuongXas.Where(x=>x.QuanHuyen == id).OrderBy(x=>x.Name))
                {
                    value.Add(phuongXa.Name);
                    ID.Add(phuongXa.Id);
                }
            }
            return Json(new
            {
                Value = value.ToArray(),
                ID = ID.ToArray()
            }); 
        }
        [HttpPost]
        public ActionResult Checkout(donHang donHang)
        {
            User user = (User)Session.Contents["Account"];
            if (!user.daDangNhap)
                return Redirect(Url.Action("Login", "User"));
            List<GioHang> gioHangs = GioHang.getGioHang();
            if (gioHangs.Count <= 0)
                return RedirectToAction("Cart");
            using (DbContextTransaction transaction = on.Database.BeginTransaction())
            {
                try
                {
                    donHang.ngayDatHang = DateTime.Now;
                    donHang.khachHang = user.id ?? 0;
                    on.donHangs.Add(donHang);
                    on.SaveChanges();
                    // đếm số đơn hợp lệ được thêm vào
                    int count = 0;
                    foreach (GioHang gioHang in gioHangs)
                    {
                        // yêu cầu số lượng sản phẩm phải lớn hơn 0
                        if (gioHang.soLuon <= 0) continue;
                        chiTietDH chiTietDH = new chiTietDH() { maDH = donHang.ID, 
                            maSP = gioHang.sanPham.ID, 
                            soLuong = gioHang.soLuon, 
                            giaBan = gioHang.tongCong,
                        };
                        on.sanPhams.Where(x => x.ID == gioHang.sanPham.ID).First().soLuong-= gioHang.soLuon;
                        on.sanPhams.Where(x => x.ID == gioHang.sanPham.ID).First().luotMua += gioHang.soLuon;
                        on.chiTietDHs.Add(chiTietDH);
                        count++;
                    }
                    // nếu trong chi tiết đơn hàng không có nào hợp lên thì xóa cả đơn hàng, tránh đơn hàng rỗng
                    if (count <= 0)
                    {
                        transaction.Rollback();
                        return Redirect("/");
                    }                        
                    on.SaveChanges();
                    transaction.Commit();
                    Response.Cookies["GioHang"].Value = "";
                    Response.Cookies["GioHang"].Expires = DateTime.Now.AddDays(-1);
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.Message);
                    ViewBag.msg = "Lỗi không thể đặt hàng";
                    return View();
                }
                
            }

            return Redirect("/");
        }
        // sản phẩm theo thể loại 
        public ActionResult Category(string id, int? page)
        {
            int pageNum = page ?? 1;
            List<sanPham> sanPhams = on.sanPhams.Where(x => x.dangSP&&x.loaiSP.tenDuongDan==id).ToList();
            return View("Index",sanPhams.ToPagedList(pageNum,12));
        }
        // Tìm kiếm sản phẩm
        public ActionResult Search(string search, int? page)
        {
            if (page.Equals(null))
            {
                page = 1;
            }
            // loại bỏ dấu tiếng việt
            search = Class1.convertToUnSign3(search);
            string[] key = search.Split(' ');
            List<sanPham> sanPhams = new List<sanPham>();
            foreach (String k in key)
            {
                foreach (sanPham s in on.sanPhams)
                {
                    if (Class1.convertToUnSign3(s.tenSP).IndexOf(k)!=-1)
                        sanPhams.Add(s);
                }
            }
            ViewBag.search = search;
            int pageNum = page ?? 1;
            return View("Index",sanPhams.ToPagedList(pageNum,12));
            
        }
        // Quản lý đơn hàng của khách
        public ActionResult OrderManagement(int id)
        {
            donHang donHang = on.donHangs.Where(x => x.ID == id).First();
            return View(donHang);
        }
        [HttpPost]
        public ActionResult OrderManagement(int sao, int maSP, int maDH)
        {
            User user = (User)Session.Contents["Account"];
            if (!user.daDangNhap) return null;
            on.chiTietDHs.Where(x => x.maSP == maSP && x.maDH == maDH).First().danhGia = sao;
            on.SaveChanges();
            return Json(new { msg = "Đánh giá thành công" });
        }
        public ActionResult Comment(binhLuan binhLuan,int maSP)
        {
            User user = (User)Session.Contents["Account"];
            if (!user.daDangNhap) return Redirect(Url.Action("Login","User"));
            binhLuan.maTK = user.Account.ID;
            binhLuan.maSP = maSP;
            if (binhLuan.phanHoi != null && binhLuan.binhLuan1.IndexOf('@')!=-1)
            {
                string ten= binhLuan.binhLuan1.Split('@')[1];
                string tenPhanHoi = on.binhLuans.Where(x => x.ID == binhLuan.phanHoi).First().taiKhoan.HoTen;
                if (!ten.Equals(tenPhanHoi))
                    binhLuan.phanHoi = null;
                else
                {
                    ten = "<a href = ''><strong>"+ten+"</strong></a>";
                    binhLuan.binhLuan1 = ten + binhLuan.binhLuan1.Split('@')[2];
                }
            }
            if (binhLuan.binhLuan1.IndexOf('@') == -1)
                binhLuan.phanHoi = null;
            on.binhLuans.Add(binhLuan);
            on.SaveChanges();
            string tenDuongDan = on.sanPhams.Where(x => x.ID == binhLuan.maSP).First().tenDuongDan;
            return Redirect(Url.Action("Product", "Shop", routeValues: new { id = tenDuongDan }));
        }
        // lọc giá sản phẩm
        public ActionResult price_filter(string price, int? page)
        {
            string[] price1 = price.Split(',');
            int min = Convert.ToInt32(price1[0]);
            int max = Convert.ToInt32(price1[1]);
            int pageNum = page ?? 1;            
            List<sanPham> sanPhams = on.sanPhams.Where(x => x.dangSP && x.giaBan >=min && x.giaBan <=max).ToList();
            ViewBag.price = price;
            return View("Index",sanPhams.ToPagedList(pageNum, 12));
        }
    }
}