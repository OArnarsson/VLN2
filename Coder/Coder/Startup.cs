using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Coder.Startup))]
namespace Coder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
