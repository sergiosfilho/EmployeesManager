
angular.module("employeesManager").factory("employeesAPI", function ($http, config) {

    var _postLogin = function (loginData) {
        return $http.post(config.baseUrl + "/api/employeesapi/PostLogin", loginData)
    };

    var _getEmployees = function () {
        return $http.get(config.baseUrl + "/api/employeesapi/GetEmployees");
    };

    var _getJobTitles = function () {
        return $http.get(config.baseUrl + "/api/employeesapi/GetJobTitles");
    };

    var _postNewEmployee = function (newEmployee) {
        var baseUrl = config.baseUrl;
        return $http.post(config.baseUrl + "/api/employeesapi/PostNewEmployee", newEmployee)
    };

    var _getEmployeesPerBirth = function () {
        return $http.get(config.baseUrl + "/api/employeesapi/GetEmployeesPerBirth");
    };

    var _getEmployeesPerJob = function () {
        return $http.get(config.baseUrl + "/api/employeesapi/GetEmployeesPerJob");
    };

    var _getEmployeesByFilter = function (filterData) {
        return $http.post(config.baseUrl + "/api/employeesapi/GetEmployeesByFilter", filterData);
    };

    return {
        getEmployees: _getEmployees,
        postLogin: _postLogin,
        getJobTitles: _getJobTitles,
        postNewEmployee: _postNewEmployee,
        getEmployeesPerBirth: _getEmployeesPerBirth,
        getEmployeesPerJob: _getEmployeesPerJob,
        getEmployeesByFilter: _getEmployeesByFilter
    }
});