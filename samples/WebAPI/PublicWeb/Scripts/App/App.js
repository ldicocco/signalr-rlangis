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
    .factory('Country', ['$resource', function ($resource) { return $resource('bridge/apiServer/api/countries/:Id', { Id: '@id' }); }])
    .controller('MainCtrl', function ($scope, $http, rlangis, Country) {
        $scope.apiServer = rlangis.getServerProxy('apiServer');
        $scope.toLoad = 0;
        $scope.countries = [];
        $scope.selectedCountry;
        $scope.loadedCountry;
        $scope.loadCountries = function () { $scope.countries = Country.query(); };
        $scope.selectCountry = function (country) { $scope.selectedCountry = country; };
        $scope.saveCountry = function (country) { country.$save(); };
        $scope.loadCountry = function (id) {
            Country.get({ Id: id }, function (country) { $scope.loadedCountry = country; });
        };
        rlangis.start().done(function () { $scope.apiServer.checkStatus(); });
    });
