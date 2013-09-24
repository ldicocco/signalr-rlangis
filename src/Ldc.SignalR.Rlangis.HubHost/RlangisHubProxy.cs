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
		private string _serverName;
		private string _connectionId;

		public RlangisHubProxy(string serverName = null)
		{
			_serverName = serverName;
		}

		public Task<TResult> SendRequestToName<TResult>(string method, params object[] parlist)
		{
			if (!string.IsNullOrEmpty(_serverName))
			{
				return RlangisRequests.Instance.SendRequestToServer<TResult>(_serverName, method, parlist);
			}
			else
			{
				return RlangisRequests.Instance.SendRequest<TResult>(_connectionId, method, parlist);

			}
		}
	}
}
