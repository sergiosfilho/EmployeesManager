using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueOpexEmployeesManager.Models;
using BlueOpexEmployeesManager.Common;
using System.Web.Security;


namespace BlueOpexEmployeesManager.Controllers
{
    public class EmployeesAPIController : ApiController
    {
        [HttpPost]
        public LoginResult PostLogin([FromBody]LoginData data)
        {
            LoginResult retVal = new LoginResult() { Success = false };
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                User user = db.User.FirstOrDefault(u => u.login == data.UserLogin);
                if (user != null)
                {
                    string hashFromTypedPassword = MD5Helper.Hash(data.UserPassword + user.passwordSalt);
                    if (user.passwordHash.ToLower() == hashFromTypedPassword.ToLower())
                    {
                        //TODO: implementar login
                        retVal.Success = true;
                        retVal.UserName = user.login;
                        retVal.UserId = user.id;




                    }
                }
            }
            return retVal;
        }
                
        [HttpGet]
        public IEnumerable<EmployeeData> GetEmployees()
        {
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                return EmployeeData.FromEntityList(db.Employee.Include("Employee_JobTitle").ToList());
            }
        }

        [HttpPost]
        public NewEmployeeResult PostNewEmployee([FromBody]NewEmployeeData data)
        {
            NewEmployeeResult retVal = new NewEmployeeResult() { Success = true };
            //TODO: validar dados
            //TODO: recuperar id do usuário logado
            int clientId = 1;
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                Employee newEmployee = data.ToEntity(clientId);
                db.Employee.AddObject(newEmployee);
                db.SaveChanges();                
                Employee_JobTitle employeeJob = new Employee_JobTitle();
                employeeJob.dateBegin = DateTime.Now;
                employeeJob.employeeId = newEmployee.id;
                employeeJob.jobTitleId = data.JobTitleId;
                db.Employee_JobTitle.AddObject(employeeJob);
                db.SaveChanges();
                EmailHelper.SendEmailNewEmployee(newEmployee);
            }
            return retVal;
        }

        [HttpPost]
        public NewEmployeeResult PostEmployeesList([FromBody]List<NewEmployeeData> items)
        {
            NewEmployeeResult retVal = new NewEmployeeResult() { Success = true };
            //TODO: validar dados
            //TODO: recuperar id do usuário logado
            int clientId = 1;
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                foreach(NewEmployeeData item in items)
                {
                    Employee newEmployee = item.ToEntity(clientId);
                    db.Employee.AddObject(newEmployee);
                    db.SaveChanges();
                    Employee_JobTitle employeeJob = new Employee_JobTitle();
                    employeeJob.dateBegin = DateTime.Now;
                    employeeJob.employeeId = newEmployee.id;
                    employeeJob.jobTitleId = item.JobTitleId;
                    db.Employee_JobTitle.AddObject(employeeJob);
                    db.SaveChanges();                    
                }
            }
            return retVal;
        }

        [HttpGet]
        public IEnumerable<ChartDataPoint> GetEmployeesPerBirth()
        {
            List<ChartDataPoint> retVal = new List<ChartDataPoint>();
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                Dictionary<string, int> months = new Dictionary<string, int>();
                foreach (Employee employee in db.Employee.ToList())
                {
                    string label = employee.birth.ToString("yyyy-MM");
                    if (months.Keys.Contains(label))
                        months[label] = months[label] + 1;
                    else
                        months.Add(label, 1);
                }   
                foreach(KeyValuePair<string,int> pair in months)                
                    retVal.Add(new ChartDataPoint() { Label = pair.Key, Value = pair.Value });                
            }
            return retVal.OrderBy(p=> p.Label);
        }

        [HttpGet]
        public IEnumerable<ChartDataPoint> GetEmployeesPerJob()
        {
            List<ChartDataPoint> retVal = new List<ChartDataPoint>();
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                Dictionary<string, int> jobs = new Dictionary<string, int>();
                foreach (Employee employee in db.Employee.Include("Employee_JobTitle").ToList())
                {
                    string label = employee.Employee_JobTitle.OrderByDescending(j=>j.dateBegin).FirstOrDefault().JobTitle.name;
                    if (jobs.Keys.Contains(label))
                        jobs[label] = jobs[label] + 1;
                    else
                        jobs.Add(label, 1);
                }
                foreach (KeyValuePair<string, int> pair in jobs)
                    retVal.Add(new ChartDataPoint() { Label = pair.Key, Value = pair.Value });
            }
            return retVal.OrderBy(p => p.Label);
        }

        [HttpGet]
        public IEnumerable<JobTitleData> GetJobTitles()
        {             
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                return JobTitleData.FromEntityList(db.JobTitle.OrderBy(j => j.name).ToList());
            }            
        }

        [AcceptVerbs("Get", "Post")]
        public FilterDataResult GetEmployeesByFilter(FilterData filterData)
        {   
            using (EmployeesManagerEntities db = new EmployeesManagerEntities())
            {
                //lista de todos os funcionários seguindo os filtros. Essa lista popula a tabela e a partir dela será gerada outra coleção para o gráfico
                IEnumerable<Employee> employees = db.Employee.OrderBy(e => e.name);
                if (filterData != null) 
                {   
                    employees = employees
                        .Where(e => e.Employee_JobTitle
                            .Any(j => j.jobTitleId == filterData.JobTitleId
                                && (
                                //data de início no cargo dentro do range do filtro
                                (j.dateBegin >= filterData.DateStart && j.dateBegin <= filterData.DateEnd)
                                ||
                                //data de fim do cargo dentro do range do filtro ou cargo não foi abandonado e data de início do cargo é menor que o fim do range do filtro
                                (j.dateBegin <= filterData.DateEnd && (j.dateEnd == null || j.dateEnd >= filterData.DateStart)))));
                }
                List<Employee> filteredEmployees = employees.ToList();
                FilterDataResult retVal = new FilterDataResult();
                retVal.TableData = EmployeeData.FromEntityList(filteredEmployees);
                
                //separa resultado em períodos menores para exibir no gráfico
                retVal.ChartData = new List<ChartDataPoint>();
                if (filterData != null)
                {
                    double filterRangeDaysAmount = (filterData.DateEnd - filterData.DateStart).TotalDays;
                    double labelPeriodRange = filterRangeDaysAmount / 6;                                  
                    for (int i = 0; i < 6; i++)
                    {
                        DateTime periodStart = filterData.DateStart.AddDays(labelPeriodRange * i);
                        DateTime periodEnd = periodStart.AddDays(labelPeriodRange);
                        int periodCount = filteredEmployees
                        .Count(e => e.Employee_JobTitle.Any(j => j.jobTitleId == filterData.JobTitleId                            
                            && ((j.dateBegin >= periodStart && j.dateBegin <= periodEnd) ||
                                (j.dateBegin <= periodEnd && (j.dateEnd == null || j.dateEnd >= periodStart)))));
                        string periodLabel = periodStart.ToString("dd/MM/yyyy") + " - " + periodEnd.ToString("dd/MM/yyyy");
                        retVal.ChartData.Add(new ChartDataPoint() { Label = periodLabel, Value = periodCount });
                    }
                }
                else
                {
                    //se não houver filtro, criar uma série para cada cargo
                }
                
                return retVal;
            }
        }                
    }
}