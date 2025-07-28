using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Orchestration.Interfaces
{
    public interface IDropDownOrchestration
    {
        Task<List<string>> GetDesignationsAsync();
        Task<List<string>> GetLocationsAsync();
        Task<List<string>> GetSkillsAsync();
        Task<List<string>> GetProjectsAsync();
        Task<List<string>> GetReportingToAsync();
        

    }
}
