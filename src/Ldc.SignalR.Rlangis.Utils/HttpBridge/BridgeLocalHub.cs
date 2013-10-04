using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.SignalR.Client;

using Ldc.SignalR.Rlangis;
using Ldc.SignalR.Rlangis.Utils.HttpBridge;

namespace Ldc.SignalR.Rlangis.Utils.HttpBridge
{
	public class BridgeLocalHub : LocalHub
	{
		private string _baseUrl;

		public BridgeLocalHub(IHubProxy hubProxy, string name, string baseUrl)
			: base(hubProxy, name)
		{
			_baseUrl = baseUrl;

			this.OnRlangis("httpBridge", (BridgeRequest req) =>
			{
				PrintBridgeRequest(req);
				//						return GetResponse(req);
				var res = new BridgeResponse();
				HttpWebRequest webRequest = CreateHttpWebRequest(req);
				var webResponse = (HttpWebResponse)webRequest.GetResponseAsync().Result;
				PrintHttpWebResponse(webResponse);
				var stream = new System.IO.StreamReader(webResponse.GetResponseStream());
				var result = stream.ReadToEnd();

				res.StatusCode = (int)webResponse.StatusCode;
				res.StatusDescription = webResponse.StatusDescription;
				res.Body = result;
				foreach (var key in webResponse.Headers.AllKeys)
				{
					res.Headers[key] = webResponse.Headers[key];
				}
				Console.WriteLine("{0} {1}", res.StatusCode, res.Body);
				return res;
			});
		}

		private HttpWebRequest CreateHttpWebRequest(BridgeRequest req)
		{
			var protectedHeaders = new Dictionary<string, Action<string, HttpWebRequest>> {
				{"Accept", (val, hwr) => hwr.Accept = val},
//				{"Accept", (val, hwr) => hwr.Accept = "application/json"},
				{"Connection", (val, hwr) => {}},
				{"Content-Length", (val, hwr) => hwr.ContentLength = long.Parse(val)},
				{"Date", (val, hwr) => {}},
				{"Expect", (val, hwr) => hwr.Expect = val},
				{"Host", (val, hwr) => {}},
				{"If-Modified-Since", (val, hwr) => {}},
				{"Range", (val, hwr) => {}},
				{"Referer", (val, hwr) => hwr.Referer = val},
				{"Transfer-Encoding", (val, hwr) => hwr.TransferEncoding = val},
				{"User-Agent", (val, hwr) => hwr.UserAgent = val},
				{"Proxy-Connection", (val, hwr) => {}},
			};
			string url = _baseUrl + req.Url;
			Console.WriteLine(url);
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Method = req.HttpMethod;
			foreach (var pair in req.Headers)
			{
				if (!protectedHeaders.ContainsKey(pair.Key))
				{
					Console.WriteLine("{0} included", pair.Key);
					webRequest.Headers[pair.Key] = pair.Value;
				}
				else
				{
					Console.WriteLine("{0} applied", pair.Key);
					protectedHeaders[pair.Key](pair.Value, webRequest);
				}
			}
			return webRequest;
		}

		private void PrintBridgeRequest(BridgeRequest req)
		{
			Console.WriteLine("httpBridge\nUrl {0}\nHttpMethod {1}\nHeaders count {2}", req.Url, req.HttpMethod, req.Headers.Count);
			foreach (var pair in req.Headers)
			{
				Console.WriteLine("{0} {1}", pair.Key, pair.Value);
			}
		}

		private void PrintHttpWebResponse(HttpWebResponse res)
		{
			Console.WriteLine("\nHttpWebResponse");
			Console.WriteLine("{0} {1}", (int)res.StatusCode, res.StatusDescription);
			foreach (var key in res.Headers.AllKeys)
			{
				Console.WriteLine("{0} {1}", key, res.Headers[key]);
			}
		}
		/*
				private async Task<BridgeResponse> GetResponse(BridgeRequest req)
				{
					var res = new BridgeResponse();
					string url = _baseUrl + req.Url;
					Console.WriteLine(url);
					HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
					webRequest.Method = req.HttpMethod;
					Console.WriteLine("BING");
					var wr = await webRequest.GetResponseAsync();
					var webResponse = (HttpWebResponse)wr;
					Console.WriteLine("BONG");
					var stream = new System.IO.StreamReader(webResponse.GetResponseStream());
					var result = stream.ReadToEnd();

					res.StatusCode = (int)webResponse.StatusCode;
					res.Body = result;
					Console.WriteLine("{0} {1}", res.StatusCode, res.Body);
					return res;
				}*/
	}
}
