using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SDCEventTracker.Startup))]
namespace SDCEventTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
