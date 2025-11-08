using System.Security.Claims;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interfaces.Authentication;
using ECommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories.Authentication;

public class UserManagement(IRoleManagement roleManagement, UserManager<AppUser> userManager, AppDbContext dbContext) : IUserManagement
{
    public async Task<bool> CreateUser(AppUser user)
    {
        var userByEmail = await GetUserByEmail(user.Email!); 
        
        if (userByEmail != null) return false;
        
        return (await userManager.CreateAsync(user!, user.PasswordHash!)).Succeeded;
    }

    public async Task<bool> LoginUser(AppUser user)
    {
        var userByEmail = await GetUserByEmail(user.Email!);
        
        if (userByEmail is null) return false;

        string? roleName = await roleManagement.GetUserRole(userByEmail.Email!);
        
        if (roleName is null) return false;

        return await userManager.CheckPasswordAsync(userByEmail, user.PasswordHash!);
    }

    public async Task<AppUser?> GetUserByEmail(string email) => await userManager.FindByEmailAsync(email);

    public async Task<AppUser> GetUserById(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        return user!;
    }

    public async Task<IEnumerable<AppUser>?> GetAllUsers() => await dbContext.Users.ToListAsync();

    public async Task<int> RemoveUserByEmail(string email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(a => a.Email == email);
        dbContext.Users.Remove(user);
        return await dbContext.SaveChangesAsync();
    }

    public async Task<List<Claim>> GetUserClaims(string email)
    {
        var userByEmail = await GetUserByEmail(email);
        string? roleName = await roleManagement.GetUserRole(userByEmail!.Email!);

        List<Claim> claims =
        [
            new Claim("Fullname", userByEmail.Fullname),
            new Claim(ClaimTypes.NameIdentifier, userByEmail.Id),
            new Claim(ClaimTypes.Email, userByEmail!.Email!),
            new Claim(ClaimTypes.Role, roleName!)
        ];
        return claims;
    }
}