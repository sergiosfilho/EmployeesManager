using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueOpexEmployeesManager.Models
{
    public class JobTitleData
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static JobTitleData FromEntity(JobTitle jobTitle)
        {
            JobTitleData retVal = new JobTitleData();
            retVal.Id = jobTitle.id;
            retVal.Name = jobTitle.name;
            return retVal;
        }

        public static List<JobTitleData> FromEntityList(List<JobTitle> jobTitleList)
        {
            List<JobTitleData> retVal = new List<JobTitleData>();
            foreach (JobTitle jobTitle in jobTitleList)            
                retVal.Add(JobTitleData.FromEntity(jobTitle));            
            return retVal;
        }
    }
}