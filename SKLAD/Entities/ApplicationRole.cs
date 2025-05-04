using Microsoft.AspNetCore.Identity;

namespace SKLAD.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
