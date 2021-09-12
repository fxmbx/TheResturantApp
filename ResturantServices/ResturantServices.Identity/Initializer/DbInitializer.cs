using ResturantServices.Identity.Data;
using ResturantServices.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;

namespace ResturantServices.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManger;
        private readonly RoleManager<IdentityRole> roleManager;
      
        public DbInitializer(ApplicationDbContext _dbcontext, UserManager<ApplicationUser> _usermanager, RoleManager<IdentityRole> _roleManager)
        {
            dbContext = _dbcontext;
            roleManager = _roleManager;
            userManger = _usermanager;
           
        }
        public void Initialize()
        {
            if(roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }else   { return; }

            ApplicationUser adminUser  = new ApplicationUser()
            {
                UserName  = "admin1@gmail.com",
                Email  = "funbiolaore@gmail.com",
                EmailConfirmed  = true,
                PhoneNumber  = "09068047434",
                FirstName = "Funbi",
                LastName = "Olaore"
            };
            userManger.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            userManger.AddToRoleAsync(adminUser,SD.Admin).GetAwaiter().GetResult();

            var temp1 =  userManger.AddClaimsAsync(adminUser, new Claim[] 
            {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName +" "+adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Admin),
            }).Result;

             ApplicationUser customerUser  = new ApplicationUser()
            {
                UserName  = "customer1@gmail.com",
                Email  = "customer1@gmail.com",
                EmailConfirmed  = true,
                PhoneNumber  = "09068047434",
                FirstName = "Funbi",
                LastName = "Cus"
            };
            userManger.CreateAsync(customerUser, "Cust123*").GetAwaiter().GetResult();
            userManger.AddToRoleAsync(customerUser,SD.Customer).GetAwaiter().GetResult();

            var temp2 =  userManger.AddClaimsAsync(customerUser, new Claim[] 
            {
                new Claim(JwtClaimTypes.Name, customerUser.FirstName +" "+customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Customer)
                // new Claim(JwtClaimTypes.Email, adminUser.Email)
            }).Result;
        }
    }
}