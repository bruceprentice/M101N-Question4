using System.Security.Claims;
using System.Threading.Tasks;
using M101N.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Options;

public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<M101N.Models.ApplicationUser, IdentityRole>
{
    public AppClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager
        , RoleManager<IdentityRole> roleManager
        , IOptions<Microsoft.AspNetCore.Builder.IdentityOptions> optionsAccessor)
    : base(userManager, roleManager, optionsAccessor)
    { }

    public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);

        if (!string.IsNullOrWhiteSpace(user.Name))
        {
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(ClaimTypes.GivenName, user.Name)
            });
        }
        return principal;
    }
}