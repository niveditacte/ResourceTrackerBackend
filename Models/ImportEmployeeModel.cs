namespace Models
{
    public class ImportEmployeeModel
    {   
        // Excel input fields
        public string? Employee_Name { get; set; }
        public string? EmailId { get; set; }
        public DateOnly? CTE_DOJ { get; set; }
        public string? Remarks { get; set; }
        public string? Billable { get; set; }

        public string? Designation_Name { get; set; }
        public string? Manager_Name { get; set; }
        public string? Location_Name { get; set; }

        public string? Skills { get; set; }
        public string? Projects { get; set; }

        // Resolved ID fields
        public int? DesignationId { get; set; }
        public int? LocationId { get; set; }
        public int? ManagerId { get; set; }

        public List<int>? SkillIds { get; set; }
        public List<int>? ProjectIds { get; set; }

        // Optional - status tracking during import
        public string? ErrorMessage { get; set; }
    }
}
