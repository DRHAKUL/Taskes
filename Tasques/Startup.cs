using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tasques.Startup))]
namespace Tasques
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
