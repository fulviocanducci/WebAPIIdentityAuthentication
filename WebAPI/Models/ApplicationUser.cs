using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models
{
    public class ApplicationUser: IdentityUser<long>
    {
        public string Name { get; set; }
    }
}
