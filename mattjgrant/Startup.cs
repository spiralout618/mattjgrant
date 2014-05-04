using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mattjgrant.Startup))]
namespace mattjgrant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
