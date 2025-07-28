using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ResourceTracker.DAO.Interfaces;
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
            this._connectionString = config.GetConnectionString("DefaultConnnection")!;
        }
        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        public async Task<List<string>> GetDesignationsAsync()
        {
            var list = new List<string>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetDesignations", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(reader.GetString(0));

            return list;
        }

        public async Task<List<string>> GetLocationsAsync()
        {
            var list = new List<string>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetLocations", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(reader.GetString(0));

            return list;
        }

        public async Task<List<string>> GetSkillsAsync()
        {
            var list = new List<string>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetTechnologySkills", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(reader.GetString(0));

            return list;
        }

        public async Task<List<string>> GetProjectsAsync()
        {
            var list = new List<string>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetProjects", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(reader.GetString(0));

            return list;
        }

        public async Task<List<string>> GetReportingToAsync()
        {
            var list = new List<string>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("sp_GetReportingTo", conn); 
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(reader.GetString(0));

            return list;
        }
    }
}
