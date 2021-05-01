using FTEC5910.Shared.Features;
using FTEC5910.Shared.Entities.Dto;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace FTEC5910.Server.Controllers
{
    [ApiController]
    [Route("api/accounts")]

    public class AccountsController : ControllerBase
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        public AccountsController(UserManager<MyIdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var audienceList = new List<string>();
            _jwtSettings.GetSection("ValidAudiences").Bind(audienceList);

            var user = await _userManager.FindByNameAsync(userForAuthentication.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = JwtFunctions.GetSigningCredentials(_jwtSettings["SecurityKey"]);
            var claims = JwtFunctions.GetClaims(user);            
            var token = JwtFunctions.GenerateToken(signingCredentials, claims, _jwtSettings["ValidIssuer"], audienceList, _jwtSettings["ExpiryInMinutes"]);

            //var c = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var p = new ClaimsPrincipal(c);
            //var authProperties = new AuthenticationProperties
            //{
            //    AllowRefresh = false,
            //    IsPersistent = false,
            //   RedirectUri = this.Request.Host.Value
            //
            //};            
            //await ControllerContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, p, authProperties);
            //return SignIn( p, authProperties, CookieAuthenticationDefaults.AuthenticationScheme);            
            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        [HttpGet("UserInfo")]
        [Authorize]
        public async Task<IActionResult> UserInfo() 
        {
            try
            {
                var user = await _userManager.FindByNameAsync(Request.HttpContext.User.Identity.Name);
                return Ok(new GetUserResponseDto { FullName = user.FullName,Address = user.Address });
            }
            catch (Exception ex)
            {
                return Ok(new GetUserResponseDto { FullName = ex.Message });
            }
        }
    }
}
