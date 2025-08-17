using ResourceTracker.DAO;
using ResourceTracker.Models;
using ResourceTracker.Orchestration.Interfaces;
using ResourceTracker.Orchestration.Services;
using ResourceTracker.Orchestration.Utilities;
using ResourceTracker.DAO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Orchestration
{
    public class AuthOrchestration: IAuthOrchestration
    {
        private readonly IAuthDAO _dao;
        private readonly JWTService _jwt;

        public AuthOrchestration(IAuthDAO dao, JWTService jwt)
        {
            _dao = dao;
            _jwt = jwt;
        }

        public void Register(RegisterRequestDto dto)
        {
            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.HashPassword(dto.Password, salt);
            _dao.Register(dto.Username, dto.Email, hash, salt, dto.RoleId);
        }

        public string? Login(LoginRequestDto dto)
        {
            var user = _dao.Authenticate(dto.EmailorUsername);
            if (user == null) return null;
            var isValid = PasswordHelper.Verify(dto.Password, user.Salt, user.PasswordHash);
            return isValid ? _jwt.GenerateToken(user) : null;
        }

    }
}
