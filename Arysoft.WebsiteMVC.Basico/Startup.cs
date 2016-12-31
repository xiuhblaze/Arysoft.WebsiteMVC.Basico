using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Arysoft.WebsiteMVC.Basico.Startup))]
namespace Arysoft.WebsiteMVC.Basico
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
