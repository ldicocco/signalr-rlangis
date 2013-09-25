using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

using Ldc.SignalR.Rlangis;

using DemoInfrastructure;

namespace ConsoleClientDemo
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
				Action<string, string> onRegisteredName = (name, connectionId) =>
					{
						Console.WriteLine("Connected {0} {1}", name, connectionId);
						if (name == "server01")
						{
							SendRequests(hubProxy);
						}
					};
				Action<string, string> onUnregisteredName = (name, connectionId) =>
					{
						Console.WriteLine("Disconnected {0} {1}", name, connectionId);
					};
				hubProxy.OnRlangisName(onRegisteredName, onUnregisteredName);

/*				hubProxy.On<string, string>("_registeredName", (name, connectionId) =>
					{
						Console.WriteLine("Connected {0} {1}", name, connectionId);
						if (name == "server01")
						{
							SendRequests(hubProxy);
						}
					});
				hubProxy.On<string, string>("_unregisteredName", (name, connectionId) =>
					{
						Console.WriteLine("Disconnected {0} {1}", name, connectionId);
					});*/
				hubConnection.Start().Wait();
				Console.WriteLine("Ready");
				//				SendRequests(hubProxy);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}

		static async void SendRequests(IHubProxy rc)
		{
			var res1 = await rc.SendRequest<Country>("server01", "testMethod");
			Console.WriteLine("{0} {1}", res1.Name, res1.Population);
			var res2 = await rc.SendRequest<long>("server01", "add", 2, 40);
			Console.WriteLine(res2);
			var res21 = await rc.SendRequest<double>("server01", "addDouble", 2.5, 40.6);
			Console.WriteLine(res21);
			var res3 = await rc.SendRequest<string>("server01", "sayHello", "Luciano");
			Console.WriteLine(res3);
			var res4 = await rc.SendRequest<IEnumerable<Country>>("server01", "queryCountries");
			foreach (var item in res4)
			{
				Console.WriteLine("{0} {1}", item.Name, item.Population);
			}
		}
	}
}
