using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

using Ldc.SignalR.Rlangis.HubHost.Internals;

namespace Ldc.SignalR.Rlangis.HubHost
{
	public class RlangisHubProxy
	{
		private IHubContext _context;
		private string _serverName;
		private string _connectionId;

		private RlangisHubProxy(string serverName = null)
		{
			_context = GlobalHost.ConnectionManager.GetHubContext<RlangisHub>();
		}

/*		public Task<TResult> SendRequest<TResult>(string connectionId, string method, params object[] parlist)
		{
			return PendingRequests.Instance.SendRequest<TResult>(connectionId, method, parlist);
		}*/

		public Task<TResult> SendRequestToName<TResult>(string method, params object[] parlist)
		{
			if (!string.IsNullOrEmpty(_serverName))
			{
				return PendingRequests.Instance.SendRequestToName<TResult>(_serverName, method, parlist);
			}
			else
			{
				return PendingRequests.Instance.SendRequest<TResult>(_connectionId, method, parlist);

			}
		}
	}
}
