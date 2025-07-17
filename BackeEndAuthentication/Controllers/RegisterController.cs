using Microsoft.AspNetCore.Mvc;
using BackeEndAuthentication.DTO;
using BackeEndAuthentication.Services;
using BackeEndAuthentication.CustomExceptions;
using BackeEndAuthentication.Helpers;
using BackeEndAuthentication.Models;
using BackeEndAuthentication.Repository;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace BackeEndAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IRegisterService registerService, ILogger<RegisterController> logger)
        {
            _registerService = registerService;
            _logger = logger;
        }

        [HttpPost("/register")]
        public async Task<ActionResult<ApiResponse<RegisterResponseDTO>>> RegisterAsync([FromForm] RegisterUserDTO register)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed",
                    Data = errors
                });
            }

            try
            {
                var response = await _registerService.RegisterUserAsync(register);
                var message = $"User '{response.Username}' has successfully registered with ID {response.UserID}.";

                return Ok(new ApiResponse<RegisterResponseDTO>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = message,
                    Data = response
                });
            }
            catch (ConflictException ex)
            {
                return Conflict(new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 409,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during registration.");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "An unexpected error occurred.",
                    Data = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}