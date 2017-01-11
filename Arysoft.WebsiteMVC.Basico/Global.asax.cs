using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Arysoft.WebsiteMVC.Basico
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo unCI = System.Globalization.CultureInfo.CreateSpecificCulture("es-MX");

            System.Threading.Thread.CurrentThread.CurrentCulture = unCI;
            System.Threading.Thread.CurrentThread.CurrentUICulture = unCI;
        }
    }
}
