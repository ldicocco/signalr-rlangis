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
		public TaskCompletionSource<object> Tcs { get; set; }
		public DateTime TimeStarted { get; set; }

		public PendingRequest()
		{
		}
	}
}
