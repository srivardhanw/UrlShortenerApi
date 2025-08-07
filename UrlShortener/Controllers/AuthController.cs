using AuthExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UrlShortener.DTOs.Request;
using UrlShortener.Enums;
using UrlShortener.ServiceContracts;

namespace AuthExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;

        public AuthController(IRegisterService registerService, ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService; 
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult> HandleSignUp([FromBody] RegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }
            var res = await _registerService.RegisterUser(user);

            return (res == RegisterResult.Success)? 
                Ok("registered successfully"):
                BadRequest("Username already exists or some error occured, could not register the user");
        }

        [HttpPost]
        [Route("login")]
        
        public async Task<ActionResult> HandleLogin([FromBody] LoginDTO user)
        {
           
            //if the details are generate a JWT and return in the UserDTO along with username
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage).ToList();
                return BadRequest(new { errors });
            }
            //check with db if username and password matches
            try
            {
                var auth = await _loginService.AuthenticateUserByUsernameOrEmailAndPassword(user.EmailOrUsername, user.Password);
                return Ok(auth);
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
