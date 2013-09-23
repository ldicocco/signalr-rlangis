using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Ldc.SignalR.Rlangis
{
	public class RlangisHubConnection
	{
		string _serverName;
		string _hubName;
		HubConnection _hubConnection;
		IHubProxy _hubProxy;

		public RlangisHubConnection(string hubUrl, string serverName, string hubName = "RlangisHub")
		{
			_serverName = serverName;
			_hubName = hubName;
			_hubConnection = new HubConnection(hubUrl);
			_hubProxy = _hubConnection.CreateHubProxy(_hubName);
		}

		public string ServerName
		{
			get
			{
				return _serverName;
			}
		}

		public HubConnection HubConnection
		{
			get
			{
				return _hubConnection;
			}
		}

		public IHubProxy HubProxy
		{
			get
			{
				return _hubProxy;
			}
		}
	}
}
