using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Yx.Portal.Startup))]
namespace Yx.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
