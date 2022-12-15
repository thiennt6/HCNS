using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Owin;
namespace IT
{
    public class MvcApplication : System.Web.HttpApplication
    {
      
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");           //Remove Server Header  
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version Header
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                var TaikhoanCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (TaikhoanCookie != null)
                {
                    var authTicket = FormsAuthentication.Decrypt(TaikhoanCookie.Value);
                    var Quyen = authTicket.UserData.Split(new Char[] { ',' });
                    var userPrincipal = new GenericPrincipal(new GenericIdentity
                    (authTicket.Name), Quyen);
                    Context.User = userPrincipal;
                }
            }
            catch (CryptographicException cex)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}
