using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Text;
using System.Security.Cryptography;

namespace DoAn.Models
{
    public class User
    {
        public Nullable<int> id { get; set; }
        public bool daDangNhap { get; set; }
        public taiKhoan Account { get; set; }
        private onlineTradeEntities1 db;
        public User()
        {
            id = null;
            daDangNhap = false;
            db = new onlineTradeEntities1();
            Account = null;
        }              

        public bool Login(string tk, string mk)
        {
            List<taiKhoan> kt = db.taiKhoans.Where(x => x.duocSD && x.tenTK.Equals(tk) && x.matKhau.Equals(mk)).ToList();
            if (kt.Count <= 0) return false;
            else
            {
                this.id = kt.First().ID;
                this.Account = kt.First();
                daDangNhap = true;
                return true;
            }
        }

        public void Logout()
        {
            daDangNhap = false;
            Account = null;
        }
    }
}