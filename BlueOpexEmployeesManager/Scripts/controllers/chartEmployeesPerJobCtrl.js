
angular.module("employeesManager").controller("chartEmployeesPerJobCtrl", function ($scope, $location, employeesAPI) {

    $scope.chartOptions = {
        title: {
            display: true,
            text: 'Funcionários  X  Cargo'
        },
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    };

    function refreshChartData() {
        employeesAPI.getEmployeesPerJob().success(function (data) {
            var labels = [];
            var values = [];
            for (i = 0; i < data.length; i++) {
                labels.push(data[i].Label);
                values.push(data[i].Value);
            }
            $scope.labels = labels;
            $scope.data = values;
        })
        .error(function (data, code) {                     
            if (code == 401) {
                $location.path('/login')
            }
        });
    }

    refreshChartData();
});