
angular.module("employeesManager").config(function ($routeProvider) {
    
    $routeProvider.when("/", {
        templateUrl: "/Home/EmployeesList",
        controller: "employeesListCtrl"
    })
    .when("/login", {
        templateUrl: "/Login/Login",
        controller: "loginCtrl"
    })
    .when("/createEmployee", {
        templateUrl: "/Home/CreateEmployee",
        controller: "createEmployeeCtrl"
    })
    .when("/chartEmployeesPerJob", {
        templateUrl: "/Home/ChartEmployeesPerJob",
        controller: "chartEmployeesPerJobCtrl"
    })
    .when("/chartEmployeesPerAge", {
        templateUrl: "/Home/ChartEmployeesPerAge",
        controller: "chartEmployeesPerAgeCtrl"
    })
    .when("/chartEmployeesPerPerfil", {
        templateUrl: "/Home/ChartEmployeesPerPerfil",
        controller: "chartEmployeesPerPerfilCtrl"
    })
    .when("/filterEmployees", {
        templateUrl: "/Home/FilterEmployees",
        controller: "filterEmployeesCtrl"
    });

});   



