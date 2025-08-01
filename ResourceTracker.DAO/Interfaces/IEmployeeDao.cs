using Models;
using ResourceTracker.Models;

namespace ResourceTracker.DAO.Interfaces
{
    public interface IEmployeeDao
    {
        void InsertEmployee(EmployeeModel model);
        void UpdateEmployee(EmployeeModel model);
        void DeleteEmployee(int empId);

        EmployeeDetailsResponse GetEmployeeDetails(int empId);  // Full details (joins)
        EmployeeModel GetEmployeeById(int empId);            // Basic info only
        List<EmployeeDetailsResponse> GetAllEmployees();
        // IEmployeeDao.cs
        void BulkUpdateEmployee(BulkEmployeeModel model);

        // ✅ Helper methods to resolve names to IDs using SPs
        int? GetDesignationId(string designationName);
        int? GetLocationId(string locationName);
        int? GetManagerId(string managerName);
        List<int> GetSkillIds(List<string> skillNames);
        List<int> GetProjectIds(List<string> projectNames);
    }
}
