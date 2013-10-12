using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

using Ldc.SignalR.Rlangis;

namespace LocalServer
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				string hubUrl = "http://localhost:49804/";

				var hubConnection = new HubConnection(hubUrl);
				var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
				using (var localHub = new LocalHub(hubProxy, "server01"))
				{
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
					//					hubConnection.Start().ContinueWith((t) => localHub.Activate()).Wait();
					hubConnection.TryUntilStart();
					localHub.Activate().Wait();

					Console.WriteLine("Ready");

					Console.ReadLine();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			Console.ReadLine();
		}
	}
}
