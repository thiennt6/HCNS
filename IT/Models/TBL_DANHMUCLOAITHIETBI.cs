//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_DANHMUCLOAITHIETBI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_DANHMUCLOAITHIETBI()
        {
            this.TBL_DANHMUCTHIETBI = new HashSet<TBL_DANHMUCTHIETBI>();
        }
    
        public string maloai { get; set; }
        public string tenthietbi { get; set; }
        public string vongdoi { get; set; }
        public string baohanh { get; set; }
        public string nhacungcap { get; set; }
        public string hang { get; set; }
        public string hopdongmua { get; set; }
        public string model { get; set; }
        public string lop { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_DANHMUCTHIETBI> TBL_DANHMUCTHIETBI { get; set; }
    }
}
