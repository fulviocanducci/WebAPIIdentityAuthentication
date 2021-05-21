using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class UserController : ControllerBase
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public UserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            return Ok(await UserManager.Users.Select(x => new UserModel(x.Name, x.Email)).ToListAsync());
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserModel>> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            return Ok(await UserManager.Users.Where(x => x.Email == email).Select(x => new UserModel(x.Name, x.Email)).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> Post(UserModel model)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ApplicationUser applicationUser = new()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    SecurityStamp = model.SecurityStamp().ToString()
                };
                IdentityResult result = await UserManager.CreateAsync(applicationUser, model.Password);
                await UserManager.AddToRolesAsync(applicationUser, new[] { "ADMINISTRATOR" });
                return Ok(new { status = result.Succeeded, model });
            }
            return BadRequest(new { Error = "E-mail invalid or existent" });
        }
    }
}
