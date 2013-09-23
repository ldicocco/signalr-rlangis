﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Ldc.SignalR.Rlangis.HubHost.Internals
{
	// Singleton
	class PendingRequests
	{
		private static object syncRoot = new Object();
		private static volatile PendingRequests _instance;
		private IHubContext _context;

		private readonly IDictionary<Guid, PendingRequest> _requests = null;

		public static PendingRequests Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (syncRoot)
					{
						if (_instance == null)
						{
							_instance = new PendingRequests();
						}
					}
				}

				return _instance;
			}
		}

		private PendingRequests()
		{
			_requests = new Dictionary<Guid, PendingRequest>();
			_context = GlobalHost.ConnectionManager.GetHubContext<RlangisHub>();
		}

		public void Add(PendingRequest request)
		{
			lock (syncRoot)
			{
				_requests.Add(request.Id, request);
			}
		}

		public void Remove(Guid id)
		{
			lock (syncRoot)
			{
				_requests.Remove(id);
			}
		}

		public PendingRequest Get(Guid id)
		{
			lock (syncRoot)
			{
				PendingRequest request = null;
				_requests.TryGetValue(id, out request);
				return request;
			}
		}

		public Task<TResult> SendRequest<TResult>(string connectionId, string method, params object[] parlist)
		{
			var pr = new PendingRequest(connectionId);
			Add(pr);
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
				return (TResult)res;
			});
		}

		public Task<TResult> SendRequestToName<TResult>(string serverName, string method, params object[] parlist)
		{
			var server = RlangisServers.Instance.GetByName(serverName);
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
