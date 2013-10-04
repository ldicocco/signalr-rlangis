﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Ldc.SignalR.Rlangis.HubHost;

using DemoInfrastructure;

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
			var proxy = new RlangisHubLocal("server01");
			var res0 = await proxy.SendRequestToName<Country>("testMethod");
			ViewBag.TestMethodRes = res0;

			var res1 = await proxy.SendRequestToName<long>("add", 8, 34);
			ViewBag.AddRes = res1;

			var res2 = await proxy.SendRequestToName<string>("sayHello", "Luciano");
			ViewBag.SayHelloRes = res2;

			var res3 = await proxy.SendRequestToName<IEnumerable<Country>>("queryCountries");
			ViewBag.QueryCountriesRes = res3;

			return View();
		}

	}
}