angular.module('publicWeb', ['ngResource'])
    .factory("rlangis", function ($rootScope) {
        var connection = $.hubConnection();
        var rlangisHubProxy = connection.createRlangisHubProxy();
        function ServerProxy(rlangisHubProxy, name) {
            var _self = this;
            this.hubProxy = rlangisHubProxy;
            this.name = name;
            this.isConnected = false;

            this.hubProxy.onRlangisConnected(function (name, connectionId) {
                $rootScope.$apply(function () { if (name === _self.name) { _self.isConnected = true; } });
            });

            this.hubProxy.onRlangisDisconnected(function (name, connectionId) {
                $rootScope.$apply(function () { if (name === _self.name) { _self.isConnected = false; } });
            });

            this.checkStatus = function () {
                this.hubProxy.invoke('_isActive', this.name).done(function (res) { $rootScope.$apply(function () { _self.isConnected = res; }); });
            };
        }
        rlangisHubProxy.onRlangisConnected(function (name, connectionId) { $rootScope.$emit("serverStatus", name, true); });
        rlangisHubProxy.onRlangisDisconnected(function (name, connectionId) { $rootScope.$emit("serverStatus", name, false); });
        var onRlangisNameConnected = function (onConnected) {
            var func = function (name, connectionId) { $rootScope.$apply(onConnected.apply(rlangisHubProxy, arguments)); };
            rlangisHubProxy.onRlangisConnected(func);
        };
        var onRlangisNameDisconnected = function (onDisconnected) {
            var func = function (name, connectionId) { $rootScope.$apply(onDisconnected.apply(rlangisHubProxy, arguments)); };
            rlangisHubProxy.onRlangisDisconnected(func);
        };
        var getServerProxy = function (name) { return new ServerProxy(rlangisHubProxy, name); };
        var start = function () { return connection.start(); };
        return {
            getServerProxy: getServerProxy,
            start: start,
            onRlangisNameConnected: onRlangisNameConnected,
            onRlangisNameDisconnected: onRlangisNameDisconnected
        };
    })
    .factory('Person', ['$resource', function ($resource) { return $resource('bridge/apiServer/api/persons/:Id', { Id: '@id' }); }])
    .controller('MainCtrl', function ($scope, $http, rlangis, Person) {
        $scope.apiServer = rlangis.getServerProxy('apiServer');
        $scope.toLoad = 0;
        $scope.persons = [];
        $scope.selectedPerson;
        $scope.newPerson = new Person();
        $scope.loadedPerson;
        $scope.loadPersons = function () { $scope.selectedPerson = null; $scope.persons = Person.query(); };
        $scope.selectPerson = function (person) { $scope.selectedPerson = person; };
        $scope.savePerson = function (person) { person.$save(); };
        $scope.addPerson = function (person) { person.$save(function () { $scope.loadPersons(); }); };
        $scope.loadPerson = function (id) {
            Person.get({ Id: id }, function (person) { $scope.loadedPerson = person; });
        };
        rlangis.start().done(function () { $scope.apiServer.checkStatus(); });
    });
