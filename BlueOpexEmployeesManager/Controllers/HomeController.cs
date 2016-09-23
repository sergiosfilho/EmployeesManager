using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueOpexEmployeesManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeesList() { return View(); }
        public ActionResult CreateEmployee() { return View(); }
        public ActionResult ChartEmployeesPerJob() { return View(); }
        public ActionResult ChartEmployeesPerAge() { return View(); }
        public ActionResult ChartEmployeesPerPerfil() { return View(); }
        public ActionResult FilterEmployees() { return View(); }
    }
}
