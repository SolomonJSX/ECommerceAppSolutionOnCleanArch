using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Infrastructure.Repositories.Authentication;

public class RoleManagement(UserManager<AppUser> userManager) : IRoleManagement
{
    public async Task<string?> GetUserRole(string userEmail)
    {
        var user = await userManager.FindByEmailAsync(userEmail);
        return (await userManager.GetRolesAsync(user!)).FirstOrDefault();
    }

    public async Task<bool> AddUserToRole(AppUser user, string roleName) =>
        (await userManager.AddToRoleAsync(user, roleName)).Succeeded;
}