using ResourceTracker.DAO.Interfaces;
using ResourceTracker.Models;
using ResourceTracker.Orchestration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Orchestration
{
    public class DropDownOrchestration: IDropDownOrchestration
    {
        private readonly IDropDownDAO _dropdownDAO;

        public DropDownOrchestration(IDropDownDAO dropdownDAO)
        {
            _dropdownDAO = dropdownDAO;
        }
        public Task<List<DesignationModel>> GetDesignationsAsync()
        {
            return _dropdownDAO.GetDesignationsAsync();
        }

        public Task<List<LocationModel>> GetLocationsAsync()
        {
            return _dropdownDAO.GetLocationsAsync();
        }

        public Task<List<SkillModel>> GetSkillsAsync()
        {
            return _dropdownDAO.GetSkillsAsync();
        }

        public Task<List<ProjectModel>> GetProjectsAsync()
        {
            return _dropdownDAO.GetProjectsAsync();
        }

        public Task<List<ManagerModel>> GetManagersAsync()
        {
            return _dropdownDAO.GetManagersAsync();
        }

        public Task<List<RoleModel>> GetRolesAsync()
        {
            return _dropdownDAO.GetRolesAsync();
        }
    }
}
