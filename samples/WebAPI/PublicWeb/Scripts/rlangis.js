var Ldc = Ldc || {};
if (!Ldc.createNS) {
    // createNS by Kenneth Truyers
    Ldc.createNS = function (namespace) {
        var nsparts = namespace.split(".");
        var parent = Ldc;

        // we want to be able to include or exclude the root namespace so we strip
        // it if it's in the namespace
        if (nsparts[0] === "Ldc") {
            nsparts = nsparts.slice(1);
        }

        // loop through the parts and create a nested namespace if necessary
        for (var i = 0; i < nsparts.length; i++) {
            var partname = nsparts[i];
            // check if the current parent already has the namespace declared
            // if it isn't, then create it
            if (typeof parent[partname] === "undefined") {
                parent[partname] = {};
            }
            // get a reference to the deepest element in the hierarchy so far
            parent = parent[partname];
        }
        // the parent is now constructed with empty namespaces and can be used.
        // we return the outermost namespace
        return parent;
    };

    $.hubConnection.fn.createRlangisHubProxy = function (hubName) {
        var hubProxy = this.createHubProxy(hubName || 'RlangisHub');
        hubProxy.onRlangisConnected = function (onConnected) {
            this.on("_registeredName", onConnected);
        };
        hubProxy.onRlangisDisconnected = function (onDisconnected) {
            this.on("_unregisteredName", onDisconnected);
        };
        hubProxy.sendRequest = function () {
            var args = Array.prototype.slice.call(arguments);
            return this.invoke.call(this, '_routeToRlangisServer', args[0], args[1], args.slice(2));
        };
        return hubProxy;
    };

}


(function () {
    var ns = Ldc.createNS("Ldc.SignalR.Rlangis");
    ns.LocalHub = function (hubProxy, name) {
        var _methodsTable = {};
        var _hubProxy = hubProxy;
        var _name = name || '<inactive>';
        var _isActive = false;

        _hubProxy.on('_request', function (id, method, parameters) {
            if (method in _methodsTable) {
                var res = _methodsTable[method].apply(hubProxy, parameters);
                hubProxy.invoke('_result', id, res);
            }
        });

        var _getHubProxy = function () { return _hubProxy; };
        var _getName = function () { return _name; };
        var _onRlangis = function (methodName, func) {
            _methodsTable[methodName] = func;
        };
        var _activate = function (name) {
            _name = name;
            _isActive = true;
            return hubProxy.invoke("_registerServer", name, "");
        };

        var _deactivate = function () {
            _isActive = false;
            return hubProxy.invoke("_unregisterServer", name, "");
        };

        return {
            getHubProxy: _hubProxy,
            getName: _getName,
            onRlangis: _onRlangis,
            activate: _activate,
            deactivate: _deactivate
        };
    };
})();
