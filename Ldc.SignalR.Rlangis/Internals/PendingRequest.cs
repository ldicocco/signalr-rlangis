using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldc.SignalR.Rlangis.Internals
{
	class PendingRequest
	{
		public Guid Id { get; set; }
		public TaskCompletionSource<object> Tcs { get; set; }
		public Type ResultType { get; set; }
		public DateTime TimeStarted { get; set; }

		public PendingRequest()
		{
			ResultType = typeof (object);
		}
	}
}
