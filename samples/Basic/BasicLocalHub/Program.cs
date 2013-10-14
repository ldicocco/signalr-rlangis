using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

using Ldc.SignalR.Rlangis;

namespace BasicLocalHub
{
	class Program
	{
		static void Main(string[] args)
		{
			string hubUrl = "http://localhost:54082/";

			var hubConnection = new HubConnection(hubUrl);
			var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
			using (var localHub = new LocalHub(hubProxy, "server01"))
			{
				localHub.OnRlangis("getCountry", () =>
				{
					return new Country("Denmark", 5500000);
				});
				localHub.OnRlangis("add", (long a, long b) =>
				{
					return a + b;
				});
				localHub.OnRlangis("addDouble", (double a, double b) =>
				{
					return a + b;
				});
				localHub.OnRlangis("sayHello", (string a) =>
				{
					return "Hello " + a;
				});
				localHub.OnRlangis("queryCountries", () =>
				{
					var countries = new Country[] {
							new Country("Italy", 56000000),
							new Country("Denmark", 5500000),
							new Country("U.S.A.", 316285000),
						};
					return countries;
				});
				hubConnection.TryUntilStart();
				localHub.Activate().Wait();

				Console.WriteLine("Ready");

				Console.ReadLine();
			}

		}
	}
}
