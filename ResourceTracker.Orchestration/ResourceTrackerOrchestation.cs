using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using ResourceTracker.DAO.Interfaces;
using ResourceTracker.DAO.Models;
using ResourceTracker.Orchestration.Interfaces;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;


namespace ResourceTracker.Orchestration
{
    
    public class ResourceTrackerOrchestation(IResourceTrackerDao dao,ILogger<ResourceTrackerOrchestation> logger ) : IResourceTrackerOrchestration
    {
        private readonly IResourceTrackerDao _dao = dao;
        private readonly ILogger<ResourceTrackerOrchestation> _logger = logger;

        public async Task<OperationResult> AddEmployeeAsync(Resource resource)
        {
            try
            {
                if (resource == null) 
                {
                    _logger.LogWarning("AddEmployeeAsync was call with a null resource.");
                    return OperationResult.Fail("Invalid employee data.");
                }
                _logger.LogInformation("Adding emloyee with id: {EmpId}", resource?.EmpId);
                return await _dao.AddEmployeeAsync(resource);
                
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while adding employee with ID: {EmpId}", resource?.EmpId);
                return OperationResult.Fail("Something went wrong!");
            }
            
        }

        public async Task<OperationResult> AddEmployeeFromExcelAsync(ResourcesExcel resourceExcel)
        {
            try
            {
                if (resourceExcel == null)
                {
                    _logger.LogWarning("AddEmployeeAsync was call with a null resource.");
                    return OperationResult.Fail("Invalid employee data.");
                }
                _logger.LogInformation("Adding emloyee with id: {EmpId}", resourceExcel?.EmpId);
                return await _dao.AddEmployeeFromExcelAsync(resourceExcel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding employee with ID: {EmpId}", resourceExcel?.EmpId);
                return OperationResult.Fail("Something went wrong!");
            }

        }

        public async Task<OperationResult> UpdateEmployeeAsync(Resource resource)
        {
            try
            {
                if (resource == null)
                {
                    _logger.LogWarning("UpdateEmployeeAsync was called with null resource.");
                    return OperationResult.Fail("Invalid Resource");
                }
                return await _dao.UpdateEmployeeAsync(resource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while updating employee with id:{EmpId}", resource?.EmpId);
                return OperationResult.Fail("Error occured while updating employee");
            }

            
        }

        public async Task<OperationResult> BulkUpdateEmployeesAsync(List<Resource> resources)
        {
            try
            {
                if (resources == null || !resources.Any())
                {
                    _logger.LogWarning("BulkUpdateEmployeesAsync was called with an empty or null resource list.");
                    return OperationResult.Fail("Invalid Resource List");
                }

                return await _dao.BulkUpdateEmployeesAsync(resources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while bulk updating employees.");
                return OperationResult.Fail("Error occurred while bulk updating employees");
            }
        }

        public async Task<OperationResult> DeleteEmployeeAsync(string empId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empId)) 
                {
                    _logger.LogWarning("DeleteEmployeeAsync failed: empId id null.");
                    return OperationResult.Fail("EmpId is required to delete.");
                }

                _logger.LogInformation("Deleting employee with Id:{empId}", empId);
                return await _dao.DeleteEmployeeAsync(empId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error occured while deleting employee with Id:{empId}", empId);
                return OperationResult.Fail("Error occured while deleting");
            }
           
        }

        public async Task<Resource> GetEmployeeByIdAsync(string empId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empId))
                {
                    
                    _logger.LogWarning("GetEmployeeByIdAsync failed: empId id null.");
                    return null;
                    
                }

                _logger.LogInformation("Fetching the information of employee with Id:{empId}",empId);
                return await _dao.GetEmployeeByIdAsync(empId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching infomation of employee with Id:{empId}", empId);
                return null;
            }
        }

        public async Task<List<Resource>> GetAllEmployeesAsync()
        {
            try 
            {
                _logger.LogInformation("Fetching Information of all the employees.");
                return await _dao.GetAllEmployeesAsync();
            }
            catch(Exception ex) 
            {
                _logger.LogError("Error occured while fetching information of all the employees.",ex);
                return null;
            }
            
        }
        public async Task<List<Resource>> GetAllEmployeesAsyncDownload()
        {
            try
            {
                _logger.LogInformation("Fetching Information of all the employees.");
                return await _dao.GetAllEmployeesAsyncDownload();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while fetching information of all the employees.", ex);
                return null;
            }

        }

        public async Task<OperationResult> ImportFromExcelAsync(List<Resource> resources)
        {
            try
            {
                foreach (var res in resources)
                {
                    await _dao.AddEmployeeAsync(res); // or use a bulk insert method
                }

                return OperationResult.Ok($"Successfully imported {resources.Count} resources.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import parsed resources.");
                return OperationResult.Fail("Import failed.");
            }
        }
    }
}
