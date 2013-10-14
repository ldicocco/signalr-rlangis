using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Ldc.SignalR.Rlangis
{
	public static class HubConnectionExtensions
	{
		public static RlangisHubProxy CreateRlangisHubProxy(this HubConnection hubConnection, string name, string hubName = "RlangisHub")
		{
			var hubProxy = hubConnection.CreateHubProxy(hubName);
			return new RlangisHubProxy(hubProxy, hubConnection, name);
		}

		public static void TryUntilStart(this HubConnection hubConnection, int delay = 10000)
		{
			while (true)
			{
//			Console.WriteLine("START");
				try
				{
					hubConnection.Start().Wait();
					if (hubConnection.State == ConnectionState.Connected)
					{
						break;
					}
//					Console.WriteLine("Connection failed");
					Task.Delay(delay).Wait();
				}
				catch (Exception ex)
				{
//					Console.WriteLine(ex.Message);
					Task.Delay(delay).Wait();
				}
			}

		}
	}
}
