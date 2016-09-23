
angular.module("employeesManager").controller("loginCtrl", function ($scope, $location, $http, employeesAPI) {
    
    $scope.onBtnLoginClick = function (loginData) {
        employeesAPI.postLogin(loginData).success(function (data) {
            if (data.Success) {
                $location.path('/')
            }
            else {
                $scope.InvalidPassword = true;
            }
        });
    };

});