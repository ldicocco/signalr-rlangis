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
				string url = "http://localhost:53588/";

				HubConnection hubConnection = new HubConnection(url);
				//			var hubProxy = hubConnection.CreateHubProxy("MyHub1");
				var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
				hubProxy.On<string>("hello", (message) => Console.WriteLine(message));
//				hubConnection.Start().Wait();
				hubConnection.Start().Wait();
				Console.WriteLine("Ready");
				//			hubProxy.Invoke("Hello");
				hubProxy.Invoke("_result", Guid.NewGuid(), new { res = 42 });
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}
	}
}
