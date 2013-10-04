using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Ldc.SignalR.Rlangis
{
	public class RlangisHubProxy : IHubProxy
	{
		private HubConnection _hubConnection;
		private IHubProxy _hubProxy;
		private string _name;
		readonly Dictionary<string, Func<object[], object>> _methodsTable = new Dictionary<string, Func<object[], object>>();

		public RlangisHubProxy(IHubProxy hubProxy, HubConnection hubConnection, string name)
		{
			_hubProxy = hubProxy;
			_hubConnection = hubConnection;
			_name = name;
			_hubProxy.On<string, string, object[]>("_request", (id, method, parameters) =>
			{
				Console.WriteLine(id + " " + method + " " + parameters);
				if (_methodsTable.ContainsKey(method))
				{
					Console.WriteLine("Method " + method + " found.");
					var res = _methodsTable[method](parameters);
					Console.WriteLine("Result = " + res);
					Invoke("_result", id, res);
				}
			});
		}

		public Task StartRlangis()
		{
			return _hubConnection.Start().ContinueWith((t) => Invoke("_registerServer", _name, ""));
		}

		public void OnRlangis(string methodName, Func<object[], object> func)
		{
			_methodsTable[methodName] = func;
		}

		public void OnRlangis(string methodName, Func<object> func)
		{
			Console.WriteLine("Encapsulated Func<object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
			{
				Console.WriteLine("Called Func<object>");
				return func();
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T>(string methodName, Func<T, object> func)
		{
			Console.WriteLine("Encapsulated Func<T,object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
			{
				return func((T)list[0]);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T1, T2>(string methodName, Func<T1, T2, object> func)
		{
			Console.WriteLine("Encapsulated Func<T1, T2, object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
			{
				return func((T1)list[0], (T2)list[1]);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T1, T2, T3>(string methodName, Func<T1, T2, T3, object> func)
		{
			Func<object[], object> executedFunc = (list) =>
			{
				return func((T1)list[0], (T2)list[1], (T3)list[3]);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T1, T2, T3, T4>(string methodName, Func<T1, T2, T3, T4, object> func)
		{
			Func<object[], object> executedFunc = (list) =>
			{
				return func((T1)list[0], (T2)list[1], (T3)list[3], (T4)list[4]);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public Task<T> Invoke<T>(string method, params object[] args)
		{
			return _hubProxy.Invoke<T>(method, args);
		}

		public Task Invoke(string method, params object[] args)
		{
			return _hubProxy.Invoke(method, args);
		}

		public Newtonsoft.Json.JsonSerializer JsonSerializer
		{
			get { return _hubProxy.JsonSerializer; }
		}

		public Subscription Subscribe(string eventName)
		{
			return _hubProxy.Subscribe(eventName);
		}

		public Newtonsoft.Json.Linq.JToken this[string name]
		{
			get
			{
				return _hubProxy[name];
			}
			set
			{
				_hubProxy[name] = value;
			}
		}
	}
}
