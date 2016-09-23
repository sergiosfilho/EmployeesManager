using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueOpexEmployeesManager.Models
{
    public class NewEmployeeData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string DocumentIdentity { get; set; }
        public string DocumentCPF { get; set; }
        public string DocumentPassport { get; set; }
        public int JobTitleId { get; set; }
        public string Comments { get; set; }
        public DateTime Birth { get; set; }

        public Employee ToEntity(int clientId)
        {
            Employee retVal = new Employee();
            retVal.name = Name;
            retVal.clientId = clientId;
            retVal.comments = Comments;
            retVal.document_CPF = DocumentCPF;
            retVal.document_identity = DocumentIdentity;
            retVal.document_passport = DocumentPassport;
            retVal.email = Email;
            retVal.birth = Birth;
            return retVal;
        }
    }
}