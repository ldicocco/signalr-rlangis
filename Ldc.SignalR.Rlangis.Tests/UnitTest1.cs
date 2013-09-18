using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ldc.SignalR.Rlangis.Internals;

namespace Ldc.SignalR.Rlangis.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			Server server = new Server{ConnectionId = Guid.NewGuid().ToString(), Name = "Server01", Interface = ""};
			Servers.Instance.Add(server);
			Assert.IsNotNull(Servers.Instance.GetByName("Server01"));
		}
	}
}
