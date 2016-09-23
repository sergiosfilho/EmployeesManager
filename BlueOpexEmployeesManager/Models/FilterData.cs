using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueOpexEmployeesManager.Models
{
    public class FilterData
    {
        public int JobTitleId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}