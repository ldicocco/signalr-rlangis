﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Ldc.SignalR.Rlangis
{
	public static class HubProxyExtensions
	{
		private static TResult JConvert<TResult>(object val)
		{
			if (val is Newtonsoft.Json.Linq.JObject)
			{
				var jo = val as Newtonsoft.Json.Linq.JObject;
				return jo.ToObject<TResult>();
			}
			if (val is Newtonsoft.Json.Linq.JArray)
			{
				var jo = val as Newtonsoft.Json.Linq.JArray;
				return jo.ToObject<TResult>();
			}
			return (TResult)val;
		}

		public static Task<TResult> SendRequest<TResult>(this IHubProxy hubProxy, string serverName, string methodName, params object[] parlist)
		{
			var task = hubProxy.Invoke<object>("_routeToRlangisServer", serverName, methodName, parlist);
			return task.ContinueWith((t) =>
			{
				return JConvert<TResult>(t.Result);
/*				var res = t.Result;
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
				return (TResult)res;*/
			});
		}

		public static void OnRlangisName(this IHubProxy hubProxy, Action<string, string> onRegistered, Action<string, string> onUnregistered)
		{
			hubProxy.On("_registeredName", onRegistered);
			hubProxy.On("_unregisteredName", onUnregistered);
		}
	}
}
