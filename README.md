Rlangis
===============

A simple SignalR utility to get results from clients
---------------

In SignalR hubs it is possible for a client to get a return value
from a hub method, but it is not possible to get a return value
from a client method.

This is generally fine, but there are scenarios where having a result from a client could be useful.

One of these scenarios is having some resources on a machine behind firewalls, for example a database.
With SignalR is easy to coll methods that interact with these resources, but you cannot directly query the database,
as methods on the clients cannot give a result.
It is relatively easy to simulate a result, but it involves some annoying boilerplate code.

Samples
-------

One console app, possibly behind firewalls, can invoke and get results from another app in a different machine, possibly behind a different firewall.

## Server
	string hubUrl = "http://localhost:53588/";

	var hubConnection = new HubConnection(hubUrl);
	var hubProxy = hubConnection.CreateRlangisHubProxy("server01");
	hubProxy.OnRlangis("testMethod", () =>
	{
		return new Country("Denmark", 5500000);
	});
	hubProxy.OnRlangis("add", (long a, long b) =>
	{
		return a + b;
	});
	hubProxy.OnRlangis("addDouble", (double a, double b) =>
	{
		return a + b;
	});
	hubProxy.OnRlangis("sayHello", (string a) =>
	{
		return "Hello " + a;
	});
	hubProxy.OnRlangis("queryCountries", () =>
	{
		var countries = new Country[] {
		new Country("Italy", 56000000),
		new Country("Denmark", 5500000),
		new Country("U.S.A.", 316285000),
		};
		return countries;
	});
	hubProxy.StartRlangis().Wait();
	Console.WriteLine("Ready");

## Client
	string serverName = "server01";
	string hubUrl = "http://localhost:53588/";

	var hubConnection = new HubConnection(hubUrl);
	var hubProxy = hubConnection.CreateHubProxy("RlangisHub");
	
	Action<string, string> onRegisteredName = (name, connectionId) =>
		{
			Console.WriteLine("Connected {0} {1}", name, connectionId);
			if (name == serverName)
			{
				SendRequests(hubProxy, serverName);
			}
		};
	Action<string, string> onUnregisteredName = (name, connectionId) =>
		{
			Console.WriteLine("Disconnected {0} {1}", name, connectionId);
		};
	hubProxy.OnRlangisName(onRegisteredName, onUnregisteredName);

	hubConnection.Start().Wait();
	Console.WriteLine("Ready");

	static async void SendRequests(IHubProxy rc, string serverName)
	{
		var res1 = await rc.SendRequest<Country>(serverName, "testMethod");
		Console.WriteLine("{0} {1}", res1.Name, res1.Population);
		var res2 = await rc.SendRequest<long>(serverName, "add", 2, 40);
		Console.WriteLine(res2);
		var res21 = await rc.SendRequest<double>(serverName, "addDouble", 2.5, 40.6);
		Console.WriteLine(res21);
		var res3 = await rc.SendRequest<string>(serverName, "sayHello", "Luciano");
		Console.WriteLine(res3);
		var res4 = await rc.SendRequest<IEnumerable<Country>>(serverName, "queryCountries");
		foreach (var item in res4)
		{
			Console.WriteLine("{0} {1}" , item.Name, item.Population);
		}
	}



