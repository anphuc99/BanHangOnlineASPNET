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
    
    public partial class binhLuan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public binhLuan()
        {
            this.binhLuan11 = new HashSet<binhLuan>();
        }
    
        public int ID { get; set; }
        public int maSP { get; set; }
        public int maTK { get; set; }
        public string binhLuan1 { get; set; }
        public Nullable<int> phanHoi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<binhLuan> binhLuan11 { get; set; }
        public virtual binhLuan binhLuan2 { get; set; }
        public virtual sanPham sanPham { get; set; }
        public virtual taiKhoan taiKhoan { get; set; }
    }
}
