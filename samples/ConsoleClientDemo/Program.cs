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
				string url = "http://localhost:53588/";

				var rm = new RlangisClient(url, "server01");
				rm.Start().Wait();
				Console.WriteLine("Ready");
				SendRequests(rm);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}

		static async void SendRequests(RlangisClient rc)
		{
			var res1 = await rc.SendRequest<Country>("testMethod");
			Console.WriteLine(res1.Name);
			var res2 = await rc.SendRequest<long>("add", 2, 40);
			Console.WriteLine(res2);
			var res3 = await rc.SendRequest<IEnumerable<Country>>("queryCountries");
			foreach (var item in res3)
			{
				Console.WriteLine("{0} {1}" , item.Name, item.Population);
			}
		}
	}
}
