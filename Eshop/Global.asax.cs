using DataLayer;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eshop
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HttpContext.Current.Application["Online"] = 0;
        }

        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        protected void Session_Start()
        {
            var userIp = Request.UserHostName;
            var dtNow = DateTime.Now.Date;
            using (Eshop_DBEntities db = new Eshop_DBEntities())
            {
                if (!db.Visits.Any(v => v.UserIp == userIp && v.Date == dtNow))
                {
                    db.Visits.Add(new Visits()
                    {
                        UserIp = userIp,
                        Date = dtNow
                    });

                    db.SaveChanges();
                }
            }
        }

    }
}