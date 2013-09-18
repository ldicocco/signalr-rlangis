using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Ldc.SignalR.Rlangis;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> TestRequest()
        {
			var obj = await RemoteRequester.Instance.SendRequest("server01", "testMethod", 6, 9, "bing");
			dynamic res = obj;
            return View(res);
        }

    }
}
