using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartSchool.Startup))]
namespace SmartSchool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
