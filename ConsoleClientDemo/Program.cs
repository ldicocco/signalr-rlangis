using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

using Ldc.Signal.Rlangis.Client;

using DemoInfrastructure;

namespace ConsoleClientDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var rnd = new Random();
				string url = "http://localhost:53588/";
				Func<object[], object> func = (parms) => { return new { res = rnd.Next(42) }; };

				var rm = new RemoteMethods(url, "server01");
				rm.AddMethod("testMethod", () =>
					{
						return new Country("Denmark", 5500000);
					});
				rm.AddMethod("add", (long a, long b) =>
					{
						return a + b;
					});
				rm.AddMethod("sayHello", (string a) =>
					{
						return "Hello " + a;
					});
				rm.AddMethod("queryCountries", () =>
					{
						var countries = new Country[] {
							new Country("Italy", 56000000),
							new Country("Denmark", 5500000),
							new Country("U.S.A.", 316285000),
						};
						return countries;
					});
				rm.Start().Wait();
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
