using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Ldc.SignalR.Rlangis
{
	public class LocalHub : IHubProxy, IDisposable
	{
		private bool _disposed;
		private int _disposeTimeOut = 5000;
		private IHubProxy _hubProxy;
		private string _name;
		readonly Dictionary<string, Func<object[], object>> _methodsTable = new Dictionary<string, Func<object[], object>>();
		private bool _isActive;

		public LocalHub(IHubProxy hubProxy, string name)
		{
			_disposed = false;
			_hubProxy = hubProxy;
			_name = name;
			_hubProxy.On<string, string, object[]>("_request", (id, method, parameters) =>
			{
				//Console.WriteLine(id + " " + method + " " + parameters);
				if (_methodsTable.ContainsKey(method))
				{
					//Console.WriteLine("Method " + method + " found.");
					var res = _methodsTable[method](parameters);
					//Console.WriteLine("Result = " + res);
					Invoke("_result", id, res);
				}
			});
		}

		public Task Activate()
		{
			return Invoke("_registerServer", _name, "").ContinueWith((t) => _isActive = true);
		}

		public Task Deactivate()
		{
			if (_isActive)
			{
				return Invoke("_unregisterServer", _name).ContinueWith((t) => _isActive = false);
			}
			else
			{
				return Task.FromResult(0);
			}
		}

		public void OnRlangis(string methodName, Func<object[], object> func)
		{
			_methodsTable[methodName] = func;
		}

		public void OnRlangis(string methodName, Func<object> func)
		{
			//Console.WriteLine("Encapsulated Func<object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
			{
//				Console.WriteLine("Called Func<object>");
				return func();
			};
			_methodsTable[methodName] = executedFunc;
		}

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

		public void OnRlangis<T>(string methodName, Func<T, object> func)
		{
			//Console.WriteLine("Encapsulated Func<T,object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
			{
				T p1 = JConvert<T>(list[0]);
				return func(p1);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T1, T2>(string methodName, Func<T1, T2, object> func)
		{
			//Console.WriteLine("Encapsulated Func<T1, T2, object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
			{
				T1 p1 = JConvert<T1>(list[0]);
				T2 p2 = JConvert<T2>(list[1]);
				return func(p1, p2);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T1, T2, T3>(string methodName, Func<T1, T2, T3, object> func)
		{
			Func<object[], object> executedFunc = (list) =>
			{
				T1 p1 = JConvert<T1>(list[0]);
				T2 p2 = JConvert<T2>(list[1]);
				T3 p3 = JConvert<T3>(list[2]);
				return func(p1, p2, p3);
			};
			_methodsTable[methodName] = executedFunc;
		}

		public void OnRlangis<T1, T2, T3, T4>(string methodName, Func<T1, T2, T3, T4, object> func)
		{
			Func<object[], object> executedFunc = (list) =>
			{
				T1 p1 = JConvert<T1>(list[0]);
				T2 p2 = JConvert<T2>(list[1]);
				T3 p3 = JConvert<T3>(list[2]);
				T4 p4 = JConvert<T4>(list[3]);
				return func(p1, p2, p3, p4);
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

		public void Dispose()
		{
			Dispose(true);

			// Call SupressFinalize in case a subclass implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these  
			// operations, as well as in your methods that use the resource. 
			if (!_disposed)
			{
				if (disposing)
				{
					if (_hubProxy != null)
					{
						if (_isActive)
						{
							Deactivate().Wait(_disposeTimeOut);
						}
						//Console.WriteLine("Object disposed.");
					}
				}

				_hubProxy = null;
				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}
	}
}
