using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Identity;
using ECommerceApp.Application.Services.Interfaces.Authentication;
using ECommerceApp.Application.Services.Interfaces.Logging;
using ECommerceApp.Application.Validations;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interfaces.Authentication;
using FluentValidation;

namespace ECommerceApp.Application.Services.Implementations.Authentication;

public class AuthenticationService(IValidationService validationService, IValidator<LoginUser> loginUserValidator, IValidator<CreateUser> createUserValidator, ITokenManagement tokenManagement, IUserManagement userManagement, IRoleManagement roleManagement, IAppLogger<AuthenticationService> appLogger, IMapper mapper) : IAuthenticationService
{
    public async Task<ServiceResponse> CreateUser(CreateUser user)
    {
        var validationResult = await validationService.ValidateAsync(user, createUserValidator);

        if (!validationResult.Success) return validationResult;

        var mappedModel = mapper.Map<AppUser>(user);
        
        mappedModel.UserName =  user.Email;
        mappedModel.PasswordHash = user.Password;

        var result = await userManagement.CreateUser(mappedModel);
        
        if (!result)
            return new ServiceResponse() {Message = ""}
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