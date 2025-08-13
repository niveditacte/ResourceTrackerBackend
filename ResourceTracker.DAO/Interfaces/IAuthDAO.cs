using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.DAO.Interfaces
{
    public interface IAuthDAO
    {
        public void Register(string username, string email, byte[] hash, byte[] salt, int?roleId = null );
        public UserDto? Authenticate(string EmailorUsername);
    }
}
