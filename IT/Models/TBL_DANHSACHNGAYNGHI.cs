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
    
    public partial class TBL_DANHSACHNGAYNGHI
    {
        public int Id { get; set; }
        public string taikhoan { get; set; }
        public System.DateTime tungay { get; set; }
        public int buoitungay { get; set; }
        public System.DateTime denngay { get; set; }
        public int buoidenngay { get; set; }
        public double songaynghi { get; set; }
        public int loaiphep { get; set; }
        public string lydo { get; set; }
        public string nguoiduyet { get; set; }
        public int trangthai { get; set; }
        public string ghichu { get; set; }
        public string phongban { get; set; }
        public System.DateTime ngaytao { get; set; }
        public string lydotuchoi { get; set; }
    
        public virtual TBL_DANHSACHLOAIPHEP TBL_DANHSACHLOAIPHEP { get; set; }
    }
}
