using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Exceptions;
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
            try
            {
                var response = _authService.SignUp(request);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> SignIn([FromBody] SignInRequest request)
        {
            try
            {
                return Ok(_authService.SignIn(request));
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
