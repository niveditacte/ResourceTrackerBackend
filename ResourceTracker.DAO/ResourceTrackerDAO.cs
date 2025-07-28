using ResourceTracker.DAO.Models;
using System.Data;
using ResourceTracker.DAO.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ResourceTracker.DAO
{
    public class ResourceTrackerDAO: IResourceTrackerDao
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public ResourceTrackerDAO(IConfiguration configuration, ILogger<ResourceTrackerDAO> logger)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnnection")!;
            this._logger = logger;
        }
        public async Task<OperationResult> AddEmployeeAsync(Resource resource)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddEmployee", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            try { 
                if(resource == null)
                {
                    _logger.LogWarning("AddEmployeeAsync called with a null resource.");
                    return OperationResult.Fail("Invalid resource data.");
                }
           
                cmd.Parameters.AddWithValue("@Resource_Name", resource.Resource_Name);
                cmd.Parameters.AddWithValue("@Designation", resource.Designation);
                cmd.Parameters.AddWithValue("@ReportingTo", resource.ReportingTo);
                cmd.Parameters.AddWithValue("@Billable", resource.Billable);
                cmd.Parameters.AddWithValue("@Technology_Skill", resource.Technology_Skill);
                cmd.Parameters.AddWithValue("@Project_Allocate", resource.Project_Allocate);
                cmd.Parameters.AddWithValue("@Location", resource.Location);
                cmd.Parameters.AddWithValue("@EmailId", resource.EmailId);
                cmd.Parameters.AddWithValue("@CTE_DOJ", resource.CTE_DOJ);
                cmd.Parameters.AddWithValue("@Remarks", resource.Remarks);

                SqlParameter outputIdParam = new SqlParameter("@NewEmpId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                _logger.LogInformation("Executing the stored procedure sp_AddEmployee for employee {Name}",resource.Resource_Name);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                int newEmpId = (int)outputIdParam.Value;

                _logger.LogInformation("Successfully added the employee {Name}",resource.Resource_Name);
                return OperationResult.Ok("resource added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while adding employee {Name}",resource.Resource_Name);
                return OperationResult.Fail($"Add failed: {ex.Message}");
            }
        }

        public async Task<OperationResult> AddEmployeeFromExcelAsync(ResourcesExcel resourceExcel)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_AddEmployee", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                if (resourceExcel == null)
                {
                    _logger.LogWarning("AddEmployeeAsync called with a null resource.");
                    return OperationResult.Fail("Invalid resource data.");
                }

                cmd.Parameters.AddWithValue("@Resource_Name", resourceExcel.Resource_Name);
                cmd.Parameters.AddWithValue("@Designation", resourceExcel.Designation);
                cmd.Parameters.AddWithValue("@ReportingTo", resourceExcel.ReportingTo);
                cmd.Parameters.AddWithValue("@Billable", resourceExcel.Billable);
                cmd.Parameters.AddWithValue("@Technology_Skill", resourceExcel.Technology_Skill);
                cmd.Parameters.AddWithValue("@Project_Allocate", resourceExcel.Project_Allocate);
                cmd.Parameters.AddWithValue("@Location", resourceExcel.Location);
                cmd.Parameters.AddWithValue("@EmailId", resourceExcel.EmailId);
                cmd.Parameters.AddWithValue("@CTE_DOJ", resourceExcel.CTE_DOJ);
                cmd.Parameters.AddWithValue("@Remarks", resourceExcel.Remarks);

                SqlParameter outputIdParam = new SqlParameter("@NewEmpId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                _logger.LogInformation("Executing the stored procedure sp_AddEmployee for employee {Name}", resourceExcel.Resource_Name);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                int newEmpId = (int)outputIdParam.Value;

                _logger.LogInformation("Successfully added the employee {Name}", resourceExcel.Resource_Name);
                return OperationResult.Ok("resource added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while adding employee {Name}", resourceExcel.Resource_Name);
                return OperationResult.Fail($"Add failed: {ex.Message}");
            }
        }

        public async Task<OperationResult> UpdateEmployeeAsync(Resource resource)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_UpdateEmployee", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                if(resource == null)
                {
                    _logger.LogWarning("UpdateEmployeeAsync called with a null resource.");
                    return OperationResult.Fail("Invalid resource data.");
                }
                
                cmd.Parameters.AddWithValue("@EmpId", resource.EmpId);
                cmd.Parameters.AddWithValue("@Resource_Name", resource.Resource_Name);
                cmd.Parameters.AddWithValue("@Designation", resource.Designation);
                cmd.Parameters.AddWithValue("@ReportingTo", resource.ReportingTo);
                cmd.Parameters.AddWithValue("@Billable", resource.Billable);
                cmd.Parameters.AddWithValue("@Technology_Skill", resource.Technology_Skill);
                cmd.Parameters.AddWithValue("@Project_Allocate", resource.Project_Allocate);
                cmd.Parameters.AddWithValue("@Location", resource.Location);
                cmd.Parameters.AddWithValue("@EmailId", resource.EmailId);
                cmd.Parameters.AddWithValue("@CTE_DOJ", resource.CTE_DOJ);
                cmd.Parameters.AddWithValue("@Remarks", resource.Remarks);

                _logger.LogInformation("Excecuted the stored procedure sp_UpdateEmployee for employee {Name}",resource.Resource_Name);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                _logger.LogInformation("Successfully updated the employee {Name}",resource.Resource_Name);
                return OperationResult.Ok("resource updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating the employee {Name}", resource.Resource_Name);
                return OperationResult.Fail($"Update failed: {ex.Message}");
            }
        }

        public async Task<OperationResult> BulkUpdateEmployeesAsync(List<Resource> resources)
        {
            if (resources == null || !resources.Any())
            {
                _logger.LogWarning("BulkUpdateEmployeesAsync called with empty resource list.");
                return OperationResult.Fail("No resources to update.");
            }

            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                foreach (var resource in resources)
                {
                    using SqlCommand cmd = new SqlCommand("sp_UpdateEmployeePartial", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //cmd.Parameters.AddWithValue("@EmpId", resource.EmpId);
                    ////cmd.Parameters.AddWithValue("@Resource_Name", resource.Resource_Name);
                    //cmd.Parameters.AddWithValue("@Designation", resource.Designation);
                    //cmd.Parameters.AddWithValue("@ReportingTo", resource.ReportingTo);
                    //cmd.Parameters.AddWithValue("@Billable", resource.Billable);
                    ////cmd.Parameters.AddWithValue("@Technology_Skill", resource.Technology_Skill);
                    //cmd.Parameters.AddWithValue("@Project_Allocate", resource.Project_Allocate);
                    //cmd.Parameters.AddWithValue("@Location", resource.Location);
                    ////cmd.Parameters.AddWithValue("@EmailId", resource.EmailId);
                    ////cmd.Parameters.AddWithValue("@CTE_DOJ", resource.CTE_DOJ);
                    ////cmd.Parameters.AddWithValue("@Remarks", resource.Remarks);
                    if (resource.EmpId != null)
                    {
                        cmd.Parameters.AddWithValue("@EmpId", resource.EmpId ?? (object)DBNull.Value);
                    }
                    if (resource.Designation != null)
                    {
                        cmd.Parameters.AddWithValue("@Designation", (object?)resource.Designation ?? DBNull.Value);
                    }
                    if (resource.ReportingTo != null)
                    {
                        cmd.Parameters.AddWithValue("@ReportingTo", (object?)resource.ReportingTo ?? DBNull.Value);
                    }
                    if (resource.Billable != null)
                    {
                        cmd.Parameters.AddWithValue("@Billable", (object?)resource.Billable ?? DBNull.Value);
                    }
                    if (resource.Project_Allocate != null)
                    {
                        cmd.Parameters.AddWithValue("@Project_Allocate", (object?)resource.Project_Allocate ?? DBNull.Value);
                    }
                    if (resource.Location != null)
                    {
                        cmd.Parameters.AddWithValue("@Location", (object?)resource.Location ?? DBNull.Value);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Bulk update successful for {Count} employees.", resources.Count);
                return OperationResult.Ok("Bulk update successful.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError("Bulk update failed: {Message}", ex.Message);
                return OperationResult.Fail($"Bulk update failed: {ex.Message}");
            }
        }


        public async Task<OperationResult> DeleteEmployeeAsync(string empId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_DeleteEmployee", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                if (string.IsNullOrWhiteSpace(empId))
                {
                    _logger.LogWarning("DeleteEmployeeAsync failed: empId is null");
                    return OperationResult.Fail("EmpId is required.");
                }

                cmd.Parameters.AddWithValue("@EmpId", empId);

                _logger.LogInformation("Executed the stored procedure sp_DeleteEmployee for the employee with Id:{empId}", empId);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                _logger.LogInformation("Sucessfully deleted the employee with Id: {empId}", empId);
                return OperationResult.Ok("resource deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in deleting the employee with Id: {empId}", empId);
                return OperationResult.Fail($"Delete failed: {ex.Message}");
            }
        }

        public async Task<Resource?> GetEmployeeByIdAsync(string empId)
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(empId)) 
                {
                    _logger.LogWarning("GetEmployeeByIdAsync called by a null or empty empId");
   
                   
                }

                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_GetEmployeeById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@EmpId", empId);

                _logger.LogInformation("Executed the stored procedure sp_GetEmployeeById for employee with Id:{empId}",empId);
                await conn.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    _logger.LogInformation("Successfully retrieved the information of employee with Id:{empId}", empId);
                    return new Resource
                    {
                        EmpId = reader["EmpId"].ToString(),
                        Resource_Name = reader["Resource_Name"].ToString()!,
                        Designation = reader["Designation"].ToString()!,
                        ReportingTo = reader["ReportingTo"].ToString()!,
                        Billable = reader["Billable"].ToString()!,
                        Technology_Skill = reader["Technology_Skill"].ToString()!,
                        Project_Allocate = reader["Project_Allocate"].ToString()!,
                        Location = reader["Location"].ToString()!,
                        EmailId = reader["EmailId"].ToString()!,
                        CTE_DOJ = DateOnly.FromDateTime((DateTime)reader["CTE_DOJ"]),
                        Remarks = reader["Remarks"].ToString()!
                    };
                }
                else 
                {
                    _logger.LogWarning("No Employee found with empId:{empId}", empId);
                    return null;
                }
                    
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,"Error occured while retriving the employee with Id:{empId}",empId);
                return null;

            }
                    
        }

        public async Task<List<Resource>> GetAllEmployeesAsync()
        {
            try
            {
                var resources = new List<Resource>();


                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _logger.LogInformation("Executed the stored procedure sp_GetAllEmployees ");
                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    resources.Add(new Resource
                    {
                        EmpId = reader["EmpId"].ToString(),
                        Resource_Name = reader["Resource_Name"].ToString()!,
                        Designation = reader["Designation"].ToString()!,
                        ReportingTo = reader["ReportingTo"].ToString()!,
                        Billable = reader["Billable"].ToString()!,
                        Technology_Skill = reader["Technology_Skill"].ToString()!,
                        Project_Allocate = reader["Project_Allocate"].ToString()!,
                        Location = reader["Location"].ToString()!,
                        EmailId = reader["EmailId"].ToString()!,
                        CTE_DOJ = DateOnly.FromDateTime((DateTime)reader["CTE_DOJ"]),
                        Remarks = reader["Remarks"].ToString()!
                    });
                }

                _logger.LogInformation("Successfully retrieved the employee data.");
                return resources;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retriving the information of all the employees.");
                return new List<Resource>();
            }
            
        }

    

    public async Task<List<Resource>> GetAllEmployeesAsyncDownload()
        {
            try
            {
                var resources = new List<Resource>();


                using SqlConnection conn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("sp_GetAllEmployees_download", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _logger.LogInformation("Executed the stored procedure sp_GetAllEmployees_download ");
                await conn.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    resources.Add(new Resource
                    {
                        EmpId = reader["EmpId"].ToString(),
                        Resource_Name = reader["Resource_Name"].ToString()!,
                        Designation = reader["Designation"].ToString()!,
                        ReportingTo = reader["ReportingTo"].ToString()!,
                        Billable = reader["Billable"].ToString()!,
                        Technology_Skill = reader["Technology_Skill"].ToString()!,
                        Project_Allocate = reader["Project_Allocate"].ToString()!,
                        Location = reader["Location"].ToString()!,
                        EmailId = reader["EmailId"].ToString()!,
                        CTE_DOJ = DateOnly.FromDateTime((DateTime)reader["CTE_DOJ"]),
                        Remarks = reader["Remarks"].ToString()!,
                        ExportedAt = reader["ExportedAt"] != DBNull.Value ? (DateTime)reader["ExportedAt"] : DateTime.MinValue
                    });
                }

                _logger.LogInformation("Successfully retrieved the employee data.");
                return resources;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retriving the information of all the employees.");
                return new List<Resource>();

            }

        }

    }
}
