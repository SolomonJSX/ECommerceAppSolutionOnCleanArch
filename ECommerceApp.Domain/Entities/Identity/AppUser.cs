using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Domain.Entities.Identity;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } =  string.Empty;
}