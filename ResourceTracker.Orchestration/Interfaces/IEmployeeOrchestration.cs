using Models;
using ResourceTracker.Models;


namespace ResourceTracker.Orchestration.Interfaces
{
    public interface IEmployeeOrchestration
    {
        void InsertEmployee(EmployeeModel model);
        void UpdateEmployee(EmployeeModel model);
        void DeleteEmployee(int empId);

        EmployeeModel GetEmployeeById(int empId);
        EmployeeDetailsResponse GetEmployeeDetails(int empId);
        List<EmployeeDetailsResponse> GetAllEmployees();
        void BulkUpdateEmployees(BulkEmployeeModel model);
        List<ImportResult> BulkImportEmployees(List<ImportEmployeeModel> employees);
        List<ExportEmployeeModel> GetExportEmployees();

    }
}
