using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldc.SignalR.Rlangis.Internals
{
	class Servers
	{
		private static volatile Servers _instance;
		private static object syncRoot = new Object();

		private readonly IDictionary<string, Server> _servers = null;

		public static Servers Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (syncRoot)
					{
						if (_instance == null)
						{
							_instance = new Servers();
						}
					}
				}

				return _instance;
			}
		}

		private Servers()
		{
			_servers = new Dictionary<string, Server>();
		}

		public void Add(Server server)
		{
			lock (syncRoot)
			{
				if (!_servers.ContainsKey(server.ConnectionId))
				{
					_servers.Add(server.ConnectionId, server);
				}
			}
		}

		public void AddWithName(Server server)
		{
			lock (syncRoot)
			{
				if (!_servers.ContainsKey(server.ConnectionId))
				{
					if (!_servers.Values.Any(p => p.Name == server.Name))
					{
						_servers.Add(server.ConnectionId, server);
					}
				}
			}
		}

		public void SetName(string connectionId, string name)
		{
			lock (syncRoot)
			{
				if (_servers.ContainsKey(connectionId))
				{
					var server = _servers[connectionId];
					server.Name = name;
				}
			}
		}

		public void Remove(string connectionId)
		{
			lock (syncRoot)
			{
				if (_servers.ContainsKey(connectionId))
				{
					_servers.Remove(connectionId);
				}
			}
		}

		public void RemoveByName(string name)
		{
			lock (syncRoot)
			{
//				var server = _servers.Values.SingleOrDefault(p => p.Name == name);
				var server = _servers.Values.FirstOrDefault(p => p.Name == name);
				if (server != null)
				{
					_servers.Remove(server.ConnectionId);
				}
			}
		}

		public Server Get(string connectionId)
		{
			lock (syncRoot)
			{
				Server server = null;
				_servers.TryGetValue(connectionId, out server);
				return server;
			}
		}

		public Server GetByName(string name)
		{
			lock (syncRoot)
			{
				return _servers.Values.SingleOrDefault(p => p.Name == name);
			}
		}
	}
}
