angular.module('App', ['ngResource', 'ldcRlangis'])
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
