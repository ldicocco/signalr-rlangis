using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

using Ldc.SignalR.Rlangis.Internals;

namespace Ldc.SignalR.Rlangis
{
	public class RemoteRequester : IRemoteRequester
	{
		// Singleton instance
		private readonly static Lazy<RemoteRequester> _instance = new Lazy<RemoteRequester>(
			() => new RemoteRequester(GlobalHost.ConnectionManager.GetHubContext<RlangisHub>()));

		static public RemoteRequester Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private IHubContext _context;

		private RemoteRequester(IHubContext context)
		{
			_context = context;
		}

		public Task<TResult> SendRequest<TResult>(string connectionId, string method, params object[] parlist)
		{
			var pr = new PendingRequest();
			pr.Id = Guid.NewGuid();
			pr.Tcs = new TaskCompletionSource<object>();
			pr.TimeStarted = DateTime.Now;
			PendingRequests.Instance.Add(pr);
			_context.Clients.Client(connectionId)._request(pr.Id, method, parlist);
			return pr.Tcs.Task.ContinueWith((t) =>
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
					return (TResult) res;
				});
		}

		public Task<TResult> SendRequestToName<TResult>(string serverName, string method, params object[] parlist)
		{
			var server = Servers.Instance.GetByName(serverName);
			if (server != null)
			{
				return SendRequest<TResult>(server.ConnectionId, method, parlist);
			}
			else
			{
				return null;
			}
		}
	}
}
