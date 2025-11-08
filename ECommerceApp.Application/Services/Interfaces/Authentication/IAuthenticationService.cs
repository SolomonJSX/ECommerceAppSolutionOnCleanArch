using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Identity;

namespace ECommerceApp.Application.Services.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<ServiceResponse> CreateUser(CreateUser user);
    Task<LoginResponse> LoginUser(LoginUser user);
    Task<LoginResponse> ReviveToken(string refreshToken);
}