using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

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

				HubConnection hubConnection = new HubConnection(url);
				var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
				hubProxy.On<string>("hello", (message) => Console.WriteLine(message));
				hubProxy.On<string, string, object>("_request", (id, method, parameters) =>
					{
						Console.WriteLine(id + " " + method + " " + parameters);
						System.Threading.Thread.Sleep(5000);
						hubProxy.Invoke("_result", id, new { res = rnd.Next(42) });
					});
				hubConnection.Start().Wait();
				Console.WriteLine("Ready");
				//			hubProxy.Invoke("Hello");
				//hubProxy.Invoke("_result", Guid.NewGuid(), new { res = 42 });
				hubProxy.Invoke("_registerServer", "server01", "");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}
	}
}
