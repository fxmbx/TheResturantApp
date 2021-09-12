using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using ResturantServices.Identity.Models;

namespace ResturantServices.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ProfileService(UserManager<ApplicationUser> _userManager,
                              RoleManager<IdentityRole> _roleManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory)
        {
            userClaimsPrincipalFactory = _userClaimsPrincipalFactory;
            userManager = _userManager;
            roleManager = _roleManager; 
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
           string sub = context.Subject.GetSubjectId(); //subject contains user id in claims 
           ApplicationUser user = await userManager.FindByIdAsync(sub);
           ClaimsPrincipal userClaims = await userClaimsPrincipalFactory.CreateAsync(user);

           List<Claim> claims = userClaims.Claims.ToList();
           claims  =  claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
                        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                        claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
           
           if(userManager.SupportsUserRole)
           {
               IList<string> roles  =  await userManager.GetRolesAsync(user);
               foreach(var rolename in roles)
               {
                   claims.Add(new Claim(JwtClaimTypes.Role, rolename));
                   if(roleManager.SupportsRoleClaims)
                   {
                       IdentityRole role  = await roleManager.FindByNameAsync(rolename);
                       if(role != null)
                       {
                           claims.AddRange(await roleManager.GetClaimsAsync(role));
                       }
                   }
               }
           }
           context.IssuedClaims = claims;   
        }
        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await userManager.FindByIdAsync(sub);
            context.IsActive = user !=null;
        }
    }
}