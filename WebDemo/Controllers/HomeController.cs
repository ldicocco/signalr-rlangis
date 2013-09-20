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
			//			var obj = await RemoteRequester.Instance.SendRequestToName("server01", "testMethod", 6, 9, "bing");
			var res1 = await RemoteRequester.Instance.SendRequestToName("server01", "add", 8, 34);
			ViewBag.AddRes = (long)res1;

			var res2 = await RemoteRequester.Instance.SendRequestToName("server01", "sayHello", "Luciano");
			ViewBag.SayHelloRes = (string)res2;
			return View();
		}

	}
}
