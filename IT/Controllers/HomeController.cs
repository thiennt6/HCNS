
using IT.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IT.Controllers
{
    public class MyBaseController : Controller
    {
        ITEntities Dbdata = new ITEntities();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan.ToUpper() == User.Identity.Name); //Add whatever
            base.OnActionExecuting(filterContext);
        }
    }
    [Authorize]
    public class HomeController : MyBaseController
    {
        ITEntities Dbdata = new ITEntities();
        [ActionName("trang-chu")]
        public ActionResult Index()
        {
            var data = new THONGKE();
            data.tongthietbi = Dbdata.TBL_DANHMUCSN.Count();
            data.tongthietbinhap = Dbdata.TBL_DANHMUCTHIETBI.Count();
            data.tongthietbichuagiao = Dbdata.TBL_DANHMUCTHIETBI.Where(n => n.xuatkho != true).Count();
            //data.tongnguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.Count();
            //data.tongphongban = Dbdata.TBL_DANHMUCPHONGBAN.Count();
            data.thongke1 = Dbdata.TBL_DANHMUCTHIETBI.GroupBy(n => n.maphong).Select(cl => new MAYTINH { loai = cl.Key, soluong = cl.Count() }).ToList();
            data.thongke2 = Dbdata.TBL_DANHMUCTHIETBI.GroupBy(n => n.makhuvuc).Select(cl => new MAYTINH { loai = cl.Key, soluong = cl.Count() }).ToList();
            return View("Index", data);
        }
        [ActionName("ma-qr")]
        public ActionResult maqr()
        {
            return View("maqr");
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, string mathietbi)
        {
            try
            {
                var tv = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi);
                if (file != null)
                {
                    string path = Server.MapPath("~/Content/Uploads/" + tv.maloai + "/" + tv.mathietbi + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    file.SaveAs(path + Path.GetFileName(file.FileName));
                    return Json(new
                    {
                        success = true,
                        message = "Thành công"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Thất bại"
                    });
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Thất bại"
                });
            }
        }


        [ActionName("tim-kiem-nhanh")]
        public ActionResult Timkiemnhanh(string mathietbi)
        {
            var data = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi.ToUpper());
            if (data != null)
            {
                return View("Timkiemnhanh", data);
            }
            else
            {
                return View("Timkiemnhanh");
            }
        }

        [ActionName("quan-ly-thiet-bi")]
        public ActionResult Quanlythietbi()
        {
            var data = new ListData();
            var nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).TBL_DANHMUCPHANQUYEN.ToList();
            if (nguoidung.SingleOrDefault(n => n.maquyen == "ADMIN") != null)
            {
                data.datathietbi = Dbdata.DTA_DANHSACHTHIETBI.ToList();
            }
            else
            {
                data.datathietbi = Dbdata.DTA_DANHSACHTHIETBI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            }
            data.listphongban = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Quanlythietbi", data);
        }
        [ActionName("quan-ly-danh-muc-phong-ban")]
        public ActionResult Quanlydanhmucphongban()
        {
            var data = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Quanlydanhmucphongban", data);
        }
        [ActionName("quan-ly-danh-muc-loai-thiet-bi")]
        public ActionResult Quanlydanhmucloaithietbi()
        {
            var data = Dbdata.TBL_DANHMUCLOAITHIETBI.ToList();
            return View("Quanlydanhmucloaithietbi", data);
        }
        [ActionName("quan-ly-thiet-bi-moi")]
        public ActionResult Quanlythietbimoi()
        {
            var data = new ListDataMoi();
            var nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).TBL_DANHMUCPHANQUYEN.ToList();
            //if (nguoidung.SingleOrDefault(n => n.maquyen == "ADMIN") != null)
            //{
            //    data.datathietbi = Dbdata.DTA_DANHSACHTHIETBI.ToList();
            //}
            //else
            //{
            //    data.datathietbi = Dbdata.DTA_DANHSACHTHIETBI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            //}
            data.datathietbi = Dbdata.TBL_DANHMUCTHIETBI.OrderByDescending(n => n.ngaynhan).ToList();
            data.listkhuvuc = Dbdata.TBL_DANHMUCKHUVUC.ToList();
            data.listphongban = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            data.listloaithietbi = Dbdata.TBL_DANHMUCLOAITHIETBI.ToList();
            data.listtrangthaimay = Dbdata.TBL_TRANGTHAIMAY.ToList();
            var listsn = Dbdata.TBL_DANHMUCSN.ToList();
            var listsnsudung = Dbdata.TBL_DANHMUCTHIETBI.Select(n => n.serialnumber).ToList();

            data.listsn = listsn;
            return View("Quanlythietbimoi", data);
        }

        [ActionName("quan-ly-anh")]
        public ActionResult Quanlyanh()
        {

            return View("Quanlyanh");
        }


        [ActionName("quan-ly-thiet-bi-khac")]
        public ActionResult Quanlythietbikhac()
        {
            var data = new ListDataMayIn();
            var nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).TBL_DANHMUCPHANQUYEN.ToList();
            if (nguoidung.SingleOrDefault(n => n.maquyen == "ADMIN") != null)
            {
                data.datamayin = Dbdata.DTA_DANHMUCTHIETBIKHAC.ToList();
            }
            else
            {
                data.datamayin = Dbdata.DTA_DANHMUCTHIETBIKHAC.Where(n => n.taikhoan == User.Identity.Name).ToList();
            }

            data.listphongban = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Quanlythietbikhac", data);
        }
        [HttpPost]
        public ActionResult Partialthietbikhac(List<string> listmaphong)
        {
            var data = new ListDataMayIn();

            var nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).TBL_DANHMUCPHANQUYEN.ToList();
            if (nguoidung.SingleOrDefault(n => n.maquyen == "ADMIN") != null)
            {
                data.datamayin = Dbdata.DTA_DANHMUCTHIETBIKHAC.Where(n => listmaphong.Contains(n.maphong)).ToList();
            }
            else
            {
                data.datamayin = Dbdata.DTA_DANHMUCTHIETBIKHAC.Where(n => n.taikhoan == User.Identity.Name && listmaphong.Contains(n.maphong)).ToList();
            }
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult Partiallichsuthietbimoi(string mathietbi)
        {
            var data = Dbdata.TBL_DANHMUCTHIETBI_AUDIT.Where(n => n.mathietbi == mathietbi).OrderByDescending(n => n.id);
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult Partialthietbi(List<string> listmaphong, List<string> listloaimaytinh, List<string> listos)
        {

            var data = new ListData();
            var nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).TBL_DANHMUCPHANQUYEN.ToList();
            if (nguoidung.SingleOrDefault(n => n.maquyen == "ADMIN") != null)
            {
                data.datathietbi = Dbdata.DTA_DANHSACHTHIETBI.ToList();
            }
            else
            {
                data.datathietbi = Dbdata.DTA_DANHSACHTHIETBI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            }
            if (listmaphong != null)
            {
                data.datathietbi = data.datathietbi.Where(n => listmaphong.Contains(n.maphong)).ToList();
            }
            if (listos != null)
            {
                data.datathietbi = data.datathietbi.Where(n => listos.Contains(n.os)).ToList();
            }
            if (listloaimaytinh != null)
            {
                data.datathietbi = data.datathietbi.Where(n => listloaimaytinh.Contains(n.loaimaytinh)).ToList();
            }
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult Partialphongban()
        {
            var data = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult Partialthietbimoi(List<string> listmaphong, List<string> listloaithietbi, List<string> listmakhuvuc, List<bool?> locxuatkho, List<string> loctrangthaimay)
        {
            var data = new ListDataMoi();
            var nguoidung = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).TBL_DANHMUCPHANQUYEN.ToList();

            data.datathietbi = Dbdata.TBL_DANHMUCTHIETBI.ToList();

            if (listmaphong != null)
            {
                data.datathietbi = data.datathietbi.Where(n => listmaphong.Contains(n.maphong)).ToList();
            }
            if (listmakhuvuc != null)
            {
                data.datathietbi = data.datathietbi.Where(n => listmakhuvuc.Contains(n.makhuvuc)).ToList();
            }
            if (listloaithietbi != null)
            {
                data.datathietbi = data.datathietbi.Where(n => listloaithietbi.Contains(n.maloai)).ToList();
            }
            if (locxuatkho != null)
            {
                data.datathietbi = data.datathietbi.Where(n => locxuatkho.Contains(n.xuatkho)).ToList();
            }
            if (loctrangthaimay != null)
            {
                data.datathietbi = data.datathietbi.Where(n => loctrangthaimay.Contains(n.trangthaiid.ToString())).ToList();
            }
            //if(locngaygiao != null && locngaygiao != "")
            //{
            //    string tungay = locngaygiao.Substring(0, 10);
            //    string denngay = locngaygiao.Substring(locngaygiao.Length - 10);
            //    DateTime tungay1 = DateTime.ParseExact(tungay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    DateTime denngay1 = DateTime.ParseExact(denngay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    data.datathietbi = data.datathietbi.Where(n => n.ngaynhan >= tungay1 && n.ngaynhan < denngay1.AddDays(1)).ToList();
            //}    
            return PartialView(data);
        }
        [ActionName("tra-cuu")]
        public ActionResult Tracuu()
        {
            var data = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Tracuu", data);
        }
        [ActionName("in-bao-cao")]
        public ActionResult Inbaocao()
        {
            var data = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Inbaocao", data);
        }
        [ActionName("them-thiet-bi")]
        public ActionResult Themthietbi()
        {
            var data = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Themthietbi", data);
        }
        [ActionName("them-thiet-bi-moi")]
        public ActionResult Themthietbimoi()
        {
            var listsn = Dbdata.TBL_DANHMUCSN.ToList();
            var listsnsudung = Dbdata.TBL_DANHMUCTHIETBI.Select(n => n.serialnumber).ToList();

            var data = new THIETBIMOI() { LOAITHIETBI = Dbdata.TBL_DANHMUCLOAITHIETBI.ToList(), TRANGTHAIMAY = Dbdata.TBL_TRANGTHAIMAY.ToList(), KHUVUC = Dbdata.TBL_DANHMUCKHUVUC.ToList(), PHONGBAN = Dbdata.TBL_DANHMUCPHONGBAN.ToList(), SN = listsn };

            return View("Themthietbimoi", data);
        }

        [ActionName("quan-ly-danh-sach-ngay-nghi")]
        public ActionResult Quanlydanhsachngaynghi()
        {
            var data = new Listquanlyngaynghi();
            var userinfo = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name);
            //data.DANHSACHNGAYNGHI = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            data.LOAIPHEP = Dbdata.TBL_DANHSACHLOAIPHEP.ToList();
            data.NGUOIDUNG = Dbdata.TBL_DANHMUCNGUOIDUNG.ToList();
            data.NguoiDuyet = userinfo.TBL_DANHMUCPHONGBAN.nguoiduyet;
            // Tính tổng số ngày nghỉ phép năm 
            var ngaykyhopdong = userinfo.ngaykyhopdong;



            var sonamlamviec = (DateTime.Now - (DateTime)ngaykyhopdong).TotalDays;

            int ngaynghithem = Convert.ToInt32(sonamlamviec / 365 / Dbdata.TBL_DANHSACHLOAIPHEP.SingleOrDefault(n => n.loaiphep == 1).sonam);
            // Tính phép năm đã dùng & chờ duyệt
            var phepnamdadung = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 1 && n.taikhoan == User.Identity.Name && n.trangthai == 2).Count();
            var phepnamchoduyet = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 1 && n.taikhoan == User.Identity.Name && n.trangthai == 1).Count();
            // Tính phép gia cảnh đã dùng
            var phepgiacanhdadung = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 2 && n.taikhoan == User.Identity.Name && n.trangthai == 2).Count();
            var phepgiacanhchoduyet = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 2 && n.taikhoan == User.Identity.Name && n.trangthai == 1).Count();
            // Tính phép nghỉ ốm đã dùng
            var phepnghiomdadung = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 3 && n.taikhoan == User.Identity.Name && n.trangthai == 2).Count();
            var phepnghiomchoduyet = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 3 && n.taikhoan == User.Identity.Name && n.trangthai == 1).Count();
            // Tính phép nghỉ xưởng đã dùng
            var phepnghixuongdadung = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 4 && n.taikhoan == User.Identity.Name && n.trangthai == 2).Count();
            var phepnghixuongchoduyet = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 4 && n.taikhoan == User.Identity.Name && n.trangthai == 1).Count();
            // Tính phép nghỉ việc riêng đã dùng
            var phepnghiviecriengdadung = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 5 && n.taikhoan == User.Identity.Name && n.trangthai == 2).Count();
            var phepnghiviecriengchoduyet = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.loaiphep == 5 && n.taikhoan == User.Identity.Name && n.trangthai == 1).Count();
            //var datathongke = new Listthongkengaynghi();

            data.TongPhepNam = (int)Dbdata.TBL_DANHSACHLOAIPHEP.SingleOrDefault(n => n.loaiphep == 1).tongngaynghi + ngaynghithem;
            data.PhepNamDaDung = phepnamdadung;
            data.PhepNamChoDuyet = phepnamchoduyet;
            data.PhepGiaCanhDaDung = phepgiacanhdadung;
            data.PhepGiaCanhChoDuyet = phepgiacanhchoduyet;
            data.PhepNghiOmDaDung = phepnghiomdadung;
            data.PhepNghiOmChoDuyet = phepnghiomchoduyet;
            data.PhepNghiXuongDaDung = phepnghixuongdadung;
            data.PhepNghiXuongChoDuyet = phepnghixuongchoduyet;
            data.PhepNghiViecRiengTSmDaDung = phepnghiviecriengdadung;
            data.PhepNghiViecRiengTSChoDuyet = phepnghiviecriengchoduyet;


            //data.LISTTHONGKENGAYNGHI = datathongke;
            return View("Quanlydanhsachngaynghi", data);
        }
        [HttpPost]
        public JsonResult addngaynghi(TBL_DANHSACHNGAYNGHI data)
        {
            Dbdata.TBL_DANHSACHNGAYNGHI.Add(new TBL_DANHSACHNGAYNGHI()
            {
                taikhoan = User.Identity.Name,
                tungay = data.tungay,
                buoitungay = data.buoitungay,
                buoidenngay = data.buoidenngay,
                denngay = data.denngay,
                songaynghi = data.songaynghi,
                loaiphep = data.loaiphep,
                lydo = data.lydo,
                nguoiduyet = data.nguoiduyet,
                trangthai = 1,
                ghichu = data.ghichu,
                phongban = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).maphong,
                ngaytao = DateTime.Now,
            });
            Dbdata.SaveChanges();
            return Json(1);
        }
        [HttpPost]
        public JsonResult deletengaynghi(int Id)
        {
            var data = Dbdata.TBL_DANHSACHNGAYNGHI.SingleOrDefault(n => n.Id == Id);
            Dbdata.TBL_DANHSACHNGAYNGHI.Remove(data);
            Dbdata.SaveChanges();
            return Json(1);
        }
        [HttpPost]
        public JsonResult getngaynghi(int Id)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.TBL_DANHSACHNGAYNGHI.SingleOrDefault(n => n.Id == Id);
            return Json(data);
        }
        [HttpPost]
        public ActionResult Partialquanlydanhsachngaynghi(string tungay, string denngay, int? loaiphep, int? trangthai)
        {
            var data = new List<TBL_DANHSACHNGAYNGHI>();

            if (tungay != null && denngay != null)
            {
                DateTime tungay1 = DateTime.ParseExact(tungay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime denngay1 = DateTime.ParseExact(denngay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                data = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name && n.tungay >= tungay1 && n.tungay <= denngay1).ToList();
            }
            else
            {
                DateTime tungay1 = DateTime.ParseExact("01/01/" + DateTime.Today.Year, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime denngay1 = DateTime.Today;
                data = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name && n.tungay >= tungay1 && n.tungay <= denngay1).ToList();
            }
            return PartialView(data);
        }

        [HttpPost]
        public JsonResult editngaynghi(TBL_DANHSACHNGAYNGHI data)
        {
            var tv = Dbdata.TBL_DANHSACHNGAYNGHI.SingleOrDefault(n => n.Id == data.Id);
            if (tv != null)
            {
                tv.tungay = data.tungay;
                tv.buoitungay = data.buoitungay;
                tv.buoidenngay = data.buoidenngay;
                tv.denngay = data.denngay;
                tv.songaynghi = data.songaynghi;
                tv.loaiphep = data.loaiphep;
                tv.lydo = data.lydo;
                tv.nguoiduyet = data.nguoiduyet;
                tv.trangthai = 1;
                tv.ghichu = data.ghichu;
                Dbdata.SaveChanges();
            }

            else
            {
                return Json("0");
            }
            return Json("1");
        }


        [ActionName("quan-ly-danh-sach-phe-duyet")]
        public ActionResult Quanlydanhsachpheduyet()
        {
            var data = new Listquanlypheduyet();
            data.PhongBanCuaBan = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).maphong;
            data.DANHSACHNGAYNGHI = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            data.LOAIPHEP = Dbdata.TBL_DANHSACHLOAIPHEP.ToList();
            data.NguoiDuyet = Dbdata.TBL_DANHMUCPHONGBAN.SingleOrDefault(n => n.maphong == data.PhongBanCuaBan).nguoiduyet;

            return View("Quanlydanhsachpheduyet", data);
        }

        [HttpPost]
        public JsonResult getpheduyet(int Id)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.TBL_DANHSACHNGAYNGHI.SingleOrDefault(n => n.Id == Id);
            return Json(data);
        }

        [HttpPost]
        public JsonResult pheduyet(int Id)
        {
            var tv = Dbdata.TBL_DANHSACHNGAYNGHI.SingleOrDefault(n => n.Id == Id);
            if (tv != null)
            {
                tv.trangthai = 2;
                Dbdata.SaveChanges();
            }

            else
            {
                return Json("0");
            }
            return Json("1");
        }

        [HttpPost]
        public JsonResult tuchoi(Listtuchoi data)
        {
            var tv = Dbdata.TBL_DANHSACHNGAYNGHI.SingleOrDefault(n => n.Id == data.Id);
            if (tv != null)
            {
                tv.trangthai = 3;
                tv.lydotuchoi = data.LyDoTuChoi;
                Dbdata.SaveChanges();
            }

            else
            {
                return Json("0");
            }
            return Json("1");
        }
        [HttpGet]
        public ActionResult Partialquanlydanhsachpheduyet()
        {
            var data = new Listquanlypheduyet();
            data.PhongBanCuaBan = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name).maphong;
            data.DANHSACHNGAYNGHI = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            data.LOAIPHEP = Dbdata.TBL_DANHSACHLOAIPHEP.ToList();
            data.NguoiDuyet = Dbdata.TBL_DANHMUCPHONGBAN.SingleOrDefault(n => n.maphong == data.PhongBanCuaBan).nguoiduyet;
            return PartialView(data);
        }

        [ActionName("lich-nghi-phep")]
        public ActionResult Lichnghiphep()
        {
            //Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = new Listquanlyngaynghi();
            var userinfo = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name);
            //data.DANHSACHNGAYNGHI = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name).ToList();
            data.LOAIPHEP = Dbdata.TBL_DANHSACHLOAIPHEP.ToList();
            data.NGUOIDUNG = Dbdata.TBL_DANHMUCNGUOIDUNG.ToList();
            data.NguoiDuyet = userinfo.TBL_DANHMUCPHONGBAN.nguoiduyet;
            //var data = new Listlich();
            var data1 = new List<Lichnghiphep>();
            data.DANHSACHNGAYNGHI = Dbdata.TBL_DANHSACHNGAYNGHI.Where(n => n.taikhoan == User.Identity.Name && n.trangthai != 3).ToList();
            // data.LOAIPHEP = Dbdata.TBL_DANHSACHLOAIPHEP.ToList();
            int i;
            DateTime Ngay;
            for (i = 0; i < data.DANHSACHNGAYNGHI.Count; i++)
            {
                if (data.DANHSACHNGAYNGHI[i].buoidenngay == 1)
                {
                    Ngay = data.DANHSACHNGAYNGHI[i].denngay.AddDays(-1);        
                }
                else
                {
                    Ngay = data.DANHSACHNGAYNGHI[i].denngay;
                };
                data1.Add(new Lichnghiphep()
                {
                    Id = data.DANHSACHNGAYNGHI[i].Id,
                    loaiphep = data.DANHSACHNGAYNGHI[i].loaiphep,
                    tungay = data.DANHSACHNGAYNGHI[i].tungay,
                    buoitungay = data.DANHSACHNGAYNGHI[i].buoitungay,
                    denngay = Ngay,
                    buoidenngay = data.DANHSACHNGAYNGHI[i].buoidenngay,
                    trangthai = data.DANHSACHNGAYNGHI[i].trangthai,
                });
            }
            ViewBag.data = data1;

            return View("Lichnghiphep", data);
        }

        //Extension method to convert Bitmap to Byte Array






        [ActionName("them-thiet-bi-khac")]
        public ActionResult Themthietbikhac()
        {
            var data = Dbdata.TBL_DANHMUCPHONGBAN.ToList();
            return View("Themthietbikhac", data);
        }

        [HttpPost]
        public JsonResult xoathietbi(int id)
        {
            var data = Dbdata.DTA_DANHSACHTHIETBI.SingleOrDefault(n => n.id == id);
            Dbdata.DTA_DANHSACHTHIETBI.Remove(data);
            Dbdata.SaveChanges();
            return Json(1);
        }
        [HttpPost]
        public JsonResult xoathietbimoi(string mathietbi)
        {
            var data = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi);

            Dbdata.TBL_DANHMUCTHIETBI.Remove(data);
            Dbdata.SaveChanges();
            return Json(1);
        }
        [HttpPost]
        public JsonResult xoathietbikhac(int id)
        {
            var data = Dbdata.DTA_DANHMUCTHIETBIKHAC.SingleOrDefault(n => n.id == id);
            Dbdata.DTA_DANHMUCTHIETBIKHAC.Remove(data);
            Dbdata.SaveChanges();
            return Json(1);
        }
        [HttpPost]
        public JsonResult addthietbi(DTA_DANHSACHTHIETBI data)
        {
            data.taikhoan = User.Identity.Name;
            try
            {
                Dbdata.DTA_DANHSACHTHIETBI.Add(data);
                Dbdata.SaveChanges();
                return Json("1");
            }
            catch (Exception)
            {
                return Json("0");
            }
            return Json("1");
        }
        //[HttpPost]
        //public JsonResult addthietbimoi(TBL_DANHMUCTHIETBI data)
        //{
        //    data.taikhoan = User.Identity.Name;
        //    data.ngay = DateTime.Now;
        //    data.tenmay = "STVN-" + data.serialnumber;
        //    data.mathietbi = GETMATHIETBIMOI(data.maloai);

        //    try
        //    {
        //        Dbdata.TBL_DANHMUCTHIETBI.Add(data);

        //        Dbdata.SaveChanges();
        //        return Json(new QRCODE() { data = data.mathietbi, qrcode = CreateQRCode(data.tenmay) });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new QRCODE() { data = null, qrcode = null });
        //    }
        //    return Json("1");
        //}
        //[HttpPost]
        //public JsonResult editthietbimoi(TBL_DANHMUCTHIETBI data)
        //{
        //    var tv = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == data.mathietbi);
        //    if (tv != null)
        //    {
        //        tv.taikhoan = User.Identity.Name;
        //        tv.ngay = DateTime.Now;
        //        tv.serialnumber = data.serialnumber;
        //        tv.tenmay = "STVN-" + data.serialnumber;
        //        tv.makhuvuc = data.makhuvuc;
        //        tv.maphong = data.maphong;
        //        tv.tennguoidung = data.tennguoidung;
        //        tv.userid = data.userid;
        //        tv.chucvu = data.chucvu;
        //        tv.emailpyme = data.emailpyme;
        //        tv.sdt = data.sdt;
        //        tv.bpyeucau = data.bpyeucau;
        //        tv.email = data.email;
        //        tv.ngaynhan = data.ngaynhan;
        //        tv.ghichu = data.ghichu;
        //        tv.xuatkho = data.xuatkho;
        //        //tv.giaothanhcong = data.giaothanhcong;
        //        tv.urlimage = data.urlimage;
        //        tv.trangthaiid = data.trangthaiid;
        //        Dbdata.SaveChanges();
        //        return Json(new QRCODE() { data = tv.mathietbi, qrcode = CreateQRCode(tv.mathietbi) });
        //    }
        //    else
        //    {
        //        return Json(new QRCODE() { data = null, qrcode = null });
        //    }
        //    return Json("1");
        //}
        [HttpPost]
        public JsonResult editloaithietbi(TBL_DANHMUCLOAITHIETBI data)
        {
            var tv = Dbdata.TBL_DANHMUCLOAITHIETBI.SingleOrDefault(n => n.maloai == data.maloai);
            if (tv != null)
            {
                tv.tenthietbi = data.tenthietbi;
                tv.model = data.model;
                tv.lop = data.lop;

                Dbdata.SaveChanges();
                return Json(new QRCODE() { data = tv.maloai });
            }
            else
            {
                return Json(new QRCODE() { data = null, qrcode = null });
            }
            return Json("1");
        }
        [HttpPost]
        public JsonResult editphongban(TBL_DANHMUCPHONGBAN data)
        {
            var tv = Dbdata.TBL_DANHMUCPHONGBAN.SingleOrDefault(n => n.maphong == data.maphong);
            if (tv != null)
            {
                tv.tenphong = data.tenphong;
                Dbdata.SaveChanges();
                return Json(new QRCODE() { data = tv.maphong });
            }
            else
            {
                return Json(new QRCODE() { data = null, qrcode = null });
            }
            return Json("1");
        }
        [HttpPost]
        public JsonResult deletephongban(string maphong)
        {
            var tv = Dbdata.TBL_DANHMUCPHONGBAN.SingleOrDefault(n => n.maphong == maphong);
            if (tv != null)
            {
                Dbdata.TBL_DANHMUCPHONGBAN.Remove(tv);
                Dbdata.SaveChanges();
                return Json(new QRCODE() { data = tv.maphong });
            }
            else
            {
                return Json(new QRCODE() { data = null, qrcode = null });
            }
            return Json("1");
        }
        [HttpPost]
        public JsonResult deleteloaithietbi(string maloai)
        {
            var tv = Dbdata.TBL_DANHMUCLOAITHIETBI.SingleOrDefault(n => n.maloai == maloai);
            if (tv != null)
            {
                Dbdata.TBL_DANHMUCLOAITHIETBI.Remove(tv);
                Dbdata.SaveChanges();
                return Json(new QRCODE() { data = tv.maloai });
            }
            else
            {
                return Json(new QRCODE() { data = null, qrcode = null });
            }
            return Json("1");
        }
        [HttpPost]
        public JsonResult addphongban(TBL_DANHMUCPHONGBAN data)
        {
            Dbdata.TBL_DANHMUCPHONGBAN.Add(new TBL_DANHMUCPHONGBAN() { maphong = data.maphong, tenphong = data.tenphong });
            Dbdata.SaveChanges();
            return Json(new QRCODE() { data = data.maphong });
        }
        [HttpPost]
        public JsonResult addloaithietbi(TBL_DANHMUCLOAITHIETBI data)
        {
            Dbdata.TBL_DANHMUCLOAITHIETBI.Add(data);
            Dbdata.SaveChanges();
            return Json(new QRCODE() { data = data.maloai });
        }
        private string GETMATHIETBIMOI(string maloai)
        {
            try
            {
                var mathietbimoinhat = Dbdata.TBL_DANHMUCTHIETBI.Where(n => n.maloai == maloai).OrderByDescending(n => n.mathietbi).FirstOrDefault().mathietbi;
                var mathietbi = maloai + "-" + (Int32.Parse(mathietbimoinhat.Substring(mathietbimoinhat.Length - 4)) + 1).ToString("00000");
                return mathietbi;
            }
            catch (Exception)
            {
                return (maloai + "-00001");
            }
        }

        [HttpPost]
        public JsonResult suathietbi(DTA_DANHSACHTHIETBI data)
        {
            var z = Dbdata.DTA_DANHSACHTHIETBI.SingleOrDefault(n => n.id == data.id);
            if (data.urlimage != null && data.urlimage != z.urlimage)
            {
                try
                {
                    string fullPath = Request.MapPath("~" + z.urlimage);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch (Exception)
                {

                }

            }
            z.maphong = data.maphong;
            z.ip = data.ip;
            z.hdd = data.hdd;
            z.ssd = data.ssd;
            z.ram = data.ram;
            z.cpu = data.cpu;
            z.loaimaytinh = data.loaimaytinh;
            z.namsudung = data.namsudung;
            z.os = data.os;
            z.tenmay = data.tenmay;
            z.thuonghieu = data.thuonghieu;
            if (data.urlimage != null)
            {
                z.urlimage = data.urlimage;
            }

            z.tennguoidung = data.tennguoidung;
            Dbdata.SaveChanges();
            return Json("1");

        }
        [HttpPost]
        public JsonResult addthietbikhac(DTA_DANHMUCTHIETBIKHAC data)
        {
            data.taikhoan = User.Identity.Name;
            try
            {
                Dbdata.DTA_DANHMUCTHIETBIKHAC.Add(data);
                Dbdata.SaveChanges();
                return Json("1");
            }
            catch (Exception)
            {
                return Json("0");
            }
            return Json("1");
        }
        [HttpPost]
        public JsonResult getthietbikhac(int id)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.DTA_DANHMUCTHIETBIKHAC.SingleOrDefault(n => n.id == id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getthietbi(int id)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.DTA_DANHSACHTHIETBI.SingleOrDefault(n => n.id == id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getthietbimoi(string mathietbi)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi);
            var final = new Getthietbimoi() { thietbi = data };
            try
            {
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/" + data.maloai + "/" + data.mathietbi + "/"));

                List<string> files = new List<string>();
                foreach (string filePath in filePaths)
                {
                    files.Add(Path.GetFileName(filePath));
                }
                final.files = files;
            }
            catch (Exception)
            {

            }


            return Json(final);
        }
        [HttpPost]
        public ActionResult PartialFile(string mathietbi)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi);
            List<string> files = new List<string>();
            try
            {
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/Uploads/" + data.maloai + "/" + data.mathietbi + "/"));


                foreach (string filePath in filePaths)
                {
                    files.Add(Path.GetFileName(filePath));
                }
                ViewBag.mathietbi = data.mathietbi;
                return PartialView(files);
            }
            catch (Exception)
            {
                return PartialView(files);
            }
            return PartialView(files);

        }
        [HttpPost]
        public ActionResult FileDelete(string name, string mathietbi)
        {
            var data = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi);
            try
            {
                string path = Server.MapPath("~/Content/Uploads/" + data.maloai + "/" + data.mathietbi + "/") + name;

                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                return Json(0);
            }
            return Json(0);
        }
        public ActionResult FileDownload(string name, string mathietbi)
        {
            var data = Dbdata.TBL_DANHMUCTHIETBI.SingleOrDefault(n => n.mathietbi == mathietbi);
            try
            {
                string path = Server.MapPath("~/Content/Uploads/" + data.maloai + "/" + data.mathietbi + "/") + name;

                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                //Send the File to Download.
                return File(bytes, "application/octet-stream", name);
            }
            catch (Exception)
            {
                return Content("<script type='text/javascript'>alert('Không tìm thấy File này, vui lòng thử lại !'); window.close();</script>");
            }
        }
        [HttpPost]
        public JsonResult getloaithietbi(string maloai)
        {
            Dbdata.Configuration.ProxyCreationEnabled = false;
            var data = Dbdata.TBL_DANHMUCLOAITHIETBI.SingleOrDefault(n => n.maloai == maloai);
            return Json(data);
        }
        [HttpPost]
        public JsonResult suathietbikhac(DTA_DANHMUCTHIETBIKHAC data)
        {
            var z = Dbdata.DTA_DANHMUCTHIETBIKHAC.SingleOrDefault(n => n.id == data.id);
            z.ip = data.ip;
            z.loai = data.loai;
            z.maphong = data.maphong;
            z.namsudung = data.namsudung;
            z.soluong = data.soluong;
            z.tenmay = data.tenmay;
            Dbdata.SaveChanges();
            return Json("1");
        }
        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/DataImage/"), fileName);
                file.SaveAs(path);
                return Json(1);
            }
            return Json(0);
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }


}
