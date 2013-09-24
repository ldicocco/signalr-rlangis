using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Ldc.SignalR.Rlangis
{
	public static class HubProxyExtensions
	{
		public static Task<TResult> SendRequest<TResult>(this IHubProxy hubProxy, string serverName, string methodName, params object[] parlist)
		{
			var task = hubProxy.Invoke<object>("_routeToRlangisServer", serverName, methodName, parlist);
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
