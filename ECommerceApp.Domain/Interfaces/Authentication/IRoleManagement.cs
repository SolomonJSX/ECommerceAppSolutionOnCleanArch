using ECommerceApp.Domain.Entities.Identity;

namespace ECommerceApp.Domain.Interfaces.Authentication;

public interface IRoleManagement
{
    Task<string?> GetUserRole(string userEmail);
    Task<bool> AddUserToRole(AppUser user, string roleName);
}