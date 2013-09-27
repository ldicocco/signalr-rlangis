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
				string serverName = "server01";
				string hubUrl = "http://localhost:53588/";

				var hubConnection = new HubConnection(hubUrl);
				var hubProxy = hubConnection.CreateHubProxy("RlangisHub");

				Action<string, string> onRegisteredName = (name, connectionId) =>
					{
						Console.WriteLine("Connected {0} {1}", name, connectionId);
						if (name == serverName)
						{
							SendRequests(hubProxy, serverName);
						}
						else if (name == "server02")
						{
							SendRequests2(hubProxy, "server02");
						}
					};
				Action<string, string> onUnregisteredName = (name, connectionId) =>
					{
						Console.WriteLine("Disconnected {0} {1}", name, connectionId);
					};
				hubProxy.OnRlangisName(onRegisteredName, onUnregisteredName);

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

		static async void SendRequests(IHubProxy rc, string serverName)
		{
			var res1 = await rc.SendRequest<Country>(serverName, "testMethod");
			Console.WriteLine("{0} {1}", res1.Name, res1.Population);
			var res2 = await rc.SendRequest<long>(serverName, "add", 2, 40);
			Console.WriteLine(res2);
			var res21 = await rc.SendRequest<double>(serverName, "addDouble", 2.5, 40.6);
			Console.WriteLine(res21);
			var res3 = await rc.SendRequest<string>(serverName, "sayHello", "Luciano");
			Console.WriteLine(res3);
			var res4 = await rc.SendRequest<IEnumerable<Country>>(serverName, "queryCountries");
			foreach (var item in res4)
			{
				Console.WriteLine("{0} {1}", item.Name, item.Population);
			}
		}
		static async void SendRequests2(IHubProxy rc, string serverName)
		{
			Console.WriteLine("REQUESTS TO " + serverName);
			var res1 = await rc.SendRequest<long>(serverName, "testJS");
			Console.WriteLine(res1);
		}
	}
}
