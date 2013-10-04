using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldc.SignalR.Rlangis.Utils.HttpBridge
{
	public class BridgeResponse
	{
		public int StatusCode { get; set; }
		public string StatusDescription { get; set; }
		public Dictionary<string, string> Headers { get; set; }
		public string Body { get; set; }

		public BridgeResponse()
		{
			Headers = new Dictionary<string, string>();
		}
	}
}
