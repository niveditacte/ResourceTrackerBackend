using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Orchestration.Interfaces
{
    public interface IAuthOrchestration
    {
        public void Register(RegisterRequestDto dto);
        public string? Login(LoginRequestDto dto);
    }
}
