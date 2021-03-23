using Microsoft.AspNetCore.Identity;

namespace Imagegram.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
