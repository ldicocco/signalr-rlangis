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
	}
}
