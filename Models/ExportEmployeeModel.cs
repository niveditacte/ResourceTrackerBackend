using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Models
{
    public class ExportEmployeeModel
    {
        public int EmpId { get; set; }
        public string Employee_Name { get; set; }
        public string Designation_Name { get; set; }
        public string ReportingToName { get; set; }
        public string Billable { get; set; }
        public string Skills { get; set; }
        public string Projects { get; set; }
        public string Location_Name { get; set; }
        public string EmailId { get; set; }
        public DateOnly CTE_DOJ { get; set; }
        public string Remarks { get; set; }
        public DateOnly ExportedAt { get; set; }
    }
}
