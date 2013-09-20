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
			var res0 = await RemoteRequester.Instance.SendRequestToName<object>("server01", "testMethod");
			ViewBag.TestMethodRes = res0;

			var res1 = await RemoteRequester.Instance.SendRequestToName<long>("server01", "add", 8, 34);
			ViewBag.AddRes = res1;

			var res2 = await RemoteRequester.Instance.SendRequestToName<string>("server01", "sayHello", "Luciano");
			ViewBag.SayHelloRes = res2;

			var res3 = await RemoteRequester.Instance.SendRequestToName<object>("server01", "queryCountries");
//			ViewBag.QueryCountriesRes = ((Newtonsoft.Json.Linq.JArray)res3).ToArray<dynamic>();
			ViewBag.QueryCountriesRes = res3;

			return View();
		}

	}
}
