using Microsoft.AspNetCore.Identity;

namespace API.Entity.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string AccountEmail { get; set; }
    }
}
