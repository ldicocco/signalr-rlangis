using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldc.SignalR.Rlangis.HubHost.Internals
{
	class RlangisServers
	{
		private static volatile RlangisServers _instance;
		private static object syncRoot = new Object();

		private readonly IDictionary<string, RlangisServerEntry> _servers = null;

		public static RlangisServers Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (syncRoot)
					{
						if (_instance == null)
						{
							_instance = new RlangisServers();
						}
					}
				}

				return _instance;
			}
		}

		private RlangisServers()
		{
			_servers = new Dictionary<string, RlangisServerEntry>();
		}

		public void Add(RlangisServerEntry server)
		{
			lock (syncRoot)
			{
				if (!_servers.ContainsKey(server.ConnectionId))
				{
					_servers.Add(server.ConnectionId, server);
				}
			}
		}

		public void AddWithName(RlangisServerEntry server)
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

		public RlangisServerEntry Remove(string connectionId)
		{
			lock (syncRoot)
			{
				if (_servers.ContainsKey(connectionId))
				{
					var se = _servers[connectionId];
					_servers.Remove(connectionId);
					return se;
				}
				else
				{
					return null;
				}
			}
		}

		public RlangisServerEntry RemoveByName(string name)
		{
			lock (syncRoot)
			{
//				var server = _servers.Values.SingleOrDefault(p => p.Name == name);
				var server = _servers.Values.FirstOrDefault(p => p.Name == name);
				if (server != null)
				{
					var se = _servers[server.ConnectionId];
					_servers.Remove(server.ConnectionId);
					return se;
				}
				else
				{
					return null;
				}
			}
		}

		public RlangisServerEntry Get(string connectionId)
		{
			lock (syncRoot)
			{
				RlangisServerEntry server = null;
				_servers.TryGetValue(connectionId, out server);
				return server;
			}
		}

		public RlangisServerEntry GetByName(string name)
		{
			lock (syncRoot)
			{
				return _servers.Values.SingleOrDefault(p => p.Name == name);
			}
		}
	}
}
