using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceTracker.DAO.Models;


namespace ResourceTracker.DAO.Interfaces
{
    public interface IResourceTrackerDao
    {
        Task<OperationResult> AddEmployeeAsync(Resource resource);
        Task<OperationResult> AddEmployeeFromExcelAsync(ResourcesExcel resourceExcel);

        Task<Resource?> GetEmployeeByIdAsync(string empId);

        Task<List<Resource>> GetAllEmployeesAsync();

        Task<List<Resource>> GetAllEmployeesAsyncDownload();

        Task<OperationResult> UpdateEmployeeAsync(Resource resource);

        Task<OperationResult> BulkUpdateEmployeesAsync(List<Resource> resources);

        Task<OperationResult> DeleteEmployeeAsync(string empId);
    }
}
