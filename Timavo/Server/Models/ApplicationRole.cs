using Microsoft.AspNetCore.Identity;

namespace Timavo.Server.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = null!;
    }
}
