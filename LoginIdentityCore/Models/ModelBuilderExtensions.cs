using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace LoginIdentityCore.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            //insert All necessary Roles
            builder.Entity<IdentityRole<Guid>>().HasData(new
            {
                Id = UserCategory.AdminId,
                Name = UserCategory.AdminDescription,
                NormalizedName = UserCategory.AdminDescription.ToUpper()
            });
        }
    }
}
