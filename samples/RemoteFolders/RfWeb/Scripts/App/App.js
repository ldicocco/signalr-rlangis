(function () {

	//angular module
	var myApp = angular.module('myApp', ['angularTreeview', 'ldcRlangis']);

	//test controller
	myApp.controller('myController', function ($scope, rlangis) {
		$scope.server01 = rlangis.getServerProxy('server01');

		//test tree model 1
		$scope.roleList1 = [
			{
				"roleName": "User", "roleId": "role1", "children": [
				{ "roleName": "subUser1", "roleId": "role11", "children": [] },
				{
					"roleName": "subUser2", "roleId": "role12", "children": [
					{
						"roleName": "subUser2-1", "roleId": "role121", "children": [
						{ "roleName": "subUser2-1-1", "roleId": "role1211", "children": [] },
						{ "roleName": "subUser2-1-2", "roleId": "role1212", "children": [] }
						]
					}
					]
				}
				]
			},

			{ "roleName": "Admin", "roleId": "role2", "children": [] },

			{ "roleName": "Guest", "roleId": "role3", "children": [] }
		];

		//test tree model 2
		$scope.roleList2 = [
			{
				"roleName": "User", "roleId": "role1", "children": [
				{ "roleName": "subUser1", "roleId": "role11", "collapsed": true, "children": [] },
				{
					"roleName": "subUser2", "roleId": "role12", "collapsed": true, "children": [
					{
						"roleName": "subUser2-1", "roleId": "role121", "children": [
						{ "roleName": "subUser2-1-1", "roleId": "role1211", "children": [] },
						{ "roleName": "subUser2-1-2", "roleId": "role1212", "children": [] }
						]
					}
					]
				}
				]
			},

			{
				"roleName": "Admin", "roleId": "role2", "children": [
				{ "roleName": "subAdmin1", "roleId": "role11", "collapsed": true, "children": [] },
				{
					"roleName": "subAdmin2", "roleId": "role12", "children": [
					{
						"roleName": "subAdmin2-1", "roleId": "role121", "children": [
						{ "roleName": "subAdmin2-1-1", "roleId": "role1211", "children": [] },
						{ "roleName": "subAdmin2-1-2", "roleId": "role1212", "children": [] }
						]
					}
					]
				}
				]
			},

			{
				"roleName": "Guest", "roleId": "role3", "children": [
				{ "roleName": "subGuest1", "roleId": "role11", "children": [] },
				{
					"roleName": "subGuest2", "roleId": "role12", "collapsed": true, "children": [
					{
						"roleName": "subGuest2-1", "roleId": "role121", "children": [
						{ "roleName": "subGuest2-1-1", "roleId": "role1211", "children": [] },
						{ "roleName": "subGuest2-1-2", "roleId": "role1212", "children": [] }
						]
					}
					]
				}
				]
			}
		];

		$scope.$watch('tree01.currentNode', function (newObj, oldObj) {
			if ($scope.tree01 && angular.isObject($scope.tree01.currentNode)) {
				console.log('Node Selected!!');
				console.log($scope.tree01.currentNode);
			}
		}, false);

		$scope.addNodes = function () {
			$scope.tree01.currentNode.children.push({ "roleName": "Guest 40", "roleId": "role201", "children": [] });
			$scope.roleList1.push({ "roleName": "Guests", "roleId": "role101", "children": [] });
		};

		rlangis.start().done(function () { alert("OK"); $scope.server01.checkStatus(); });
	});

})();

