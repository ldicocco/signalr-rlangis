using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldc.SignalR.Rlangis.HubHost.Internals
{
	class PendingRequest
	{
		public Guid Id { get; set; }
		public string ConnectionId { get; set; }
		public TaskCompletionSource<object> Tcs { get; set; }
		public DateTime TimeStarted { get; set; }

		public PendingRequest(string connectionId)
		{
			Id = Guid.NewGuid();
			Tcs = new TaskCompletionSource<object>();
			ConnectionId = connectionId;
			TimeStarted = DateTime.Now;
		}
	}
}
