using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "taikhoan")]

        public string taikhoan { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "matkhau")]
        public string matkhau { get; set; }
    }

    public partial class PDFBase64
    {
        public string Base64 { get; set; }
    }
    public partial class Getthietbimoi
    {
        public TBL_DANHMUCTHIETBI thietbi { get; set; }
        public List<string> files { get; set; }
    }
    public partial class THONGKE
    {
        public int tongthietbi { get; set; }
        public int tongthietbinhap { get; set; }
        public int tongthietbichuagiao { get; set; }
        //public int tongphongban { get; set; }
        //public int tongnguoidung { get; set; }
        public List<MAYTINH> thongke1 { get; set; }
        public List<MAYTINH> thongke2 { get; set; }
    }
    public partial class MAYTINH
    {
        public string loai { get; set; }
        public int soluong { get; set; }
        public string os { get; set; }
        public string thuonghỉieu { get; set; }
    }
    public partial class THIETBIMOI
    {
        public List<TBL_DANHMUCSN> SN { get; set; }
        public List<TBL_DANHMUCLOAITHIETBI> LOAITHIETBI { get; set; }
        public List<TBL_DANHMUCKHUVUC> KHUVUC { get; set; }
        public List<TBL_DANHMUCPHONGBAN> PHONGBAN { get; set; }
        public List<TBL_TRANGTHAIMAY> TRANGTHAIMAY { get; set; }
    }
    public partial class Messenger
    {
        public string type { get; set; }
        public string tittle { get; set; }
        public string messenger { get; set; }

    }
    public partial class QRCODE
    {
        public string data { get; set; }
        public string qrcode { get; set; }

    }
    public class QRCodeModel
    {
        [Display(Name = "Enter QRCode Text")]
        public string QRCodeText { get; set; }

    }
    public partial class ListDataMoi
    {
        public List<TBL_DANHMUCPHONGBAN> listphongban { get; set; }
        public List<TBL_DANHMUCKHUVUC> listkhuvuc { get; set; }
        public List<TBL_DANHMUCLOAITHIETBI> listloaithietbi { get; set; }
        public List<TBL_TRANGTHAIMAY> listtrangthaimay { get; set; }

        public List<TBL_DANHMUCSN> listsn { get; set; }
        public List<TBL_DANHMUCTHIETBI> datathietbi { get; set; }
    }
    public class TokenResponse
    {
        [Key]
        public bool success { get; set; }
        public string email { get; set; }
        public DateTime vaild_to { get; set; }
    }
    public class LoginResponse
    {
        [Key]
        public bool authed { get; set; }

        public string error { get; set; }
        public string parameter { get; set; }

        public string session { get; set; }
        public string user { get; set; }
        public string token { get; set; }
    }
    public class JWT
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string NameCookieAuth { get; set; }
        public string Secret { get; set; }
        public string Domain { get; set; }
        public int Expire { get; set; }
    }
    public partial class ListData
    {
        public List<TBL_DANHMUCPHONGBAN> listphongban { get; set; }

        public List<DTA_DANHSACHTHIETBI> datathietbi { get; set; }
    }
    public partial class ListDataMayIn
    {
        public List<TBL_DANHMUCPHONGBAN> listphongban { get; set; }

        public List<DTA_DANHMUCTHIETBIKHAC> datamayin { get; set; }
    }
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string matkhaucu { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string matkhaumoi { get; set; }
    }
    public partial class Listquanlyngaynghi
    {
        public List<TBL_DANHSACHNGAYNGHI> DANHSACHNGAYNGHI { get; set; }
        public List<TBL_DANHSACHLOAIPHEP> LOAIPHEP { get; set; }
        public List<TBL_DANHMUCNGUOIDUNG> NGUOIDUNG { get; set; }
        public string NguoiDuyet { get; set; }
        public string PhongBanCuaBan { get; set; }
        public double TongPhepNam { get; set; }
        public double PhepNamDaDung { get; set; }
        public int PhepNamChoDuyet { get; set; }
        public double PhepGiaCanhDaDung { get; set; }
        public int PhepGiaCanhChoDuyet { get; set; }
        public double PhepNghiOmDaDung { get; set; }
        public int PhepNghiOmChoDuyet { get; set; }
        public double PhepNghiXuongDaDung { get; set; }
        public int PhepNghiXuongChoDuyet { get; set; }
        public double PhepNghiViecRiengTSmDaDung { get; set; }
        public int PhepNghiViecRiengTSChoDuyet { get; set; }
    }

    public partial class Listquanlypheduyet
    {
        public List<TBL_DANHSACHNGAYNGHI> DANHSACHNGAYNGHI { get; set; }
        public List<TBL_DANHSACHLOAIPHEP> LOAIPHEP { get; set; }
        public string NguoiDuyet { get; set; }
        public string PhongBanCuaBan { get; set; }
    }

    public partial class Listtuchoi
    {
        public int Id { get; set; }
        public string LyDoTuChoi { get; set; }
    }

    public partial class Listlich
    {
        public List<TBL_DANHSACHNGAYNGHI> DANHSACHNGAYNGHI { get; set; }
        public List<TBL_DANHSACHLOAIPHEP> LOAIPHEP { get; set; }
    }

    public partial class Lichnghiphep
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
    }
}
