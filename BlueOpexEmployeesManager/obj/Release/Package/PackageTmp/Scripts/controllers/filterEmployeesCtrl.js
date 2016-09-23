
angular.module("employeesManager").controller("filterEmployeesCtrl", function ($scope, $location, employeesAPI) {

    function refreshViewByFilter(filterData) {        
        employeesAPI.getEmployeesByFilter(filterData).success(function (data) {
            refreshChart(data);
            $scope.tableData = data.TableData;
        })
        .error(function (data, code) {
            if (code == 401) {
                $location.path('/login')
            }
        });
    }

    //carrega dados inicialmente sem nenhum filtro
    refreshViewByFilter(null);

    $scope.onBtnFilterClick = function (filterData) {
        refreshViewByFilter(filterData);
    };

    //carrega cargos para o select do filtro
    employeesAPI.getJobTitles().success(function (data) {
        $scope.jobs = data;
    });

    //-- preparando chart  --------------------
    $scope.chartOptions = {
        title: {
            display: true,
            text: 'Cargos por período'
        },
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    };

    function refreshChart(data) {
        var labels = [];
        var values = [];
        for (i = 0; i < data.ChartData.length; i++) {
            labels.push(data.ChartData[i].Label);
            values.push(data.ChartData[i].Value);
        }
        $scope.labels = labels;
        $scope.data = values;
    }
    //-- fim chart ---------------------------

    //preparando datepickers------------------
    $scope.today = function () {
        if ($scope.newEmployee == undefined)
            $scope.newEmployee = {};
        $scope.newEmployee.Birth = new Date();
    };
    $scope.today();

    $scope.clear = function () {
        $scope.newEmployee.Birth = null;
    };

    $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

    $scope.toggleMin = function () {
        $scope.inlineOptions.minDate = $scope.inlineOptions.minDate ? null : new Date();
        $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
    };

    $scope.toggleMin();

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.open2 = function () {
        $scope.popup2.opened = true;
    };

    $scope.setDate = function (year, month, day) {
        $scope.newEmployee.Birth = new Date(year, month, day);
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
    $scope.altInputFormats = ['M!/d!/yyyy'];

    $scope.popup1 = {
        opened: false
    };

    $scope.popup2 = {
        opened: false
    };

    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var afterTomorrow = new Date();
    afterTomorrow.setDate(tomorrow.getDate() + 1);
    $scope.events = [
    {
        date: tomorrow,
        status: 'full'
    },
    {
        date: afterTomorrow,
        status: 'partially'
    }
  ];

    function getDayClass(data) {
        var date = data.date,
      mode = data.mode;
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    }
    //fim datepickers-------------------------------

});