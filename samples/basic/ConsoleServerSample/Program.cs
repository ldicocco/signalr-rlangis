using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

using Ldc.SignalR.Rlangis;

using DemoInfrastructure;

namespace ConsoleServerSample
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				string hubUrl = "http://localhost:53588/";

				var hubConnection = new HubConnection(hubUrl);
				var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
				using (var localHub = new LocalHub(hubProxy, "server01"))
				{
					localHub.OnRlangis("testMethod", () =>
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
					hubConnection.Start().ContinueWith((t) => localHub.Activate()).Wait();
					Console.WriteLine("Ready");

					Console.ReadLine();
				}
				//hubProxy.Invoke("_unregisterServer", "server01").Wait();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			Console.ReadLine();

		}
	}
}
