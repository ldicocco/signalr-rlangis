angular.module('publicWeb', [])
    .factory("rlangis", function ($rootScope) {
        var connection = $.hubConnection();
        var rlangisHubProxy = connection.createRlangisHubProxy();
        connection.start().done(function () {
            //            alert('Started');
        });
        return {
            onRlangisNameConnected: function (onConnected) {
                var func = function (name, connectionId) { $rootScope.$apply(onConnected.apply(rlangisHubProxy, arguments)); };
                rlangisHubProxy.onRlangisConnected(func);
            },
            onRlangisNameDisconnected: function (onDisconnected) {
                var func = function (name, connectionId) { $rootScope.$apply(onDisconnected.apply(rlangisHubProxy, arguments)); };
                rlangisHubProxy.onRlangisDisconnected(func);
            }
        };
    })
    .controller('MainCtrl', function ($scope, $http, rlangis) {
        rlangis.onRlangisNameConnected(function (name, connectedId) { $scope.connected = true; });
        rlangis.onRlangisNameDisconnected(function (name, connectedId) { $scope.connected = false; });
        $scope.countries = [];
        $scope.connected = false;
        $scope.loadCountries = function () {
            //            alert('LOAD');
            $http.get('bridge/apiServer/api/countries')
                .success(function (data) { $scope.countries = data; });
        };
    });
