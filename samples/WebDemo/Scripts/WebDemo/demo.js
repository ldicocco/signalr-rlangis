
$(function () {
    $.hubConnection.fn.createRlangisHubProxy = function (hubName) {
        var hubProxy = this.createHubProxy(hubName || 'RlangisHub');
        hubProxy.onRlangisName = function (onregistered, onunregistered) {
            this.on("_registeredName", onregistered);
            this.on("_unregisteredName", onunregistered);
        };
        hubProxy.sendRequest = function () {
            var args = Array.prototype.slice.call(arguments);
            return this.invoke.call(this, '_routeToRlangisServer', args[0], args[1], args.slice(2));
        };
        hubProxy.startRlangis = function (serverName) {
            return hubProxy.connection.start().then(function () { hubProxy.invoke("_registerServer", name, ""); });
        };
        hubProxy._methodsTable = {};
 /*       hubProxy.on('_request', function (id, method, parameters) {
            if (method in hubProxy._methodsTable) {
                var res = hubProxy._methodsTable[method].apply(hubProxy, parameters);
                hubProxy.invoke('result', id, res);
            }
        });
        hubProxy.onRlangis = function (methodName, func) {
            hubProxy._methodsTable[methodName] = func;
        };*/
        return hubProxy;
    };

    function RlangisLocalHub(hubProxy, name) {
        this.hubProxy = hubProxy;
        this.name = name;
    }

    RlangisLocalHub.prototype.register = function () {

    };

    var addFunc = function () {
        return rlangisHubProxy.sendRequest("server01", "add", 2, 40);
    };

    $('#request').hide();
    $('#request').on('click', function () { addFunc().done(function (res) { $('#result').text(res); }); });

    var connection = $.hubConnection();
    var rlangisHubProxy = connection.createRlangisHubProxy();
    rlangisHubProxy.onRlangisName(
        function (name, connectionId) { $('#server01').text('CONNECTED ' + name + ' ' + connectionId); $('#request').show(); },
        function (name, connectionId) { $('#server01').text('DISCONNECTED ' + name + ' ' + connectionId); $('#request').hide(); });
    //   connection.start().done(function () {
//    rlangisHubProxy.onRlangis("testJS", function () { return 84; });
    connection.start().done(function () {
        //rlangisHubProxy.sendRequest("server01", "add", 2, 40).done(function (res) { alert(res); });
        //alert('Started');
    });
});
