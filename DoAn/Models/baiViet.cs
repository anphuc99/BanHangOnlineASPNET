//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAn.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class baiViet
    {
        public int ID { get; set; }
        public string tenBV { get; set; }
        public string tenDuongDan { get; set; }
        public System.DateTime ngayDang { get; set; }
        public System.DateTime ngayCapNhat { get; set; }
        public string tomTat { get; set; }
        public int anhBia { get; set; }
        public string noiDungBV { get; set; }
        public bool hienThi { get; set; }
        public Nullable<int> soLanDoc { get; set; }
    
        public virtual anh anh { get; set; }
    }
}