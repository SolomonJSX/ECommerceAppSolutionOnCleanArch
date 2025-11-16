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
            return new ServiceResponse() {Message = "Email address might be already in use or unknown error occured."};
        
        var userByEmail = await userManagement.GetUserByEmail(user.Email);
        var allUsers = await userManagement.GetAllUsers();
        
        bool assignedResult = await roleManagement.AddUserToRole(userByEmail!, allUsers!.Count() > 1 ? "User" : "Admin");

        if (!assignedResult)
        {
            var removeResult = await userManagement.RemoveUserByEmail(userByEmail!.Email!);

            if (removeResult >= 0)
            {
                appLogger.LogError(new Exception("Error while removing user"), "User could not be assigned to role");
                return new ServiceResponse() { Message = "Error while removing user" };
            }
        }

        return new ServiceResponse() { Success = true, Message = "Account created!" };
    }

    public async Task<LoginResponse> LoginUser(LoginUser user)
    {
        var validationResult = await validationService.ValidateAsync(user, loginUserValidator);
        
        if (!validationResult.Success)
            return new LoginResponse(Message: validationResult.Message);
        
        var mappedModel = mapper.Map<AppUser>(user);
        mappedModel.PasswordHash = user.Password;
        
        bool loginResult = await userManagement.LoginUser(mappedModel);
        
        if (!loginResult)
            return new LoginResponse(Message: "Email not found or invalid credentials!");
        
        var userByEmail = await userManagement.GetUserByEmail(user.Email);
        var claims = await userManagement.GetUserClaims(userByEmail!.Email!);

        string jwtToken = tokenManagement.GenerateToken(claims);
        string refreshToken = tokenManagement.GetRefreshToken();

        int saveTokenResult = 0;
        
        bool userTokenCheck = await tokenManagement.ValidateRefreshToken(refreshToken);
        
        if (userTokenCheck)
            saveTokenResult = await tokenManagement.UpdateRefreshToken(userByEmail.Id, refreshToken);
        else
            saveTokenResult = await tokenManagement.AddRefreshToken(userByEmail.Id, refreshToken);
        
        
        
        return saveTokenResult <= 0 ? new LoginResponse(Message: "Internal error occured while authenticating") : new LoginResponse(Success: true, Token: jwtToken, RefreshToken: refreshToken);
    }

    public async Task<LoginResponse> ReviveToken(string refreshToken)
    {
        bool validateRefreshTokenResult = await tokenManagement.ValidateRefreshToken(refreshToken);
        
        if (!validateRefreshTokenResult)
            return new LoginResponse(Message: "Invalid token");

        string userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);

        AppUser? user = await userManagement.GetUserById(userId);

        var userClaims = await userManagement.GetUserClaims(user.Email!);

        string newJwtToken = tokenManagement.GenerateToken(userClaims);
        string newRefreshToken = tokenManagement.GetRefreshToken();

        await tokenManagement.UpdateRefreshToken(userId, newRefreshToken);
        
        return new LoginResponse(Success: true, Token: newJwtToken, RefreshToken: newRefreshToken);
    }
}