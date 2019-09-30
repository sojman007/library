using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.API.Extensions;
using Library.API.FauxDb;
using Library.API.FauxDb.Entities;
using Library.API.Filters;
using Library.BLL.Dto.RequestModel;
using Library.BLL.Dto.ResponseModel;
using Library.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _context;

        public AccountController(IConfiguration config, IUserService userService, IHttpContextAccessor context)
        {
            this._config = config;
            this._userService = userService;
            this._context = context;
        }

        [AllowAnonymous, HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequestModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorsAsList());
                var existingUser = this._userService.GetUserWithCredentials(loginModel.Email, loginModel.Password);
                if (existingUser == null)
                    return Unauthorized();

                var token = generateToken(existingUser);
                this._userService.CreateUserToken(token, existingUser.Id);
                return Ok(new { Message = "Log in successful", token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpPost]
        [Route("logout")]
        [CustomAuthorization]
        public IActionResult Logout()
        {
            try
            {
                var token = this._context.HttpContext.Request.Headers[Constants.AuthorizationText].FirstOrDefault().Substring(Constants.BearerText.Length).Trim();
                this._userService.InvalidateToken(token);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpPost]
        [Route("logoutall")]
        [CustomAuthorization]
        public IActionResult LogoutAll()
        {
            try
            {
                var userEmail = this._context.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
                this._userService.InvalidateAllToken(userEmail);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpGet]
        [Route("allusers")]
        [CustomAuthorization(ClaimTypes.Role, "admin")]
        public IActionResult AllUsers([FromQuery]SearchUserRequestModel model, int page = 1, int size = 10)
        {
            try
            {
                page = page > 0 ? page : 1;
                size = size > 0 ? size : 10;
                var users = this._userService.SearchForUser(model, page, size);
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpPost]
        [Route("createuser")]
        [CustomAuthorization(ClaimTypes.Role, "admin")]
        public async Task<IActionResult> CreateUser(CreateUserRequestModel model)
        {
            try
            {
                var user = await this._userService.CreateUserAsync(model);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        [HttpDelete]
        [Route("deleteuser")]
        [CustomAuthorization(ClaimTypes.Role, "admin")]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            try
            {
                var response = await this._userService.DeleteUserAsync(userId);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // log exception
                return StatusCode(500, Constants.ServerErrorMessageText);
            }
        }

        private string generateToken(UserResponseModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : ""),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], string.Empty, claims, DateTime.Now, DateTime.Now.AddDays(1), credential);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}