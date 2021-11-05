using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;


namespace DoAn.Controllers
{
   [CustomAuthor(Roles = "admin")]
    public class AdminController : Controller
    {
        // đối tượng databasse của entity
        private onlineTradeEntities1 db;
        public AdminController()
        {
            db = new onlineTradeEntities1();            
        }
        // GET: Admin
        public ActionResult Index()
        {
            onlineTradeEntities1 on = new onlineTradeEntities1();
            IEnumerable<TinhThanh> ta = on.TinhThanhs;         
            ViewBag.tinh = from t in ta where t.Id == 1 select t;
            ViewBag.NewOrder = db.donHangs.Where(x => x.maVanChuyen == null && x.thanhCong == null).ToList().Count;
            ViewBag.DonThanhCong = db.donHangs.Where(x => x.thanhCong == true).ToList().Count;
            ViewBag.SoNguoiDung = db.taiKhoans.ToList().Count;
            List<IGrouping<DateTime, donHang>> donTheoTuan = db.donHangs.ToList().OrderByDescending(x => x.ngayDatHang).GroupBy(x => x.ngayDatHang.Date).Take(7).OrderBy(x=>x.Key).ToList();
            ViewBag.DonHangTrongTuan = donTheoTuan;
            return View();
        }
        // Product list page
        public ActionResult Product_List(int? page)
        {
            int pageNum = (page ?? 1);
            onlineTradeEntities1 on = new onlineTradeEntities1();
            var sanPham = on.sanPhams;
            List<sanPham> sanPhams = (from sp in sanPham orderby sp.ID ascending select sp).ToList();
            return View(sanPhams.ToPagedList(pageNum,10));
        }
        // Create Product 
        public ActionResult Create_Product()
        {
            ViewBag.sanPham = new List<sanPham>();
            return View();
        }
        // Insert Product 
        [HttpPost, ValidateInput(false)]
        public ActionResult Create_Product(sanPham sp, string tva, bool preview = false)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(sp.tenSP)||sp.tenSP.Equals(""))
            {
                ModelState["tenSP"].Errors.Add("Tên sản phẩm không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(sp.tenDuongDan) || sp.tenDuongDan.Equals(""))
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(sp.ndSanPham) || sp.ndSanPham.Equals(""))
            {
                ModelState["ndSanPham"].Errors.Add("Nội dung sản phẩm không được bỏ trống");
                luu = false;
            }
            List<sanPham> sanPhams = db.sanPhams.Where(x => x.tenDuongDan == sp.tenDuongDan).ToList();
            if (sanPhams.Count > 0)
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (sp.giaBan < sp.giaKM)
            {
                ModelState["giaBan"].Errors.Add("Giá khuyển mãi không được lớn hơn giá bán");
                luu = false;
            }
            if (sp.soLuong < 0)
            {
                ModelState["soLuong"].Errors.Add("Số lượng không nhỏ hơn 0");
                luu = false;
            }
            if (sp.anhBia == null)
            {
                ModelState["anhBia"].Errors.Add("Ảnh bìa không được để trống nhỏ hơn 0");
                luu = false;
            }
            if (!luu)
            {
                ViewBag.sanPham = new List<sanPham>();
                return View();
            }
                
            if (String.IsNullOrEmpty(sp.dvt) || sp.dvt.Equals(""))
                sp.dvt = "Cái";
            sp.ngayDangSP = DateTime.Now;
            sp.ndSanPham = String.IsNullOrEmpty(sp.ndSanPham)?null:sp.ndSanPham.Replace("\n", "").Replace("\r", "");
            sp.tomTat = String.IsNullOrEmpty(sp.tomTat)?null:sp.tomTat.Replace("\n", "").Replace("\r", "");
            sp.khuyenMai = String.IsNullOrEmpty(sp.khuyenMai)?null:sp.khuyenMai.Replace("\n", "").Replace("\r", "");
            sp.luotMua = 0;
            sp.luotXem = 0;
            db.sanPhams.Add(sp);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (tva != null && tva != "")
            {
                string[] tva2 = tva.Split(',');
                foreach (string t in tva2)
                {
                    int idAnh = Convert.ToInt32(t);
                    db.Database.ExecuteSqlCommand("Insert into thuVienAnhSP (maAnh,maSP) values (" + idAnh + "," + sp.ID + ")");
                }
            }
            if (preview)
                return Preview(sp.ID);
            return RedirectToAction("Product_List");
        }
        // Edit procduct
        public ActionResult Edit_Product(int id)
        {
            onlineTradeEntities1 on = new onlineTradeEntities1();
            var sanPhams = on.sanPhams;
            ViewBag.sanPham = (from sp in sanPhams where sp.ID == id select sp).ToList();
            return View("Create_Product");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit_Product(sanPham sp, string tva, bool preview = false)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(sp.tenSP) || sp.tenSP.Equals(""))
            {
                ModelState["tenSP"].Errors.Add("Tên sản phẩm không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(sp.tenDuongDan) || sp.tenDuongDan.Equals(""))
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(sp.ndSanPham) || sp.ndSanPham.Equals(""))
            {
                ModelState["ndSanPham"].Errors.Add("Nội dung sản phẩm không được bỏ trống");
                luu = false;
            }
            List<sanPham> sanPhams = db.sanPhams.Where(x => x.tenDuongDan == sp.tenDuongDan && x.ID != sp.ID ).ToList();
            if (sanPhams.Count > 0)
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (sp.giaBan < sp.giaKM)
            {
                ModelState["giaBan"].Errors.Add("Giá khuyển mãi không được lớn hơn giá bán");
                luu = false;
            }
            if (sp.soLuong < 0)
            {
                ModelState["soLuong"].Errors.Add("Số lượng không nhỏ hơn 0");
                luu = false;
            }
            if (!luu)
            {
                ViewBag.sanPham = new List<sanPham>();                
                return View("Create_Product");
            }
                
            sanPham sp2 = db.sanPhams.Where(x => x.ID == sp.ID).First();
            sp2.maLoai = sp.maLoai;
            sp2.tenSP = sp.tenSP;
            sp2.tenDuongDan = sp.tenDuongDan;            
            sp2.giaBan = sp.giaBan;
            sp2.giaKM = sp.giaKM;
            sp2.dvt = sp.dvt;            
            sp2.soLuong = sp.soLuong;
            sp2.anhBia = sp.anhBia;            
            sp2.dangSP = sp.dangSP;
            sp2.ndSanPham = String.IsNullOrEmpty(sp.ndSanPham)?null:sp.ndSanPham.Replace("\n","").Replace("\r","");
            sp2.khuyenMai = String.IsNullOrEmpty(sp.khuyenMai)?null:sp.khuyenMai.Replace("\n","").Replace("\r","");
            sp2.tomTat = String.IsNullOrEmpty(sp.tomTat)?null:sp.tomTat.Replace("\n","").Replace("\r","");
            try
            {
                db.SaveChanges();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // xóa thư viện ảnh
            db.Database.ExecuteSqlCommand("delete thuVienAnhSP where maSP = " + sp.ID);  
            // thêm lại thư viện ảnh
            if (tva!=null && tva != "")
            {
                string[] tva2 = tva.Split(',');
                foreach (string t in tva2)
                {
                    int idAnh = Convert.ToInt32(t);
                    db.Database.ExecuteSqlCommand("Insert into thuVienAnhSP (maAnh,maSP) values (" + idAnh + "," + sp2.ID + ")");
                }
            }
            if (preview)
                return Preview(sp.ID);
            return RedirectToAction("Product_List");
        }
        // Preview Product
        private ActionResult Preview(int id)
        {
            onlineTradeEntities1 db = new onlineTradeEntities1();
            ViewBag.sanPham = db.sanPhams.Where(x => x.ID == id).First();                
            ViewBag.khuyenMai = (from sp2 in db.sanPhams where sp2.giaKM < sp2.giaBan && sp2.dangSP select sp2).Take(4);
            int loaisp = ViewBag.sanPham.maLoai;
            ViewBag.CungLoai = (from sp2 in db.sanPhams where sp2.maLoai == loaisp && sp2.dangSP select sp2).Take(4);
            return View("~/Views/Shop/Product.cshtml");
        }
        // Remove Product
        public ActionResult Remove_Product(int id)
        {
            sanPham sp = db.sanPhams.Where(x => x.ID == id).First();
            try
            {
                db.sanPhams.Remove(sp);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Redirect(Url.Action("Product_List", "Admin"));
        }
        // Search Product
        public ActionResult Search_Product(string search,int? page)
        {
            int pageNum = (page ?? 1);
            List<sanPham> sanPhams = db.sanPhams.Where(x => DbFunctions.Like(x.tenSP, "%" + search + "%")).ToList();
            ViewBag.search = search; 
            return View("Product_List",sanPhams.ToPagedList(pageNum,10));
        }                
        // Category 
        public ActionResult Category()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Category(loaiSP loaiSP)
        {
            bool luu = true;
            if (String.IsNullOrEmpty(loaiSP.tenLoai))
            {
                ModelState["tenLoai"].Errors.Add("Tên thể loại không được để trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(loaiSP.tenDuongDan))
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn không được để trống");
                luu = false;
            }
            if (db.loaiSPs.Where(x => x.tenDuongDan == loaiSP.tenDuongDan).ToList().Count > 0)
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            // lưu đữ liệu
            if (luu)
            {
                try
                {
                    db.loaiSPs.Add(loaiSP);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return View();
        }
        public void Edit_Category(loaiSP loaiSP)
        {
            loaiSP loaiSP1 = db.loaiSPs.Where(x => x.ID == loaiSP.ID).First();
            loaiSP1.tenLoai = loaiSP.tenLoai;
            loaiSP1.tenDuongDan = loaiSP.tenDuongDan;
            loaiSP1.cha = loaiSP.cha;
            loaiSP1.ghiChu = loaiSP.ghiChu;
            try
            {
                db.SaveChanges();
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Response.Redirect(Url.Action("Category","Admin"));
        }
        public void Delete_Category(int id)
        {
            loaiSP loai = db.loaiSPs.Where(x => x.ID == id).First();
            try
            {
                db.loaiSPs.Remove(loai);
                db.SaveChanges();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Response.Redirect(Url.Action("Category", "Admin"));
        }       
        // Thực hiện ajax để lấy dữ liệu cho chỉnh sửa thể loại 
        [HttpPost]
        public ActionResult Ajax_Category(int id)
        {
            loaiSP loaiSP = db.loaiSPs.Where(x => x.ID == id).First();
            try
            {
                return Json(new
                {
                    TenLoai = loaiSP.tenLoai,
                    TenDuongDan = loaiSP.tenDuongDan,
                    Cha = loaiSP.cha, 
                    GhiChu = loaiSP.ghiChu
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ActionResult Media(string msg)
        {
            try
            {
                string viTri = Server.MapPath("~/Asset/Image");
                DirectoryInfo directory = new DirectoryInfo(viTri);
                DirectoryInfo[] directories = directory.GetDirectories();
                List<string> folder = new List<string>();
                ViewBag.msg = msg;
                foreach(DirectoryInfo s in directories)
                {
                    folder.Add(s.Name);
                }
                ViewBag.folder = folder;
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return View();
        }
        // Danh sách Media Hình ảnh
        public ActionResult Media_Images(string folder)
        {
            List<anh> a = db.anhs.ToList();
            ViewBag.anhs = a.Where(x=> x.Url.Split('/')[3] == folder).ToList();
            ViewBag.folder = folder;
            return View();
        }
        // Tạo Folder
        public void Media_Create_Folder(string folderName)
        {
            try
            {
                string viTri = Server.MapPath("~/Asset/Image/"+folderName);
                DirectoryInfo directory = new DirectoryInfo(viTri);
                if (!directory.Exists)
                    directory.Create();
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Response.Redirect(Url.Action("Media", "Admin"));

        }
        // Đổi tên Folder
        public void Media_Rename_Folder(string folderOldName, string folderNewName)
        {
            try
            {
                string viTri = Server.MapPath("~/Asset/Image/" + folderOldName);
                string vitri2 = Server.MapPath("~/Asset/Image/" + folderNewName);
                DirectoryInfo directory = new DirectoryInfo(viTri);
                FileInfo[] files = directory.GetFiles();
                DirectoryInfo directory1 = new DirectoryInfo(vitri2);
                if (directory1.Exists)
                {
                    Response.Redirect(Url.Action("Media", "Admin"));
                    return;
                }                    
                directory1.Create();
                foreach (FileInfo file in files)
                {
                    file.MoveTo(vitri2+"/"+file.Name);
                }
                directory.Delete();
                db.rename_path_anh(folderOldName, folderNewName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Response.Redirect(Url.Action("Media", "Admin"));
        }
        // xóa Folder 
        public void Media_Delete_Folder(string folderDeleteName)
        {
            try
            {
                string viTri = Server.MapPath("~/Asset/Image/" + folderDeleteName);
                DirectoryInfo directory = new DirectoryInfo(viTri);
                db.remov_file_anh(folderDeleteName);
                directory.Delete(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Response.Redirect(Url.Action("Media", "Admin", routeValues: new {msg = "Xóa không thành công, thư mục có thể chứa hình ảnh đang được sử dụng" }));
                return;
            }
            Response.Redirect(Url.Action("Media", "Admin"));
        }
        // thêm media
        public ActionResult Media_Add()
        {
            try
            {
                string viTri = Server.MapPath("~/Asset/Image");
                DirectoryInfo directory = new DirectoryInfo(viTri);
                DirectoryInfo[] directories = directory.GetDirectories();
                List<string> folder = new List<string>();
                foreach (DirectoryInfo s in directories)
                {
                    folder.Add(s.Name);
                }
                ViewBag.folder = folder;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Media_Add(string folder, HttpPostedFileBase[] anh)
        {
            string path = Server.MapPath("~/Asset/Image/"+folder+"/");
            foreach (HttpPostedFileBase image in anh)
            {
                if (image != null)
                {
                    string imageName = Path.GetFileName(image.FileName);
                    string PathSave = Path.Combine(path + imageName);
                    image.SaveAs(PathSave);
                    db.anhs.Add(new anh() { Url = "/Asset/Image/" + folder + "/" + imageName });
                    try
                    {
                        db.SaveChanges();
                    }catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return Redirect(Url.Action("Media", "Admin"));
        }
        // Xóa Media 
        [HttpPost]
        public ActionResult Delete_Image(string anhs, string folder)
        {
            string[] anh = anhs.Split(',');
            foreach (string a in anh)
            {
                int idAnh = Convert.ToInt32(a);
                anh anh2 = db.anhs.Where(x => x.ID == idAnh).First();
                string viTri = Server.MapPath("~"+anh2.Url);
                FileInfo file = new FileInfo(viTri);
                try
                {
                    db.anhs.Remove(anh2);
                    db.SaveChanges();
                    file.Delete();
                }
                catch 
                {
                    ViewBag.msg = "Xóa không thành công, hình ảnh có thể đang được sử dụng";
                    List<anh> a2 = db.anhs.ToList();
                    ViewBag.anhs = a2.Where(x => x.Url.Split('/')[3] == folder).ToList();
                    ViewBag.folder = folder;
                    return View("Media_Images");
                }
            }
            return RedirectToAction("Media");
        }
        // Danh sách bài viết
        public ActionResult Blog_List(int? page)
        {            
            int pageNum = (page ?? 1);
            List<baiViet> baiViets = db.baiViets.ToList();
            return View(baiViets.ToPagedList(pageNum,10));
        }
        // thêm bài viết 
        public ActionResult Create_Blog()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create_Blog(baiViet baiViet, bool preview = false)
        {
            List<baiViet> baiViets = db.baiViets.Where(x => x.tenDuongDan == baiViet.tenDuongDan).ToList();
            bool luu = true;
            if (baiViets.Count > 0)
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.tenBV) || baiViet.tenBV.Equals(""))
            {
                ModelState["tenBV"].Errors.Add("Tên bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.noiDungBV) || baiViet.noiDungBV.Equals(""))
            {
                ModelState["noiDungBV"].Errors.Add("Nội dung bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.tenDuongDan) || baiViet.tenDuongDan.Equals(""))
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.tomTat) || baiViet.tomTat.Equals(""))
            {
                ModelState["tomTat"].Errors.Add("Tóm tắt không được bỏ trống");
                luu = false;
            }
            if (baiViet.anhBia.Equals(null))
            {
                ModelState["tomTat"].Errors.Add("Ảnh bìa không được bỏ trống");
                luu = false;
            }
            if (luu)
            {
                // xóa xuống dòng
                baiViet.noiDungBV = baiViet.noiDungBV.Replace("\n", "").Replace("\r", "");
                baiViet.tomTat = baiViet.tomTat.Replace("\n", "").Replace("\r", "");
                baiViet.ngayDang = DateTime.Now;
                baiViet.ngayCapNhat = DateTime.Now;
                baiViet.soLanDoc = 0;
                db.baiViets.Add(baiViet);
                try
                {
                    db.SaveChanges();
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }   
            }
            if (preview)
                return Preview(baiViet);
            return View();
        }
        public ActionResult Edit_Blog(int id)
        {
            baiViet baiViets = db.baiViets.Where(x => x.ID == id).First();
            return View("Create_Blog",baiViets);
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult Edit_Blog (baiViet baiViet, bool preview = false)
        {            
            List<baiViet> baiViets = db.baiViets.Where(x => x.tenDuongDan == baiViet.tenDuongDan && x.ID != baiViet.ID).ToList();
            bool luu = true;
            if (baiViets.Count > 0)
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn đã tồn tại");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.tenBV) || baiViet.tenBV.Equals(""))
            {
                ModelState["tenBV"].Errors.Add("Tên bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.noiDungBV) || baiViet.noiDungBV.Equals(""))
            {
                ModelState["noiDungBV"].Errors.Add("Nội dung bài viết không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.tenDuongDan) || baiViet.tenDuongDan.Equals(""))
            {
                ModelState["tenDuongDan"].Errors.Add("Tên đường dẫn không được bỏ trống");
                luu = false;
            }
            if (String.IsNullOrEmpty(baiViet.tomTat) || baiViet.tomTat.Equals(""))
            {
                ModelState["tomTat"].Errors.Add("Tóm tắt không được bỏ trống");
                luu = false;
            }
            if (baiViet.anhBia.Equals(null))
            {
                ModelState["tomTat"].Errors.Add("Ảnh bìa không được bỏ trống");
                luu = false;
            }
            if (luu)
            {
                // xóa xuống dòng
                baiViet baiViet2 = db.baiViets.Where(x => x.ID == baiViet.ID).First();
                baiViet2.tenBV = baiViet.tenBV;
                baiViet2.tenDuongDan = baiViet.tenDuongDan;
                baiViet2.noiDungBV = baiViet.noiDungBV.Replace("\n", "").Replace("\r", "");
                baiViet2.tomTat = baiViet.tomTat.Replace("\n", "").Replace("\r", "");
                baiViet2.anhBia = baiViet.anhBia;
                baiViet2.hienThi = baiViet.hienThi;
                baiViet2.ngayCapNhat = DateTime.Now;
                try
                {
                    db.SaveChanges();
                    if (preview)
                        return Preview(baiViet);
                    return RedirectToAction("Blog_List");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return View("Create_Blog");
        }

        // xóa bài viết
        public ActionResult Remove_Blog(int id)
        {
            baiViet baiViet = db.baiViets.Where(x => x.ID == id).First();
            db.baiViets.Remove(baiViet);
            db.SaveChanges();
            return RedirectToAction("Blog_List");
        }
        // Search Blog
        public ActionResult Search_Blog(string search, int? page)
        {
            int pageNum = (page ?? 1);
            List<baiViet> baiViets  = db.baiViets.Where(x => DbFunctions.Like(x.tenBV, "%" + search + "%")).ToList();
            ViewBag.search = search;
            return View("Blog_List", baiViets.ToPagedList(pageNum, 10));
        }
        private ActionResult Preview(baiViet baiViet)
        {
            return View("~/Views/Blog/Single.cshtml",baiViet);
        }
        // Quản lý user 
        public ActionResult UserManagement(int? page)
        {
            int pageNum = (page ?? 1);
            List<taiKhoan> taiKhoans = db.taiKhoans.ToList();
            return View(taiKhoans.ToPagedList(pageNum,10));
        }
        // cài đặt trạng thái người dùng
        public ActionResult SettingUser(int id,int type)
        {
            // type: 1 khóa tài khoản, 2 thăng làm admin, 3 xóa tài khoản
            taiKhoan taiKhoan = db.taiKhoans.Where(x => x.ID == id).First();
            if (taiKhoan.tenTK == "admin")
                return RedirectToAction("UserManagement");
            if (type == 1)
            {
                taiKhoan.duocSD = taiKhoan.duocSD?false:true;
            }else if (type == 2)
            {
                taiKhoan.vaiTro = taiKhoan.vaiTro == "admin"?"user":"admin";
            }else if (type == 3)
            {
                db.taiKhoans.Remove(taiKhoan);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("UserManagement");
        }
        // thông tin tài khoản
        public ActionResult UserProfile(int id)
        {
            taiKhoan taiKhoan = db.taiKhoans.Where(x => x.ID == id).First();
            return View(taiKhoan);
        }
        // sửa thông tin
        [HttpPost]
        public ActionResult UserProfile(taiKhoan taiKhoan, bool CapLaiMK)
        {
            taiKhoan taiKhoan1 = db.taiKhoans.Where(x => x.ID == taiKhoan.ID).First();
            User user = (User)Session.Contents["Account"];
            // tài khoản admin sẽ không được phép ai thay đổi trừ chính nó
            if (taiKhoan1.tenTK == "admin")
                return View(taiKhoan1);
            bool luu = true;
            List<taiKhoan> kt = db.taiKhoans.Where(x => x.email == taiKhoan.email && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["email"].Errors.Add("Email đã tồn tại");
                luu = false;
            }
            kt = db.taiKhoans.Where(x => x.SDT == taiKhoan.SDT && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                luu = false;
            }
            if (!luu)
                return View(taiKhoan1);
            taiKhoan1.email = taiKhoan.email;
            taiKhoan1.SDT = taiKhoan.SDT;
            taiKhoan1.ghiChu = taiKhoan.ghiChu;
            try
            {
                if (CapLaiMK)
                    taiKhoan1.matKhau = Class1.MD5Hash("1111");
                db.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(taiKhoan1);
        }
        // Quản lý đơn hàng
        public ActionResult OrderManagement()
        {
            List<donHang> donHangs = db.donHangs.ToList();
            return View(donHangs);
        }
        // Cài đặt trạng thái vận chuyển
        public ActionResult SettingOrder(int id, int type, string dvVanChuyen, string maVanChuyen, int? phiShip, string ghiChu )
        {
            // type: 1 hủy đơn, 2 vận chuyển, 3 chuyển trạng thái giao thành công, 4 ghi chú
            donHang donHang = db.donHangs.Where(x => x.ID == id).First();
            if (type == 1)
            {
                donHang.thanhCong = false;
                donHang.ghiChuShop = ghiChu;
            }else if (type == 2)
            {
                donHang.dvVanChuyen = dvVanChuyen;
                donHang.maVanChuyen = maVanChuyen;
                donHang.phiShip = phiShip;
            }else if (type == 3)
            {
                donHang.ngayGiaoHang = DateTime.Now;
                donHang.thanhCong = true;
            }else if (type == 4)
            {
                donHang.ghiChuShop = ghiChu;
            }
            try
            {
                db.SaveChanges();
            }catch(DbEntityValidationException e)
            {
                Console.WriteLine(e.EntityValidationErrors);
            }
            
            return RedirectToAction("OrderManagement");
        }
        // Chi tiết đơn hàng
        public ActionResult DetailOrder(int id)
        {
            List<chiTietDH> chiTietDHs = db.chiTietDHs.Where(x => x.maDH == id).ToList();
            ViewBag.donHang = db.donHangs.Where(x => x.ID == id).First();
            return View(chiTietDHs);
        }
        // thông tin admin (tài khoản admin cá nhân)
        public ActionResult ProfileAdmin()
        {
            User user = (User) Session.Contents["Account"];
            return View(user.Account);
        }
        [HttpPost]
        public ActionResult ProfileAdmin(taiKhoan taiKhoan, string xnMK)
        {
            taiKhoan taiKhoan1 = db.taiKhoans.Where(x => x.ID == taiKhoan.ID).First();
            if (!taiKhoan.matKhau.Equals(xnMK))
            {
                ViewBag.xnMK = "Xác nhận mật khẩu không chính xác";
                return View(taiKhoan1);
            }
            bool luu = true;
            List<taiKhoan> kt = db.taiKhoans.Where(x => x.email == taiKhoan.email && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["email"].Errors.Add("Email đã tồn tại");
                luu = false;
            }
            kt = db.taiKhoans.Where(x => x.SDT == taiKhoan.SDT && x.ID != taiKhoan.ID).ToList();
            if (kt.Count > 0)
            {
                ModelState["SDT"].Errors.Add("Số điện thoại đã tồn tại");
                luu = false;
            }
            if (!luu)
                return View(taiKhoan1);
            taiKhoan1.email = taiKhoan.email;
            taiKhoan1.SDT = taiKhoan.SDT;
            taiKhoan1.ghiChu = taiKhoan.ghiChu;
            taiKhoan1.matKhau = Class1.MD5Hash(taiKhoan.matKhau);
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