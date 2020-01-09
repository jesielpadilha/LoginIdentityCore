using Microsoft.AspNetCore.Identity;
using System;

namespace LoginIdentityCore.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
