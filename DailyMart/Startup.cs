using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DailyMart.Startup))]
namespace DailyMart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
