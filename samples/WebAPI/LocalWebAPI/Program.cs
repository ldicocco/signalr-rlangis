using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Hosting;

using Ldc.SignalR.Rlangis;
using Ldc.SignalR.Rlangis.Utils.HttpBridge;

namespace LocalWebAPI
{
	class Program
	{
		static void Main(string[] args)
		{
			string baseAddress = "http://localhost:9000/";

			// Start OWIN host 
			using (WebApp.Start<Startup>(url: baseAddress))
			{
				// Create HttpCient and make a request to api/values 
				HttpClient client = new HttpClient();

				var response = client.GetAsync(baseAddress + "api/values").Result;

				Console.WriteLine(response);
				Console.WriteLine(response.Content.ReadAsStringAsync().Result);

				string hubUrl = "http://localhost:50061";

				var hubConnection = new HubConnection(hubUrl);
				var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
				using (var localHub = new BridgeLocalHub(hubProxy, "apiServer", baseAddress))
				{

					hubConnection.Start().ContinueWith((t) => localHub.Activate()).Wait();

					Console.WriteLine("Ready");

					Console.ReadLine();

				}

			}

			Console.ReadLine();
		}

	}
}
