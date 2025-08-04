using Microsoft.AspNetCore.Mvc;
using Models;
using ResourceTracker.Models;
using ResourceTracker.Orchestration.Interfaces;
using System.Text;

namespace ResourceTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeOrchestration _employeeOrchestration;

        public EmployeeController(IEmployeeOrchestration employeeOrchestration)
        {
            _employeeOrchestration = employeeOrchestration;
        }

        [HttpPost]
        public IActionResult InsertEmployee([FromBody] EmployeeModel model)
        {
            _employeeOrchestration.InsertEmployee(model);
            return Ok(new { message = "Employee inserted successfully." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeModel model)
        {
            if (id != model.EmpId)
                return BadRequest("Employee ID mismatch.");

            _employeeOrchestration.UpdateEmployee(model);
            return Ok(new { message = "Employee updated successfully." });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeOrchestration.DeleteEmployee(id);
            return Ok(new { message = "Employee deleted successfully." });
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _employeeOrchestration.GetEmployeeById(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpGet("details/{id}")]
        public IActionResult GetEmployeeDetails(int id)
        {
            var details = _employeeOrchestration.GetEmployeeDetails(id);
            if (details == null)
                return NotFound();

            return Ok(details);
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeOrchestration.GetAllEmployees();
            return Ok(employees);
        }

        [HttpPut("bulk-update")]
        public IActionResult BulkUpdateEmployees([FromBody] BulkEmployeeModel model)
        {
            if (model.EmpIds == null || model.EmpIds.Count == 0)
                return BadRequest("At least one employee ID must be provided.");

            _employeeOrchestration.BulkUpdateEmployees(model);
            return Ok(new { message = "Employees updated successfully." });
        }

        [HttpPost("import")]
        public IActionResult BulkImportEmployees([FromBody] List<ImportEmployeeModel> employees)
        {
            if (employees == null || !employees.Any())
            {
                return BadRequest("Employee list cannot be empty.");
            }

            try
            {
               var results = _employeeOrchestration.BulkImportEmployees(employees);
                return Ok(new
                {
                                message = results.Any()
                       ? "Employees imported with some failures."
                       : "Employees imported successfully.",
                                FailedRecords = results
                });
            }
            catch (Exception ex)
            {
                // Log error here if needed
                return StatusCode(500, $"Error importing employees: {ex.Message}");
            }
        }

        [HttpGet("export")]
        public IActionResult ExportEmployees()
        {
            var exportData = _employeeOrchestration.GetExportEmployees();

            var csv = new StringBuilder();
            csv.AppendLine("EmpID, Employee Name,Designation,Reporting To,Billable,Tech Skill,Project Allocation,Location,Email ID,CTE DOJ,Remarks,Exported At");

            foreach (var e in exportData)
            {
                csv.AppendLine(string.Join(",",
                    $"E{e.EmpId.ToString().PadLeft(4, '0')}",
                    WrapCsv(e.Employee_Name),
                    WrapCsv(e.Designation_Name),
                    WrapCsv(e.ReportingToName),
                    WrapCsv(e.Billable),
                    WrapCsv(e.Skills),
                    WrapCsv(e.Projects),
                    WrapCsv(e.Location_Name),
                    WrapCsv(e.EmailId),
                    e.CTE_DOJ.ToString("yyyy-MM-dd"),
                    WrapCsv(e.Remarks),
                    e.ExportedAt.ToString("yyyy-MM-dd")
                ));
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "ResourceDetails.csv");
        }

        private string WrapCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

    }


}

