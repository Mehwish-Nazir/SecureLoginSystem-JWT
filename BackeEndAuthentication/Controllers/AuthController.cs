using BackeEndAuthentication.DTO;
using BackeEndAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using BackeEndAuthentication.Helpers;
using BackeEndAuthentication.Middleware;

namespace BackeEndAuthentication.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
         
            //if Global Middleware file is writen then no need of try catch block , middleware automatically catches Excetion and return reposne accordng to specific exception 
            //but required in Service file
        }
    }
}
