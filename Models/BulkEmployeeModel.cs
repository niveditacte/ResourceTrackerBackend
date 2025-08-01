

namespace ResourceTracker.Models
{
    public class BulkEmployeeModel
    {
        public List<int> EmpIds { get; set; } = new();
        public int? DesignationId { get; set; }
        public int? ManagerId { get; set; }
        public string? Billable { get; set; }
        public int? ProjectId { get; set; }  
        public string? SkillIds { get; set; }
    }
}
