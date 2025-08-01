namespace ResourceTracker.Models
{
    public class EmployeeModel
    {
        public int? EmpId { get; set; }
        public string Employee_Name { get; set; }
        public int? DesignationId { get; set; }
        public int? LocationId { get; set; }
        public string EmailId { get; set; }
        public DateOnly CTE_DOJ { get; set; }
        public string Remarks { get; set; }
        public int? ManagerId { get; set; }
        public string Billable { get; set; }

        public List<int> SkillIds { get; set; }
        public List<int> ProjectIds { get; set; }
    }
}
