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
    
    public partial class TBL_DANHSACHLOAIPHEP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_DANHSACHLOAIPHEP()
        {
            this.TBL_DANHSACHNGAYNGHI = new HashSet<TBL_DANHSACHNGAYNGHI>();
        }
    
        public int loaiphep { get; set; }
        public string tenphep { get; set; }
        public Nullable<int> tongngaynghi { get; set; }
        public Nullable<int> sonam { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_DANHSACHNGAYNGHI> TBL_DANHSACHNGAYNGHI { get; set; }
    }
}
