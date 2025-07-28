using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.DAO.Models
{
    public class Resource
    {
        public string? EmpId { get; set; }
        public string? Resource_Name { get; set; }
        public string? Designation { get; set; }
        public string? ReportingTo { get; set; }
        public string? Billable { get; set; }
        public string? Technology_Skill { get; set; }
        public string? Project_Allocate { get; set; }
        public string? Location { get; set; }
        public string? EmailId { get; set; }
        public DateOnly? CTE_DOJ { get; set; }
        public string? Remarks { get; set; }
        public DateTime? ExportedAt { get; set; }
    }
}
