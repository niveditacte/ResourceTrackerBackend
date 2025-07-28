using Microsoft.AspNetCore.Mvc;
using ResourceTracker.Orchestration.Interfaces;

namespace ResourceTracker.Controllers
{
    [ApiController]
    [Route("api/resourcetracker")]
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

        [HttpGet("reportingto")]
        public async Task<IActionResult> GetReportingTo()
        {
            var list = await _orchestration.GetReportingToAsync();
            return Ok(list);
        }

    }
}
