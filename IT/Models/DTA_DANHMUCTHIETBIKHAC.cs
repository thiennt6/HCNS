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
    
    public partial class DTA_DANHMUCTHIETBIKHAC
    {
        public string maphong { get; set; }
        public string loai { get; set; }
        public string ip { get; set; }
        public int soluong { get; set; }
        public Nullable<int> namsudung { get; set; }
        public string tenmay { get; set; }
        public string taikhoan { get; set; }
        public int id { get; set; }
    
        public virtual TBL_DANHMUCPHONGBAN TBL_DANHMUCPHONGBAN { get; set; }
    }
}
