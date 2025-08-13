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
    public class AuthDAO : IAuthDAO
    {
        private readonly string _connectionString;
        public AuthDAO(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public void Register(string username, string email, byte[] hash, byte[] salt, int? roleId=null)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_RegisterUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", hash);
            cmd.Parameters.AddWithValue("@Salt", salt);
            if (roleId.HasValue)
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleId.Value;
            }
            else
            {
                cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = DBNull.Value;
            }
                conn.Open();
            cmd.ExecuteNonQuery();
        }
        public UserDto? Authenticate(string EmailorUsername)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_AuthenticateUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Login", EmailorUsername);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read()) return null;
            return new UserDto
            {
                Id = (int)rdr["Id"],
                Email = rdr["Email"].ToString(),
                PasswordHash = (byte[])rdr["PasswordHash"],
                Salt = (byte[])rdr["Salt"],
                RoleId = rdr["RoleId"] == DBNull.Value ? (int?)null : (int)rdr["RoleId"],
                Role = rdr["RoleName"] == DBNull.Value ? null : rdr["RoleName"].ToString(),
                Username = rdr["Username"].ToString()
            };
        }
    }
}
