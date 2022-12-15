using IT.Models;
using Microsoft.Owin;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IT.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login
        ITEntities Dbdata = new ITEntities();
        // GET: Login

        [ActionName("dang-nhap")]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            try
            {
               
                var Info = GetInfo();
                if (Info != null)
                {
                    if (returnUrl != null)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("trang-chu", "Home");
                    }
                }
                else if (Request.Cookies["Auth-Token"] != null)
                {
                    var xacthuctoken = Tokenauth();
                    if (xacthuctoken.success == true && xacthuctoken.vaild_to >= DateTime.Now)
                    {
                        var tv = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan.ToLower() == xacthuctoken.email.ToLower());
                        if (tv != null)
                        {
                            PhanQuyen(xacthuctoken.email.ToUpper(), string.Join(",", tv.TBL_DANHMUCPHANQUYEN));
                            if (returnUrl != null)
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("trang-chu", "Home");
                            }
                        }
                        else
                        {
                            return View("login", new LoginModel { taikhoan = null, matkhau = null });
                        }    
                    }
                }
                else
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
                    return View("login", new LoginModel { taikhoan = null, matkhau = null });
                }
            }
            catch (Exception)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
                return View("login", new LoginModel { taikhoan = null, matkhau = null });
            }
            Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
            return View("login", new LoginModel { taikhoan = null, matkhau = null });
        }
        [HttpPost]
        [ActionName("dang-nhap")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            string taikhoan = Sanitizer.GetSafeHtmlFragment(model.taikhoan);
            string matkhau = Sanitizer.GetSafeHtmlFragment(model.matkhau);
            var _configuration = new JWT() { Domain = ".pymepharco.com", Expire = 72, NameCookieAuth = "Auth-Token", Secret = "Pymepharco-secret-key", ValidAudience = "https://esign.pymepharco.com", ValidIssuer = "https://esign.pymepharco.com" };
            //var _configuration = new JWT() { Domain = "localhost", Expire = 72, NameCookieAuth = "Auth-Token", Secret = "Pymepharco-secret-key", ValidAudience = "https://esign.pymepharco.com", ValidIssuer = "https://esign.pymepharco.com" };
            var xacthuc = loginauthapi(taikhoan, matkhau);
            if (xacthuc.authed == true)
            {
                var user = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan.ToUpper() == taikhoan.ToUpper());
                if (user != null)
                {
                    PhanQuyen(taikhoan.ToUpper(), string.Join(",", user.TBL_DANHMUCPHANQUYEN)); //Xử lý phương thức phân quyền
                    System.Web.HttpCookie userInfo = new System.Web.HttpCookie("Auth-Token");
                    userInfo.Value = xacthuc.token;
                    userInfo.Expires.Add(new TimeSpan(72, 0, 0));
                    userInfo.Domain = _configuration.Domain;
                    Response.Cookies.Add(userInfo);


                    if (returnUrl != null)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("trang-chu", "Home");
                    }
                }
                else
                {
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.msg = "Tài khoản của bạn không tồn tại trong hệ thống";
                    return View("login", model);
                }
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.msg = "Tài khoản hoặc mật khẩu không chính xác !";
                return View("login", model);
            }
        }

        //[ActionName("quen-mat-khau")]
        //public ActionResult Recover()
        //{
        //    return View("Recover");
        //}
        //[ActionName("quen-mat-khau")]
        //[HttpPost]
        //public ActionResult Recover(string taikhoan)
        //{
        //    var tv = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan.ToUpper() == taikhoan.ToLower());
        //    if (tv != null)
        //    {
        //        if (tv.email != null)
        //        {
        //            string html = "<div style='width:100%;padding:15px;'><a href='https://pymepharco.com'><img style='margin-left:auto;margin-right: auto;display: block;' alt = 'logo2' src = 'https://gateway.pymepharco.com/Content/Layout1/images/logover2.png'></a></div><center style='width:100%;min-width:600px'><table style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0'> <tbody> <tr style='vertical-align:top;text-align:left;padding:0'> <td class='m_6291312848774364587container-row' style='width: 600px; overflow-wrap: break-word; vertical-align: top; text-align: left; font-family: Helvetica, Arial, sans-serif; line-height: 150%; font-size: 16px; word-break: break-word; margin: 0px 0px 12px; padding: 0px 8px; border-collapse: collapse !important;'> <hr style='font-weight: normal; color: rgb(0, 0, 0); margin: 0px; border: 1px solid rgb(221, 221, 221);'> <br><font color='#000000' style='font-weight: normal;'>Xin chào,</font><br><p style='font-family: Helvetica, Arial, sans-serif; text-align: left; line-height: 150%; font-size: 16px; margin: 0px 0px 10px; padding: 0px;'><span style='font-weight: normal; color: rgb(0, 0, 0);'>Bạn vui lòng click vào đường link bên dưới để thực hiện đặt lại mật khẩu cho tài khoản trên hệ thống </span><b style=''><font color='#397b21'>AM.PYMEPHARCO.COM</font></b></p></td></tr><tr style='vertical-align:top;text-align:left;padding:0'> <td class='m_6291312848774364587container-row' style='width:600px;word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#000;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:150%;font-size:16px;word-break:break-word;margin:0;padding:0 4px'> <table style='width:100%;border-spacing:0;border-collapse:separate;vertical-align:top;text-align:left;border-radius:20px;overflow:hidden;padding:0;border:1px solid #ddd'> <tbody> <tr style='vertical-align:top;text-align:left;padding:0'> <td style='width:100%;word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#000;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:150%;font-size:16px;word-break:break-word;margin:0;padding:24px'><table style='background-color: rgb(255, 255, 255); width: 600px; vertical-align: top; padding: 0px;'><tbody><tr style='vertical-align: top; padding: 0px;'><td height='24px' style='overflow-wrap: break-word; vertical-align: top; color: rgb(0, 0, 0); line-height: 24px; word-break: break-word; margin: 0px; border-collapse: collapse !important;'></td></tr></tbody></table><table cellpadding='0px' style='background-color: rgb(255, 255, 255); width: 600px; vertical-align: top; margin: 0px; padding: 0px;'><tbody><tr style='vertical-align: top; padding: 0px;'><td style='overflow-wrap: break-word; vertical-align: top; color: rgb(0, 0, 0); line-height: 24px; word-break: break-word; margin: 0px; border-collapse: collapse !important;'><center style='width: 600px; min-width: 600px;'><table cellpadding='0px' style='width: 600px; vertical-align: top; padding: 0px;'><tbody><tr style='vertical-align: top; padding: 0px;'><td valign='middle' style='background-color: inherit; overflow-wrap: break-word; vertical-align: top; color: rgb(254, 254, 254); line-height: 24px; word-break: break-word; width: 600px; margin: 0px; border-collapse: collapse !important;'><center style='width: 600px; min-width: 600px;'><a href='https://am.pymepharco.com/doi-mat-khau-reset-code?code=ABCTOKEN' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://am.pymepharco.com/doi-mat-khau-reset-code?code=ABCTOKEN' style='background-color: rgb(9, 84, 211); color: rgb(254, 254, 254); font-weight: bold; line-height: 24px; display: inline-block; border-radius: 3px; width: 360px; margin: 0px; padding: 12px 0px; border: 0px solid rgb(30, 136, 229);'>XÁC THỰC EMAIL ▸</a></center></td></tr></tbody></table></center></td></tr></tbody></table> </td></tr><tr style='background-color:#f8f8f8!important;vertical-align:top;text-align:left;padding:0'> <td style='word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#000;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:150%;font-size:16px;word-break:break-word;margin:0;padding:16px 24px 20px'> <table style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0'> <tbody> <tr style='vertical-align:top;text-align:left;padding:0'> <td style='word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#0954d3;font-family:'Helvetica Neue',sans-serif;font-weight:700;line-height:1em;font-size:11px;word-break:break-word;display:block;letter-spacing:0.7px;text-transform:uppercase;margin:0;padding:0 0 12px'>LƯU Ý</td></tr><tr style='vertical-align:top;text-align:left;padding:0'> <td style='overflow-wrap: break-word; vertical-align: top; text-align: left; color: rgb(51, 51, 51); font-family: &quot;Helvetica Neue&quot;, sans-serif; line-height: 1.4em; font-size: 15px; word-break: break-word; max-height: 88px; overflow: hidden; text-overflow: ellipsis; margin: 0px; padding: 4px 0px 0px; border-collapse: collapse !important;'><span style='font-weight: normal;'>Thư này được tạo ra tự động, nếu bạn cần sự trợ giúp vui lòng liên hệ email </span><b>nguyennhatquang@pymepharco.com</b> hoặc SĐT <b>0387246802</b> (Quang)</td></tr></tbody></table> </td></tr></tbody> </table><table cellpadding='0px' style='width:100%;border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;margin:0px;padding:0'><tbody><tr style='vertical-align:top;text-align:left;padding:0'><td style='word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#000;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:150%;font-size:16px;word-break:break-word;margin:0;padding:0'><center style='width:100%;min-width:600px'><table cellpadding='0px' style='width:100%;border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:center;box-sizing:border-box;padding:0'><tbody> </tbody> </table> </center> </td></tr></tbody> </table> <table style='width:100%;border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;padding:0'> <tbody> <tr style='vertical-align:top;text-align:left;padding:0'> <td height='12px' style='word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#000;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:150%;font-size:16px;word-break:break-word;margin:0;padding:0'><br></td></tr></tbody> </table> </td></tr><tr style='vertical-align:top;text-align:left;padding:0'> <td style='word-wrap:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#000;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:150%;font-size:16px;word-break:break-word;margin:0;padding:0'> <hr style='margin:0px;border:1px solid #ddd'> </td></tr><tr style='vertical-align:top;text-align:left;padding:0'> <td style='overflow-wrap: break-word; vertical-align: top; text-align: left; color: rgb(0, 0, 0); font-family: Helvetica, Arial, sans-serif; line-height: 150%; font-size: 16px; word-break: break-word; margin: 0px; padding: 0px; border-collapse: collapse !important;'><p style='font-weight: normal;'> <br>Trân trọng.</p><p style=''><b>PHÒNG TIN HỌC VÀ PHÂN TÍCH DỮ LIỆU</b></p></td></tr></tbody></table></center>";
        //            string resetCode = Guid.NewGuid().ToString();
        //            tv.resetcode = resetCode;
        //            Dbdata.SaveChanges();
        //            MailMessage mail = new MailMessage();
        //            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        //            mail.From = new MailAddress("gatewaypymepharco@gmail.com");
        //            mail.To.Add(tv.email);
        //            mail.Subject = "XÁC THỰC TÀI KHOẢN HỆ THỐNG AM.PYMEPHARCO.COM";
        //            mail.IsBodyHtml = true;
        //            mail.Body = html.Replace("ABCTOKEN", resetCode);
        //            SmtpServer.Port = 587;
        //            SmtpServer.Credentials = new System.Net.NetworkCredential("gatewaypymepharco@gmail.com", "aavgvkptivazfffi");
        //            SmtpServer.EnableSsl = true;
        //            SmtpServer.Send(mail);
        //            ViewBag.msg = new Messenger() { type = "success", tittle = "Thành công", messenger = "Email xác thực đã được gửi tới địa chỉ " + tv.email };
        //            return View("Recover");
        //        }
        //        else
        //        {
        //            ViewBag.msg = new Messenger() { type = "error", tittle = "Không thành công", messenger = "Tài khoản chưa được cập nhật thông tin Email" };
        //            return View("Recover");
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.msg = new Messenger() { type = "error", tittle = "Không thành công", messenger = "Tài khoản không tồn tại trong hệ thống" };
        //        return View("Recover");
        //    }
        //}
        //[Authorize]
        //[ActionName("doi-mat-khau-lan-dau")]
        //public ActionResult ChangePasswordFist()
        //{

        //    return View("ChangePasswordFist");
        //}
        //[Authorize]
        //[ActionName("doi-mat-khau-lan-dau")]
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult ChangePasswordFirst(string matkhaumoi)
        //{
        //    using (MD5 md5Hash = MD5.Create())
        //    {
        //        var tv = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name);
        //        if (tv.matkhau != GetMd5Hash(md5Hash, matkhaumoi))
        //        {
        //            tv.matkhau = GetMd5Hash(md5Hash, matkhaumoi);
        //            tv.doimk = false;
        //            tv.ngaydoimatkhau = DateTime.Now;
        //            Dbdata.SaveChanges();
        //            return RedirectToAction("trang-chu", "Home");
        //        }
        //        else
        //        {
        //            ViewBag.msg = "Mật khẩu mới trùng với mật khẩu cũ, vui lòng đặt lại mật khẩu khác";
        //            return View("ChangePasswordFirst");
        //        }

        //    }

        //}
        //[Authorize]
        //[ActionName("doi-mat-khau")]
        //public ActionResult ChangePassword()
        //{

        //    return View("ChangePassword");
        //}
        //[Authorize]
        //[ActionName("doi-mat-khau")]
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordModel input)
        //{
        //    using (MD5 md5Hash = MD5.Create())
        //    {
        //        var tv = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan == User.Identity.Name);
        //        if (tv.matkhau == GetMd5Hash(md5Hash, input.matkhaucu))
        //        {
        //            if (tv.matkhau != GetMd5Hash(md5Hash, input.matkhaumoi))
        //            {
        //                tv.matkhau = GetMd5Hash(md5Hash, input.matkhaumoi);
        //                tv.doimk = false;
        //                tv.ngaydoimatkhau = DateTime.Now;
        //                Dbdata.SaveChanges();
        //                return RedirectToAction("trang-chu", "Home");
        //            }
        //            else
        //            {
        //                ViewBag.msg = "Mật khẩu mới trùng với mật khẩu cũ";
        //                return View("ChangePassword");
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.msg = "Nhập mật khẩu cũ chưa chính xác";
        //            return View("ChangePassword");
        //        }
        //    }

        //}
        ////static string GetMd5Hash(MD5 md5Hash, string input)
        ////{

        ////    // Convert the input string to a byte array and compute the hash.
        ////    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        ////    // Create a new Stringbuilder to collect the bytes
        ////    // and create a string.
        ////    StringBuilder sBuilder = new StringBuilder();

        ////    // Loop through each byte of the hashed data
        ////    // and format each one as a hexadecimal string.
        ////    for (int i = 0; i < data.Length; i++)
        ////    {
        ////        sBuilder.Append(data[i].ToString("x2"));
        ////    }

        ////    // Return the hexadecimal string.
        ////    return sBuilder.ToString();
        ////}
        [ActionName("dang-xuat")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-10);
            }
            if (Request.Cookies["AuthenticationToken"] != null)
            {
                Response.Cookies["AuthenticationToken"].Value = string.Empty;
                Response.Cookies["AuthenticationToken"].Expires = DateTime.Now.AddMonths(-10);
            }
            if (Request.Cookies["Auth-Token"] != null)
            {
                //Fetch the Cookie using its Key.
                System.Web.HttpCookie nameCookie = Request.Cookies["Auth-Token"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);
                nameCookie.Domain = ".pymepharco.com";
                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);
               
            }
            return RedirectToAction("dang-nhap");
        }
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
            TaiKhoan,
            DateTime.Now,
            DateTime.Now.AddHours(4),
            false,
            Quyen,
            FormsAuthentication.FormsCookiePath);
            var cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) { HttpOnly = true, Secure = FormsAuthentication.RequireSSL };
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public LoginResponse loginauth(string Email, string Password)
        {
            var client = new RestClient("https://mail.pymepharco.com/WorldClientAPI/authenticate/basic");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("user", Email);
            request.AddParameter("password", Password);
            IRestResponse response = client.Execute(request);
            var data = response.Content;
            //    //Deserializing the response recieved from web api and storing into the Employee list  
            var z = JsonConvert.DeserializeObject<LoginResponse>(data);

            return z;
        }
        public TokenResponse Tokenauth()
        {
            var token = Request.Cookies["Auth-Token"].Value;
            var client = new RestClient("https://esign.pymepharco.com/api/tokeninfo?token=" + token);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var data = response.Content;
            //    //Deserializing the response recieved from web api and storing into the Employee list  
            var z = JsonConvert.DeserializeObject<TokenResponse>(data);

            return z;
        }
        public LoginResponse loginauthapi(string Email, string Password)
        {
            //
            var client = new RestClient("https://esign.pymepharco.com/api/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddParameter("email", Email);
            request.AddParameter("password", Password);
            IRestResponse response = client.Execute(request);

            var data = response.Content;
            //    //Deserializing the response recieved from web api and storing into the Employee list  
            var z = JsonConvert.DeserializeObject<LoginResponse>(data);

            return z;
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("trang-chu", "Home");
        }
        [Authorize]
        public TBL_DANHMUCNGUOIDUNG GetInfo()
        {

            var user = Dbdata.TBL_DANHMUCNGUOIDUNG.SingleOrDefault(n => n.taikhoan.ToUpper() == User.Identity.Name);
            return user;
        }
    }
}
