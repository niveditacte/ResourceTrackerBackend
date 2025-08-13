using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ResourceTracker.DAO.Interfaces;
using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.DAO
{
    public class DropDwnDAO: IDropDownDAO
    {
        private readonly string _connectionString;
       

        public DropDwnDAO(IConfiguration config)
        {
            this._connectionString = config.GetConnectionString("DefaultConnection")!;
        }
        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task<List<DesignationModel>> GetDesignationsAsync()
        {
            var list = new List<DesignationModel>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetAllDesignations", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new DesignationModel
                {
                    DesignationId = reader.GetInt32(0),
                    Designation_Name = reader.GetString(1)
                });
            }

            return list;
        }


        public async Task<List<LocationModel>> GetLocationsAsync()
        {
            var list = new List<LocationModel>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetAllLocations", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new LocationModel
                {
                    LocationId = reader.GetInt32(0),
                    Location_Name = reader.GetString(1)
                });
            }
            return list;
        }

        public async Task<List<SkillModel>> GetSkillsAsync()
        {
            var list = new List<SkillModel>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetAllSkills", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new SkillModel
                {
                    SkillId = reader.GetInt32(0),
                    Skill_Name = reader.GetString(1)
                });
            }

            return list;
        }

        public async Task<List<ProjectModel>> GetProjectsAsync()
        {
            var list = new List<ProjectModel>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetAllProjects", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ProjectModel
                {
                    ProjectId = reader.GetInt32(0),
                    Project_Name = reader.GetString(1)
                });
            }

            return list;
        }

        public async Task<List<ManagerModel>> GetManagersAsync()
        {
            var list = new List<ManagerModel>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetAllManagers", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ManagerModel
                {
                    ManagerId = reader.GetInt32(0),
                    Manager_Name = reader.GetString(1)
                });
            }

            return list;
        }

        public async Task<List<RoleModel>> GetRolesAsync()
        {
            var list = new List<RoleModel>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetAllRoles", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new RoleModel
                {
                    RoleId = reader.GetInt32(0),
                    Role = reader.GetString(1)
                });
            }

            return list;
        }
    }
}
