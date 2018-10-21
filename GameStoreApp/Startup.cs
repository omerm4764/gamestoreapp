using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameStoreApp.Startup))]
namespace GameStoreApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
