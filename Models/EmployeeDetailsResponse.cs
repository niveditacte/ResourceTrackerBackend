namespace ResourceTracker.Models
{
    public class EmployeeDetailsResponse
    {
        public int EmpId { get; set; }
        public string Employee_Name { get; set; }
        public string EmailId { get; set; }
        public DateTime? CTE_DOJ { get; set; }
        public string Remarks { get; set; }
        public string Designation_Name { get; set; }
        public string Location_Name { get; set; }
        public string ReportingToName { get; set; }
        public string Billable { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Projects { get; set; }
    }
}
