using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        public JwtConfigurationModel JwtConfigurationModel { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<ApplicationRole> RoleManager { get; }

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, JwtConfigurationModel jwtConfigurationModel)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            JwtConfigurationModel = jwtConfigurationModel;
        }

        [HttpGet()]
        [Authorize()]
        public async Task<ActionResult<object>> Get()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var data = await UserManager.FindByNameAsync(User.Identity.Name);
                    return Ok(data);
                }
                return Unauthorized();
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpPost()]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null && await UserManager.CheckPasswordAsync(user, model.Password))
            {
                IList<string> userRoles = await UserManager.GetRolesAsync(user);
                List<Claim> authClaims = new()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                JwtSecurityToken token = new(
                    audience: JwtConfigurationModel.ValidAudience,
                    issuer: JwtConfigurationModel.ValidIssuer,
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(JwtConfigurationModel.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}
