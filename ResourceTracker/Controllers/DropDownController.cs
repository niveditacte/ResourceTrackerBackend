using Microsoft.AspNetCore.Mvc;
using ResourceTracker.Orchestration.Interfaces;

namespace ResourceTracker.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    public class DropDownController: ControllerBase
    {
        private readonly IDropDownOrchestration _orchestration;

        public DropDownController(IDropDownOrchestration orchestration)
        {
            _orchestration = orchestration;
        }
        [HttpGet("designations")]
        public async Task<IActionResult> GetDesignations()
        {
            var list = await _orchestration.GetDesignationsAsync();
            return Ok(list);
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations()
        {
            var list = await _orchestration.GetLocationsAsync();
            return Ok(list);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills()
        {
            var list = await _orchestration.GetSkillsAsync();
            return Ok(list);
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var list = await _orchestration.GetProjectsAsync();
            return Ok(list);
        }

        [HttpGet("managers")]
        public async Task<IActionResult> GetManagers()
        {
            var list = await _orchestration.GetManagersAsync();
            return Ok(list);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var list = await _orchestration.GetRolesAsync();
            return Ok(list);
        }

    }
}
