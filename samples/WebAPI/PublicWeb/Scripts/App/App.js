angular.module('publicWeb', ['ngResource'])
    .factory("rlangis", function ($rootScope) {
        var connection = $.hubConnection();
        var rlangisHubProxy = connection.createRlangisHubProxy();
        var onRlangisNameConnected = function (onConnected) {
            var func = function (name, connectionId) { $rootScope.$apply(onConnected.apply(rlangisHubProxy, arguments)); };
            rlangisHubProxy.onRlangisConnected(func);
        };
        var onRlangisNameDisconnected = function (onDisconnected) {
            var func = function (name, connectionId) { $rootScope.$apply(onDisconnected.apply(rlangisHubProxy, arguments)); };
            rlangisHubProxy.onRlangisDisconnected(func);
        };
        connection.start().done(function () {
            //            alert('Started');
        });
        return {
            onRlangisNameConnected: onRlangisNameConnected,
            onRlangisNameDisconnected: onRlangisNameDisconnected
        };
    })
    .factory('Country', ['$resource', function ($resource) { return $resource('bridge/apiServer/api/countries/:Id', { Id: '@id' });}])
    .controller('MainCtrl', function ($scope, $http, rlangis, Country) {
        rlangis.onRlangisNameConnected(function (name, connectedId) { $scope.connected = true; });
        rlangis.onRlangisNameDisconnected(function (name, connectedId) { $scope.connected = false; });
        $scope.countries = [];
        $scope.connected = false;
        $scope.loadCountries = function () {
            //            alert('LOAD');
            $scope.countries = Country.query();
//            $http.get('bridge/apiServer/api/countries')
//                .success(function (data) { $scope.countries = data; });
        };
    });
