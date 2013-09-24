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
				var hubProxy = hubConnection.CreateRlangisHubProxy("server01");
				hubProxy.OnRlangis("testMethod", () =>
				{
					return new Country("Denmark", 5500000);
				});
				hubProxy.OnRlangis("add", (long a, long b) =>
				{
					return a + b;
				});
				hubProxy.OnRlangis("addDouble", (double a, double b) =>
				{
					return a + b;
				});
				hubProxy.OnRlangis("sayHello", (string a) =>
				{
					return "Hello " + a;
				});
				hubProxy.OnRlangis("queryCountries", () =>
				{
					var countries = new Country[] {
							new Country("Italy", 56000000),
							new Country("Denmark", 5500000),
							new Country("U.S.A.", 316285000),
						};
					return countries;
				});
				hubProxy.StartRlangis().Wait();
				Console.WriteLine("Ready");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}
	}
}
