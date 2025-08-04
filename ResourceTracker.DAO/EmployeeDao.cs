using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ResourceTracker.Models;
using ResourceTracker.DAO.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections;

namespace ResourceTracker.DAO
{
    public class EmployeeDao : IEmployeeDao
    {
        private readonly string  _connectionString;

        public EmployeeDao(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("Connection string: " + _connectionString);

        }

        public void InsertEmployee(EmployeeModel model)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_AddEmployeeWithDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Basic params
            cmd.Parameters.AddWithValue("@Employee_Name", model.Employee_Name);
            cmd.Parameters.AddWithValue("@DesignationId", model.DesignationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LocationId", model.LocationId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EmailId", model.EmailId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CTE_DOJ", model.CTE_DOJ);
            cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ManagerId", model.ManagerId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Billable", model.Billable ?? (object)DBNull.Value);

            // SkillIds TVP
            var skillTable = new DataTable();
            skillTable.Columns.Add("SkillId", typeof(int));
            if (model.SkillIds != null)
            {
                foreach (var id in model.SkillIds)
                    skillTable.Rows.Add(id);
            }
            var skillParam = cmd.Parameters.AddWithValue("@SkillIds", skillTable);
            skillParam.SqlDbType = SqlDbType.Structured;
            skillParam.TypeName = "SkillIdTableType";

            // ProjectIds TVP
            var projectTable = new DataTable();
            projectTable.Columns.Add("ProjectId", typeof(int));
            if (model.ProjectIds != null)
            {
                foreach (var id in model.ProjectIds)
                    projectTable.Rows.Add(id);
            }
            var projectParam = cmd.Parameters.AddWithValue("@ProjectIds", projectTable);
            projectParam.SqlDbType = SqlDbType.Structured;
            projectParam.TypeName = "ProjectIdTableType";

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        public void UpdateEmployee(EmployeeModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateEmployeeWithDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmpId", model.EmpId);
                    cmd.Parameters.AddWithValue("@Employee_Name", model.Employee_Name);
                    cmd.Parameters.AddWithValue("@DesignationId", (object?)model.DesignationId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LocationId", (object?)model.LocationId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmailId", (object?)model.EmailId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CTE_DOJ", (object?)model.CTE_DOJ ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Remarks", (object?)model.Remarks ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ManagerId", (object?)model.ManagerId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Billable", (object?)model.Billable ?? DBNull.Value);

                    // Add SkillIds as TVP
                    var skillTable = new DataTable();
                    skillTable.Columns.Add("SkillId", typeof(int));
                    foreach (var skillId in model.SkillIds)
                    {
                        skillTable.Rows.Add(skillId);
                    }
                    var skillParam = cmd.Parameters.AddWithValue("@SkillIds", skillTable);
                    skillParam.SqlDbType = SqlDbType.Structured;
                    skillParam.TypeName = "SkillIdTableType";

                    // Add ProjectIds as TVP
                    var projectTable = new DataTable();
                    projectTable.Columns.Add("ProjectId", typeof(int));
                    foreach (var projectId in model.ProjectIds)
                    {
                        projectTable.Rows.Add(projectId);
                    }
                    var projectParam = cmd.Parameters.AddWithValue("@ProjectIds", projectTable);
                    projectParam.SqlDbType = SqlDbType.Structured;
                    projectParam.TypeName = "ProjectIdTableType";

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEmployee(int empId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_DeleteEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", empId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Optionally log or rethrow
                    throw new Exception("Error occurred while deleting employee.", ex);
                }
            }
        }

        public EmployeeDetailsResponse GetEmployeeDetails(int empId)
        {
            var employee = new EmployeeDetailsResponse();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetEmployeeWithDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", empId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    // 1. Basic Info
                    if (reader.Read())
                    {
                        employee.EmpId = Convert.ToInt32(reader["EmpId"]);
                        employee.Employee_Name = reader["Employee_Name"].ToString();
                        employee.Designation_Name = reader["Designation_Name"]?.ToString();
                        employee.Location_Name = reader["Location_Name"]?.ToString();
                        employee.EmailId = reader["EmailId"]?.ToString();
                        employee.CTE_DOJ = reader["CTE_DOJ"] != DBNull.Value
                            ? Convert.ToDateTime(reader["CTE_DOJ"])
                            : (DateTime?)null;
                        employee.Remarks = reader["Remarks"]?.ToString();
                        employee.ReportingToName = reader["ReportingToName"]?.ToString();
                        employee.Billable = reader["Billable"]?.ToString();

                    }

                    // 2. Skills
                    if (reader.NextResult())
                    {
                        employee.Skills = new List<string>();
                        while (reader.Read())
                        {
                            var skillName = reader["Skill_Name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(skillName))
                            {
                                employee.Skills.Add(skillName);
                            }
                        }
                    }

                    // 3. Projects
                    if (reader.NextResult())
                    {
                        employee.Projects = new List<string>();
                        while (reader.Read())
                        {
                            var projectName = reader["Project_Name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(projectName))
                            {
                                employee.Projects.Add(projectName);
                            }
                        }
                    }
                }
            }

            return employee;
        }

        public EmployeeModel GetEmployeeById(int empId)
        {
            var employee = new EmployeeModel
            {
                SkillIds = new List<int>(),
                ProjectIds = new List<int>()
            };

            using var conn = new SqlConnection(_connectionString);

            // Step 1: Get employee data
            using (var cmd = new SqlCommand("SELECT * FROM Employees WHERE EmpId = @EmpId", conn))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee.EmpId = Convert.ToInt32(reader["EmpId"]);
                    employee.Employee_Name = reader["Employee_Name"]?.ToString();
                    employee.DesignationId = reader["DesignationId"] != DBNull.Value ? Convert.ToInt32(reader["DesignationId"]) : (int?)null;
                    employee.LocationId = reader["LocationId"] != DBNull.Value ? Convert.ToInt32(reader["LocationId"]) : (int?)null;
                    employee.EmailId = reader["EmailId"]?.ToString();
                    employee.CTE_DOJ = DateOnly.FromDateTime((DateTime)reader["CTE_DOJ"]);
                    employee.Remarks = reader["Remarks"]?.ToString();
                    employee.ManagerId = reader["ManagerId"] != DBNull.Value ? Convert.ToInt32(reader["ManagerId"]) : (int?)null;
                    employee.Billable = reader["Billable"]?.ToString();

                }
                conn.Close();
            }

            // Step 2: Get SkillIds
            using (var cmd = new SqlCommand("SELECT SkillId FROM EmployeeSkills WHERE EmpId = @EmpId", conn))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employee.SkillIds.Add(Convert.ToInt32(reader["SkillId"]));
                }
                conn.Close();
            }

            // Step 3: Get ProjectIds
            using (var cmd = new SqlCommand("SELECT ProjectId FROM EmployeeProjects WHERE EmpId = @EmpId", conn))
            {
                cmd.Parameters.AddWithValue("@EmpId", empId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employee.ProjectIds.Add(Convert.ToInt32(reader["ProjectId"]));
                }
                conn.Close();
            }

            return employee;
        }



        public List<EmployeeDetailsResponse> GetAllEmployees()
        {
            var list = new List<EmployeeDetailsResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ListAllEmployeesWithDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var skillsRaw = reader["Skills"]?.ToString();
                var projectsRaw = reader["Projects"]?.ToString();

                var skills = string.IsNullOrWhiteSpace(skillsRaw)
                     ? new List<string>()
                     : skillsRaw.Split(',').Select(s => s.Trim()).ToList();

                var projects = string.IsNullOrWhiteSpace(projectsRaw)
                    ? new List<string>()
                    : projectsRaw.Split(',').Select(p => p.Trim()).ToList();

                list.Add(new EmployeeDetailsResponse
                {
                    EmpId = Convert.ToInt32(reader["EmpId"]),
                    Employee_Name = reader["Employee_Name"]?.ToString() ?? "",
                    EmailId = reader["EmailId"]?.ToString() ?? "",
                    CTE_DOJ = reader["CTE_DOJ"] != DBNull.Value ? Convert.ToDateTime(reader["CTE_DOJ"]) : (DateTime?)null,
                    Remarks = reader["Remarks"]?.ToString() ?? "",
                    Designation_Name = reader["Designation_Name"]?.ToString() ?? "",
                    Location_Name = reader["Location_Name"]?.ToString() ?? "",
                    ReportingToName = reader["ReportingToName"]?.ToString() ?? "",
                    Billable = reader["Billable"]?.ToString() ?? "",

                    // Assuming no Skills or Projects come from this SP
                    Skills = skills,
                    Projects = projects
                });
            }

            return list;
        }

        public void BulkUpdateEmployee(BulkEmployeeModel model)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_BulkUpdateEmployee", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            var empIdsString = string.Join(",", model.EmpIds);
            var projectIdsString = model.ProjectId is int projList
                ? string.Join(",", projList)
                : model.ProjectId?.ToString();

            // 🔍 Print to console
            Console.WriteLine("EmpIds: " + empIdsString);
            Console.WriteLine("ProjectIds: " + projectIdsString);
            Console.Write("Project", model.ProjectId);
            cmd.Parameters.AddWithValue("@EmpIds", string.Join(",", model.EmpIds));
            cmd.Parameters.AddWithValue("@DesignationId", (object?)model.DesignationId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ManagerId", (object?)model.ManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Billable", (object?)model.Billable ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProjectIds", (object?)model.ProjectId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SkillIds", (object?)model.SkillIds ?? DBNull.Value);


            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Optionally log or rethrow
                throw new Exception("Error occurred during bulk employee update.", ex);
            }
        }


        // Helper Methods
        public int? GetDesignationId(string designationName)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_GetDesignationIdByName", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DesignationName", designationName?.Trim());

            var outputParam = new SqlParameter("@DesignationId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return outputParam.Value != DBNull.Value ? (int?)outputParam.Value : null;
        }


        public int? GetLocationId(string locationName)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_GetLocationIdByName", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@LocationName", locationName?.Trim());

            var outputParam = new SqlParameter("@LocationId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return outputParam.Value != DBNull.Value ? (int?)outputParam.Value : null;
        }


        public int? GetManagerId(string managerName)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_GetManagerIdByName", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ManagerName", managerName?.Trim());

            var outputParam = new SqlParameter("@ManagerId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return outputParam.Value != DBNull.Value ? (int?)outputParam.Value : null;
        }


        public List<int> GetSkillIds(List<string> skillNames)
        {
            var result = new List<int>();
            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));

            foreach (var skill in skillNames)
                table.Rows.Add(skill?.Trim());

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_GetSkillIdsByNames", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            var param = cmd.Parameters.AddWithValue("@SkillNames", table);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "dbo.StringList";

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(Convert.ToInt32(reader["SkillId"]));
            }

            return result;
        }


        public List<int> GetProjectIds(List<string> projectNames)
        {
            var result = new List<int>();
            if (projectNames == null || !projectNames.Any()) return result;

            string commaSeparated = string.Join(",", projectNames.Select(p => p.Trim()));

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_GetProjectIdsByNames", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProjectNames", commaSeparated);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(Convert.ToInt32(reader["ProjectId"]));
            }

            return result;
        }

        public List<ExportEmployeeModel> GetEmployeesForExport()
        {
            var employees = new List<ExportEmployeeModel>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetEmployeesForExport", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new ExportEmployeeModel
                        {
                            EmpId = reader.GetInt32(0),
                            Employee_Name = reader.GetString(1),
                            Designation_Name = reader.GetString(2),
                            ReportingToName = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Billable = reader.GetString(4),
                            Skills = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            Projects = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            Location_Name = reader.GetString(7),
                            EmailId = reader.GetString(8),
                            CTE_DOJ = DateOnly.FromDateTime(reader.GetDateTime(9)),
                            Remarks = reader.GetString(10),
                            ExportedAt = DateOnly.FromDateTime(reader.GetDateTime(11))
                        });
                    }
                }
            }

            return employees;
        }


    }
}
