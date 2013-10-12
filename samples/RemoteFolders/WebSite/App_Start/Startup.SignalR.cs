using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace WebSite
{
    public partial class Startup
    {
        public void ConfigureSignalR(IAppBuilder app)
        {
			app.MapSignalR();
        }
    }
}