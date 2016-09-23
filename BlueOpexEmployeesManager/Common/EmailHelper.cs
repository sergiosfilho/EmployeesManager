using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using System.Net;


namespace BlueOpexEmployeesManager.Common
{
    public class EmailHelper
    {
        public static void SendEmailNewEmployee(Employee employee)
        {
            string mailAddress = ConfigurationManager.AppSettings["employeesManagerMailAddress"];
            string mailPassword = ConfigurationManager.AppSettings["employeesManagerMailPassword"];
            string mailHost = ConfigurationManager.AppSettings["employeesManagerMailHost"];
            string mailPort = ConfigurationManager.AppSettings["employeesManagerMailPort"];
            string mailEnableSSL = ConfigurationManager.AppSettings["employeesManagerMailEnableSSL"];
            string[] adminsEmails = ConfigurationManager.AppSettings["adminsEmails"].Split(';');
                                    
            MailAddress from = new MailAddress(mailAddress, "Employees Manager");            
            var smtp = new SmtpClient
            {
                Host = mailHost,
                Port = Int32.Parse(mailPort),
                EnableSsl = Boolean.Parse(mailEnableSSL),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                
                Credentials = new NetworkCredential(from.Address, mailPassword)
            };
            MailMessage message = NewEmployeeEmail(from, employee, adminsEmails);
            smtp.Send(message);            
        }

        private static MailMessage NewEmployeeEmail(MailAddress from, Employee employee, string[] admins)
        {
            MailMessage mailMessage = new MailMessage() { From = from };
            foreach (string admin in admins)
                mailMessage.To.Add(new MailAddress(admin, admin));
            mailMessage.Subject = "Novo funcionário - " + employee.name;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body =
            "<html>" +
                "<body>" +
                    "<div style='background:#EEEEEE; margin:25px; padding:20px; border:1px solid Green;'>" +
                        "Novo funcionário incorporado ao quadro da empresa: " + employee.name +
                        "<br /><br />" +
                        "Cargo: " + (employee.Employee_JobTitle.OrderByDescending(j=>j.dateBegin).FirstOrDefault().JobTitle.name) +
                        "<br /><br />" +
                        DateTime.Now.ToString() +
                    "</div>" +
                "</body>" +
            "</html>";
            return mailMessage;
        }
    }
}