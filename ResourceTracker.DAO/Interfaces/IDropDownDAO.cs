using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.DAO.Interfaces
{
    public interface IDropDownDAO
    {
        Task<List<DesignationModel>> GetDesignationsAsync();
        Task<List<LocationModel>> GetLocationsAsync();
        Task<List<SkillModel>> GetSkillsAsync();
        Task<List<ProjectModel>> GetProjectsAsync();
        Task<List<ManagerModel>> GetManagersAsync();
       

    }
}
