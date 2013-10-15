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
				localHub.OnRlangis("sayHello", (string a) =>
				{
					return "Hello " + a;
				});
				localHub.OnRlangis("add", (long a, long b) =>
				{
					return a + b;
				});
				localHub.OnRlangis("addDouble", (double a, double b) =>
				{
					return a + b;
				});
				localHub.OnRlangis("getCountry", () =>
				{
					return new Country("Denmark", 5550142);
				});
				localHub.OnRlangis("getCountries", () =>
				{
					var countries = new List<Country> {
							new Country("Italy", 59685227),
							new Country("Denmark", 5550142),
							new Country("U.S.A.", 316285000),
							new Country("Norway", 5051518),
							new Country("Sweden", 9596436),
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
