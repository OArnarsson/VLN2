using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoderUi.Startup))]
namespace CoderUi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
