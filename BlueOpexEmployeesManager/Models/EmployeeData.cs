using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueOpexEmployeesManager.Models
{
    public class EmployeeData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DocumentIdentity { get; set; }
        public string DocumentCPF { get; set; }
        public string DocumentPassport { get; set; }
        public int JobTitleId { get; set; }
        public string JobTitle { get; set; }
        public string Status { get; set; }
        public string Birth { get; set; }
        
        public static EmployeeData FromEntity(Employee employee)
        {
            EmployeeData retVal = new EmployeeData();
            retVal.Id = employee.id;
            retVal.DocumentCPF = employee.document_CPF;
            retVal.DocumentIdentity = employee.document_identity;
            retVal.DocumentPassport = employee.document_passport;
            retVal.Email = employee.email;
            retVal.Name = employee.name;

            Employee_JobTitle lastJobTitle = employee.Employee_JobTitle.OrderByDescending(j => j.dateBegin).FirstOrDefault();
            retVal.JobTitle = lastJobTitle.JobTitle.name;
            retVal.JobTitleId = lastJobTitle.JobTitle.id;
            retVal.Status = lastJobTitle.dateEnd == null ? "Funcionário" : "Ex funcionário";

            return retVal;
        }

        public static List<EmployeeData> FromEntityList(List<Employee> employees)
        {
            List<EmployeeData> retVal = new List<EmployeeData>();
            foreach (Employee employee in employees)            
                retVal.Add(FromEntity(employee));            
            return retVal;
        }
    }
}