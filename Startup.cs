using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgencyManager.Startup))]
namespace AgencyManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
