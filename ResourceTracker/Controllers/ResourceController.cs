using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResourceTracker.DAO.Models;
using ResourceTracker.Orchestration.Interfaces;
using System.Resources;


namespace ResourceTracker.Controllers
{
    [ApiController]
    [Route("api/resourcetracker")]
    public class ResourceController(IResourceTrackerOrchestration orchestration, ILogger<ResourceController> logger) : ControllerBase
    {
        private readonly IResourceTrackerOrchestration _orchestration = orchestration;
        private readonly ILogger<ResourceController> _logger = logger;

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Resource>>> GetAll()
        {
            try 
            {
                var result = await _orchestration.GetAllEmployeesAsync();
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in fetching all resources.");
                return StatusCode(500, "Internal Server Error.");
            }
           
        }

        [HttpGet]
        [Route("download-csv")]
        public async Task<ActionResult<List<Resource>>> GetAllDownload()
        {
            try 
            {
                var result = await _orchestration.GetAllEmployeesAsyncDownload();
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching resources for CSV download.");
                return StatusCode(500, "Internal server error.");
            }

            
        }

        //🔹 GET: api/resourcetracker/{empId}
        [HttpGet]
        [Route("{empId}")]
        public async Task<ActionResult<Resource>> GetById(string empId)
        {
            try 
            {
                var resource = await _orchestration.GetEmployeeByIdAsync(empId);
                if (resource == null)
                {
                    _logger.LogWarning("Resource not found for EmpId: {EmpId}", empId);
                     return NotFound($"No resource found with EmpId: {empId}");
                }
                return Ok(resource);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching resource with EmpId: {EmpId}", empId);
                return StatusCode(500, "Internal server error.");

            }
            
            
        }

        // 🔹 POST: api/resourcetracker
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Add([FromBody] Resource resource)
        {

            try 
            {
                var result = await _orchestration.AddEmployeeAsync(resource);
                 if (!result.Success) 
                {
                    _logger.LogWarning("Failed to add resource: {@Resource}", resource);
                     return BadRequest(result);
                }
                
              return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding resource.");
                return StatusCode(500, "Internal server error.");

            }
        }

        // 🔹 PUT: api/resourcetracker
        [HttpPut]
        [Route("")]
        public async Task<ActionResult> Update([FromBody] Resource resource)
        {
            try 
            {
                var result = await _orchestration.UpdateEmployeeAsync(resource);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update resource: {@Resource}", resource);
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating resource.");
                return StatusCode(500, "Internal server error.");
            }
            
        }

        // 🔹 PUT: api/resourcetracker/bulk
        [HttpPut]
        [Route("bulk")]
        public async Task<ActionResult> BulkUpdate([FromBody] List<Resource> resources)
        {
            try
            {
                if (resources == null || resources.Count == 0)
                {
                    _logger.LogWarning("BulkUpdate called with empty or null resource list.");
                    return BadRequest("Resource list cannot be empty.");
                }

                var result = await _orchestration.BulkUpdateEmployeesAsync(resources);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to bulk update resources: {@Resources}", resources);
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while bulk updating resources.");
                return StatusCode(500, "Internal server error.");
            }
        }


        // 🔹 DELETE: api/resourcetracker/{empId}
        [HttpDelete]
        [Route("{empId}")]
        public async Task<ActionResult> Delete(string empId)
        {
            try
            {
                var result = await _orchestration.DeleteEmployeeAsync(empId);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to delete resource with EmpId: {EmpId}", empId);
                    return NotFound(result);
                }
                    

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting resource with EmpId: {EmpId}", empId);
                return StatusCode(500, "Internal server error.");
            }

        }

        //🔹 POST: api/resourcetracker/import
        [HttpPost("import")]

        public async Task<IActionResult> ImportExcel([FromBody] List<ResourcesExcel> resourcesExcel)
        {
            try
            {
                foreach (var resource in resourcesExcel)
                {
                    await _orchestration.AddEmployeeFromExcelAsync(resource);
                }

                return Ok(new { message = "Imported successfully." });
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Exception during Excel import.");
                return StatusCode(500, new { message = "Internal server error during import." });
              
            }
        }


    }
}
