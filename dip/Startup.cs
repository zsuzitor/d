using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dip.Startup))]
namespace dip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
