using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComplejo.Presentation.Controllers
{
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
        [AllowAnonymous]
        public ActionResult<AuthResponse> SignUp([FromBody] SignUpRequest request)
        {
            var response = _authService.SignUp(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> SignIn([FromBody] SignInRequest request)
        {
            return Ok(_authService.SignIn(request));
        }
    }
}
