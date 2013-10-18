using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RfWeb.Startup))]
namespace RfWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			ConfigureSignalR(app);
            ConfigureAuth(app);
        }
    }
}
