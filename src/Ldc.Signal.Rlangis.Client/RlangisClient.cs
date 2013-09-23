using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

using Ldc.SignalR.Rlangis;

namespace Ldc.Signal.Rlangis.Client
{
	public class RlangisClient : RlangisHubConnection
	{
		public RlangisClient(string url, string serverName, string hubName = "RlangisHub")
			: base(url, serverName, hubName)
		{
		}

		public Task Start()
		{
			return HubConnection.Start();
		}

		public void Stop()
		{
			HubConnection.Stop();
		}

		public Task<TResult> SendRequest<TResult>(string methodName, params object[] parlist)
		{
			var task = HubProxy.Invoke<object>("_routeToRlangisServer", ServerName, methodName, parlist);
//			return task;
			return task.ContinueWith((t) =>
			{
				var res = t.Result;
				if (t.Result is Newtonsoft.Json.Linq.JObject)
				{
					var jo = res as Newtonsoft.Json.Linq.JObject;
					return jo.ToObject<TResult>();
				}
				if (t.Result is Newtonsoft.Json.Linq.JArray)
				{
					var jo = res as Newtonsoft.Json.Linq.JArray;
					return jo.ToObject<TResult>();
				}
				return (TResult)res;
			});
		}
	}
}
