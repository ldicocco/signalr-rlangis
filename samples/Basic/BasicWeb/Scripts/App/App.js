angular.module('App', ['ngResource'])
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

			this.sendRequest = function () {
				var args = Array.prototype.slice.call(arguments);
				args.splice(0, 0, this.name);
				return this.hubProxy.sendRequest.apply(this.hubProxy, args);
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
	.controller('MainCtrl', function ($scope, $http, rlangis) {
		$scope.server01 = rlangis.getServerProxy('server01');

		$scope.sayHelloPar1;
		$scope.sayHelloRes;
		$scope.sayHello = function () {
			$scope.server01.sendRequest("sayHello", $scope.sayHelloPar1)
				.done(function (data) { $scope.$apply(function () { $scope.sayHelloRes = data; }); });
		};

		$scope.addPar1 = 40;
		$scope.addPar2 = 2;
		$scope.addRes;
		$scope.add = function () {
			$scope.server01.sendRequest("add", $scope.addPar1, $scope.addPar2)
				.done(function (data) { $scope.$apply(function () { $scope.addRes = data; }); });
		};

		$scope.addDoublePar1 = "40.2";
		$scope.addDoublePar2 = "2.2";
		$scope.addDoubleRes;
		$scope.addDouble = function () {
			$scope.server01.sendRequest("addDouble", parseFloat($scope.addDoublePar1), parseFloat($scope.addDoublePar2))
				.done(function (data) { $scope.$apply(function () { $scope.addDoubleRes = data; }); });
		};

		$scope.country;
		$scope.getCountry = function () {
			$scope.server01.sendRequest("getCountry")
				.done(function (data) { $scope.$apply(function () { $scope.country = data; }); });
		};

		$scope.countries;
		$scope.getCountries = function () {
			$scope.server01.sendRequest("getCountries")
				.done(function (data) { $scope.$apply(function () { $scope.countries = data; }); });
		};

		rlangis.start().done(function () { $scope.server01.checkStatus(); });
    });
