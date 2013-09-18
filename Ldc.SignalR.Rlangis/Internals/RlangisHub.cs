using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Ldc.SignalR.Rlangis.Internals
{
	public class RlangisHub : Hub
	{
		public override Task OnConnected()
		{
			return base.OnConnected();
		}

		public override Task OnDisconnected()
		{
			Servers.Instance.Remove(Context.ConnectionId);
			return base.OnDisconnected();
		}

		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}

		public void _registerServer(string name, string interfaces)
		{
			var server = new Server { Name = name, Interface = interfaces, ConnectionId = Context.ConnectionId };
			Servers.Instance.AddWithName(server);
		}

		public void _unregisterServer(string name)
		{
			Servers.Instance.RemoveByName(name);
		}

		public void _result(Guid id, Object result)
		{
			var pr = PendingRequests.Instance.Get(id);
			if (pr != null)
			{
				pr.Tcs.SetResult(result);
			}
//			Clients.All.hello("Welcome " + id);
		}
	}
}
