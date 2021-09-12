using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResturantServices.Identity.Models;
namespace ResturantServices.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

    }
}