using ResourceTracker.DAO.Interfaces;
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
        public Task<List<string>> GetDesignationsAsync()
        {
            return _dropdownDAO.GetDesignationsAsync();
        }

        public Task<List<string>> GetLocationsAsync()
        {
            return _dropdownDAO.GetLocationsAsync();
        }

        public Task<List<string>> GetSkillsAsync()
        {
            return _dropdownDAO.GetSkillsAsync();
        }

        public Task<List<string>> GetProjectsAsync()
        {
            return _dropdownDAO.GetProjectsAsync();
        }

        public Task<List<string>> GetReportingToAsync()
        {
            return _dropdownDAO.GetReportingToAsync();
        }
    }
}
