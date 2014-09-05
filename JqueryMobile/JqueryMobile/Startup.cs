using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JqueryMobile.Startup))]
namespace JqueryMobile
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
