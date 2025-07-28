using ResourceTracker.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace ResourceTracker.Orchestration.Interfaces
{
    public interface IResourceTrackerOrchestration
    {
        Task<OperationResult> AddEmployeeAsync(Resource resource);
        Task<OperationResult> AddEmployeeFromExcelAsync(ResourcesExcel resourceExcel);
        Task<OperationResult> UpdateEmployeeAsync(Resource resource);
        Task<OperationResult> BulkUpdateEmployeesAsync(List<Resource> resources);
        Task<OperationResult> DeleteEmployeeAsync(string empId);
        Task<Resource> GetEmployeeByIdAsync(string empId);
        Task<List<Resource>> GetAllEmployeesAsync();
        Task<List<Resource>> GetAllEmployeesAsyncDownload();
        Task<OperationResult> ImportFromExcelAsync(List<Resource> resources);


    }
}
