
angular.module("employeesManager").controller("employeesListCtrl", function ($scope, $location, employeesAPI) {
    
    function refreshList () {
        employeesAPI.getEmployees().success(function (data) {
            $scope.employees = data;
        })
        .error(function (data, code) {
            //alert(JSON.stringify(data) + ' - ' + JSON.stringify(code));                
            if (code == 401) {
                $location.path('/login')
            }
        });
    };

    refreshList();
});