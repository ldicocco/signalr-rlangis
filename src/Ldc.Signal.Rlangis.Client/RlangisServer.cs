﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Ldc.Signal.Rlangis.Client
{
	public class RlangisServer
	{
		string _serverName;
		string _hubName;
		HubConnection _hubConnection;
		IHubProxy _hubProxy;
		readonly Dictionary<string, Func<object[], object>> _methodsTable = new Dictionary<string,Func<object[],object>>();

		public RlangisServer(string url, string serverName, string hubName = "RlangisHub")
		{
			_serverName = serverName;
			_hubName = hubName;
			_hubConnection = new HubConnection(url);
			_hubProxy = _hubConnection.CreateHubProxy(_hubName);
			_hubProxy.On<string, string, object[]>("_request", (id, method, parameters) =>
			{
				Console.WriteLine(id + " " + method + " " + parameters);
				if (_methodsTable.ContainsKey(method))
				{
					Console.WriteLine("Method " + method + " found.");
					var res = _methodsTable[method](parameters);
					Console.WriteLine("Result = " + res);
					_hubProxy.Invoke("_result", id, res);
				}
			});
		}

		public Task Start()
		{
			return _hubConnection.Start().ContinueWith((t) => _hubProxy.Invoke("_registerServer", _serverName, ""));
		}

		public void Stop()
		{
			_hubConnection.Stop();
		}

		public void AddMethod(string methodName, Func<object[],object> func)
		{
			_methodsTable[methodName] = func;
		}

		public void AddMethod(string methodName, Func<object> func)
		{
			Console.WriteLine("Encapsulated Func<object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
				{ 
					Console.WriteLine("Called Func<object>");
					return func();
				};
			_methodsTable[methodName] = executedFunc;
		}

		public void AddMethod<T>(string methodName, Func<T,object> func)
		{
			Console.WriteLine("Encapsulated Func<T,object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
				{
					Console.WriteLine("Called " + list);
/*					Console.WriteLine("Called " + list);
					Console.WriteLine("Type expected " + (typeof (T)).Name);
					Console.WriteLine("Type " + list[0].GetType().Name);
					Console.WriteLine("To call " + (T)list[0]);*/
					return func((T)list[0]);
				};
			_methodsTable[methodName] = executedFunc;
		}

		public void AddMethod<T1, T2>(string methodName, Func<T1, T2, object> func)
		{
			Console.WriteLine("Encapsulated Func<T1, T2, object> " + methodName);
			Func<object[], object> executedFunc = (list) =>
				{
					return func((T1)list[0], (T2)list[1]);
				};
			_methodsTable[methodName] = executedFunc;
		}

		public void AddMethod<T1, T2, T3>(string methodName, Func<T1, T2, T3, object> func)
		{
			Func<object[], object> executedFunc = (list) =>
				{
					return func((T1)list[0], (T2)list[1], (T3)list[3]);
				};
			_methodsTable[methodName] = executedFunc;
		}

		public void AddMethod<T1, T2, T3, T4>(string methodName, Func<T1, T2, T3, T4, object> func)
		{
			Func<object[], object> executedFunc = (list) =>
				{
					return func((T1)list[0], (T2)list[1], (T3)list[3], (T4)list[4]);
				};
			_methodsTable[methodName] = executedFunc;
		}
	}
}
