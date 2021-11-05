using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DoAn.Models
{
    public class Class1
    {
        private onlineTradeEntities1 on;
        public Class1()
        {
            this.on = new onlineTradeEntities1();
        }
        public IEnumerable<loaiSP> LoaiSPCha()
        {
            var loaiSPs = on.loaiSPs;
            var loaiSPCha = from lsp in loaiSPs where lsp.cha.Equals(null) select lsp;
            return loaiSPCha;
        }
        // hàm xóa dấu tiếng việt
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }
        // hàm mã hóa MD5
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}