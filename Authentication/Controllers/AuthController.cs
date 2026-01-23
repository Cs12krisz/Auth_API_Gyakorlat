using Authentication.Models.Dtos;
using Authentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var user = await _auth.Register(registerRequestDto);
            var result = user as ResponseDto;
            if (result.Result != null) 
            {
                return StatusCode(201, user);
            }

            return BadRequest(user);
        }

        [HttpPost("AssignRole")]
        public async Task<ActionResult> AssignRole(string userName, string roleName)
        {
            var response = await _auth.AssignRole(userName, roleName);
            var result = response as ResponseDto;
            if (result.Result != null)
            {
                return StatusCode(201, result);
            }

            return BadRequest(result);
        }
    }
}
