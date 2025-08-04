using Models;
using ResourceTracker.DAO.Interfaces;
using ResourceTracker.Models;
using ResourceTracker.Orchestration.Interfaces;
using System;


namespace ResourceTracker.Orchestration
{
    public class EmployeeOrchestration: IEmployeeOrchestration

    {
        private readonly IEmployeeDao _employeeDao;
        public EmployeeOrchestration(IEmployeeDao employeeDao)
        {
            _employeeDao = employeeDao;
        }

        public void InsertEmployee(EmployeeModel model)
        {
            _employeeDao.InsertEmployee(model);
        }

        public void UpdateEmployee(EmployeeModel model)
        {
            _employeeDao.UpdateEmployee(model);
        }

        public void DeleteEmployee(int empId)
        {
            _employeeDao.DeleteEmployee(empId);
        }

        public EmployeeModel GetEmployeeById(int empId)
        {
            return _employeeDao.GetEmployeeById(empId);
        }

        public EmployeeDetailsResponse GetEmployeeDetails(int empId)
        {
            return _employeeDao.GetEmployeeDetails(empId);
        }

        public List<EmployeeDetailsResponse> GetAllEmployees()
        {
            return _employeeDao.GetAllEmployees();
        }

        public void BulkUpdateEmployees(BulkEmployeeModel model)
        {
            _employeeDao.BulkUpdateEmployee(model);
        }

        public List<ImportResult> BulkImportEmployees(List<ImportEmployeeModel> employees)
        {
            var failedImports = new List<ImportResult>();
            Console.WriteLine("Inside BulkImportEmployees");

            foreach (var importEmp in employees)
            {
                var errors = new List<string>();

                // Validation
                if (string.IsNullOrWhiteSpace(importEmp.Employee_Name))
                {
                    errors.Add("Employee name is required.");
                    continue;
                }
                

                //Designation
                importEmp.DesignationId = _employeeDao.GetDesignationId(importEmp.Designation_Name);
                Console.WriteLine($"DesignationID: {importEmp.DesignationId}");

                if (!importEmp.DesignationId.HasValue)
                {
                    errors.Add("Invalid or missing designation.");
                    continue;
                }

                // Location
                Console.WriteLine($"Trying to fetch LocationId for name: '{importEmp.Location_Name}'");
                importEmp.LocationId = _employeeDao.GetLocationId(importEmp.Location_Name);
                Console.WriteLine($"LocationId: {importEmp.LocationId}");

                if (!importEmp.LocationId.HasValue)
                {
                    errors.Add("Invalid or missing location.");
                    continue;
                }

                // Manager
                Console.WriteLine($"Trying to fetch ManagerId for name: '{importEmp.Manager_Name}'");
                importEmp.ManagerId = _employeeDao.GetManagerId(importEmp.Manager_Name);
                Console.WriteLine($"ManagerId: {importEmp.ManagerId}");

                if (!importEmp.ManagerId.HasValue)
                {
                    errors.Add("Invalid or missing manager.");
                    continue;
                }


                // Get SkillIds
                Console.WriteLine($"Raw Skills string: '{importEmp.Skills}'");

                // Get SkillIds
                if (string.IsNullOrWhiteSpace(importEmp.Skills))
                {
                    importEmp.SkillIds = new List<int>();
                    Console.WriteLine("No skills provided.");
                }
                else
                {
                    var skillList = importEmp.Skills.Split(',').Select(s => s.Trim()).ToList();
                    Console.WriteLine($"Parsed skill names: {string.Join(", ", skillList)}");

                    importEmp.SkillIds = _employeeDao.GetSkillIds(skillList);

                    if (importEmp.SkillIds != null && importEmp.SkillIds.Any())
                        Console.WriteLine($"Mapped Skill IDs: {string.Join(", ", importEmp.SkillIds)}");
                    else
                        Console.WriteLine("No valid Skill IDs found from the database.");
                }

                // Validation
                if (importEmp.SkillIds == null || !importEmp.SkillIds.Any())
                {
                    errors.Add("Missing or invalid skills.");
                    continue;
                }


                //// Get ProjectIds
                Console.WriteLine($"Raw Projects string: '{importEmp.Projects}'");
                if (string.IsNullOrWhiteSpace(importEmp.Projects))
                {
                    importEmp.ProjectIds = new List<int>();
                    Console.WriteLine("No projects provided.");
                }
                else
                {
                    var projectList = importEmp.Projects
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToList();

                    Console.WriteLine($"Parsed project names: {string.Join(", ", projectList)}");

                    importEmp.ProjectIds = _employeeDao.GetProjectIds(projectList);

                    if (importEmp.ProjectIds != null && importEmp.ProjectIds.Any())
                        Console.WriteLine($"Mapped Project IDs: {string.Join(", ", importEmp.ProjectIds)}");
                    else
                        Console.WriteLine("No valid Project IDs found from the database.");
                }

                if (importEmp.ProjectIds == null || !importEmp.ProjectIds.Any())
                {
                    errors.Add("Missing or invalid projects.");
                    continue;
                }

                // ✅ Billable validation must be BEFORE error check
                string billableVal = importEmp.Billable?.Trim();

                if (string.IsNullOrWhiteSpace(billableVal))
                {
                    billableVal = "No"; // Default
                }
                else if (billableVal.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    billableVal = "Yes";
                }
                else if (billableVal.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    billableVal = "No";
                }
                else
                {
                    errors.Add("Invalid Billable value. Only 'Yes' or 'No' are allowed.");
                    continue;
                }

                // If errors exist, track them and continue to next record
                if (errors.Any())
                {
                    failedImports.Add(new ImportResult
                    {
                        EmployeeName = importEmp.Employee_Name ?? "(Unnamed)",
                        Errors = errors
                    });
                    continue;
                }


                // Build full model and insert
                var empModel = new EmployeeModel
                {
                    Employee_Name = importEmp.Employee_Name!,
                    DesignationId = importEmp.DesignationId,
                    LocationId = importEmp.LocationId,
                    EmailId = importEmp.EmailId ?? "",
                    CTE_DOJ = importEmp.CTE_DOJ ?? DateOnly.MinValue,
                    Remarks = importEmp.Remarks ?? "",
                    ManagerId = importEmp.ManagerId,
                    Billable =  billableVal,
                    SkillIds = importEmp.SkillIds!,
                    ProjectIds = importEmp.ProjectIds!
                };

                Console.WriteLine($"Inserting employee: {empModel.Employee_Name}");
                Console.WriteLine($" -> SkillIds: {string.Join(", ", empModel.SkillIds)}");
                Console.WriteLine($" -> ProjectIds: {string.Join(", ", empModel.ProjectIds)}");
                Console.WriteLine($" -> ManagerId: {empModel.ManagerId}");
                try
                {
                    _employeeDao.InsertEmployee(empModel);
                }
                catch (Exception ex)
                {
                    failedImports.Add(new ImportResult
                    {
                        EmployeeName = importEmp.Employee_Name!,
                        Errors = new List<string> { $"Error during insert: {ex.Message}" }
                    });
                }
            }

            return failedImports;
        }

        public List<ExportEmployeeModel> GetExportEmployees()
        {
            return _employeeDao.GetEmployeesForExport();
        }

    }
}

