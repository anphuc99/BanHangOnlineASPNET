using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    public class GioHang
    {
        public sanPham sanPham { get; set; }
        public int soLuon { get; set; }  
        public GioHang(sanPham sanPham, int soLuon)
        {
            this.sanPham = sanPham;
            this.soLuon = soLuon;
        }
        public int tongCong
        {
            get
            {
                return (soLuon * sanPham.giaKM?? sanPham.giaBan);
            }
        }
        public static List<GioHang> getGioHang()
        {
            List<GioHang> gioHangs = new List<GioHang>();
            if (!HttpContext.Current.Request.Cookies.AllKeys.Contains("GioHang") ) return gioHangs;
            string cookie = HttpContext.Current.Request.Cookies["GioHang"].Value;
            if (String.IsNullOrEmpty(cookie) || cookie.Equals("")) return gioHangs;
            string[] id_sl = cookie.Split(',');
            foreach (string i in id_sl)
            {
                string[] gh = i.Split('|');
                onlineTradeEntities1 db = new onlineTradeEntities1();
                try
                {
                    sanPham sanPham = db.sanPhams.ToList().Where(x => x.ID == Convert.ToInt32(gh[0])).First();
                    GioHang gioHang = new GioHang(sanPham,Convert.ToInt32(gh[1]));
                    gioHangs.Add(gioHang);
                }
                catch
                {
                    continue;
                }
                
            }
            return gioHangs;
        }

        public static int tongHang()
        {
            List<GioHang> gioHangs = GioHang.getGioHang();
            int tc = 0;
            foreach(GioHang gh in gioHangs)
            {
                tc += gh.soLuon*(gh.sanPham.giaKM?? gh.sanPham.giaBan);
            }
            return tc;
        }        
    }
}