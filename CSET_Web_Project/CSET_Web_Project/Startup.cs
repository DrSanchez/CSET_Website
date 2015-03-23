using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSET_Web_Project.Startup))]
namespace CSET_Web_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
