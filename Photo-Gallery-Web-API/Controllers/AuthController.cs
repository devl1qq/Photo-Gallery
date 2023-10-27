using Microsoft.AspNetCore.Mvc;
using Photo_Gallery_Web_API.Dtos.AuthDtos;
using Photo_Gallery_Web_API.Middleware;
using Photo_Gallery_Web_API.Services.Auth;

namespace Photo_Gallery_Web_API.Controllers;

[Tags("Auth")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationError[]))]
        public async Task<IActionResult> Signup([FromBody] SignUp signupDTO)
        {
            var result = await _authService.SignupAsync(signupDTO);

            if (result.Succeeded)
            {
                return Ok("Signup successful");
            }

            var errors = result.Errors.Select(error => new ValidationError
            {
                Property = error.Code,
                Message = error.Description
            }).ToArray();

            return BadRequest(errors);
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public async Task<IActionResult> Signin([FromBody] SignIn signinDTO)
        {
            var token = await _authService.SigninAsync(signinDTO);

            if(token is not null)
            {
                
                return Ok(token);
            }               
            return Unauthorized("Signin failed");
        }
    }

