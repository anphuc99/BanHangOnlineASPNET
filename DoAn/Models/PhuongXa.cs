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
    
    public partial class PhuongXa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhuongXa()
        {
            this.donHangs = new HashSet<donHang>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int QuanHuyen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<donHang> donHangs { get; set; }
        public virtual QuanHuyen QuanHuyen1 { get; set; }
    }
}
