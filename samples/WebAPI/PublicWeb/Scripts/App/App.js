angular.module('publicWeb', [])
    .controller('MainCtrl', function ($scope, $http) {
        $scope.countries = [];
        $scope.loadCountries = function () {
            //            alert('LOAD');
            $http.get('bridge/apiServer/api/countries')
                .success(function (data) { $scope.countries = data; });
        }
    });
