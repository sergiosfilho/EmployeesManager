using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueOpexEmployeesManager.Models
{
    public class FilterDataResult
    {
        public List<EmployeeData> TableData { get; set; }
        public List<ChartDataPoint> ChartData { get; set; }
    }
}