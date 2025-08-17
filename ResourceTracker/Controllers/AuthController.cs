using Microsoft.AspNetCore.Mvc;
using ResourceTracker.Models;
using ResourceTracker.Orchestration;
using ResourceTracker.Orchestration.Interfaces;

namespace ResourceTracker.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthOrchestration _auth;

        public AuthController(IAuthOrchestration auth) => _auth = auth;
        
        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto dto)
        {
            try
            {
                _auth.Register(dto);
                return Ok(new { message = "User registered successfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto dto)
        {
            var token = _auth.Login(dto);
            return token == null ? Unauthorized("Invalid credentials") : Ok(new { Token = token });
        }

    }
}
