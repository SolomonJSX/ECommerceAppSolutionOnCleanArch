using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Identity;
using ECommerceApp.Application.Services.Interfaces.Authentication;
using ECommerceApp.Application.Services.Interfaces.Logging;
using ECommerceApp.Domain.Interfaces.Authentication;

namespace ECommerceApp.Application.Services.Implementations.Authentication;

public class AuthenticationService(ITokenManagement tokenManagement, IUserManagement userManagement, IRoleManagement roleManagement, IAppLogger<AuthenticationService> appLogger, IMapper mapper) : IAuthenticationService
{
    public Task<ServiceResponse> CreateUser(CreateUser user)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> LoginUser(LoginUser user)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> ReviveToken(string refreshToken)
    {
        throw new NotImplementedException();
    }
}