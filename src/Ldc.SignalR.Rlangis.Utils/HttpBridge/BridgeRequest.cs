using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldc.SignalR.Rlangis.Utils.HttpBridge
{
	public class BridgeRequest
	{
		public string Url {get; set; }
		public string HttpMethod {get; set; }
		public Dictionary<string, string> Headers { get; set; }

		public BridgeRequest()
		{
			Headers = new Dictionary<string, string>();
		}
	}
}
