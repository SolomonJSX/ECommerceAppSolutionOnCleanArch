using ECommerceApp.Application.DTOs;
using FluentValidation;

namespace ECommerceApp.Application.Validations;

public interface IValidationService
{
    Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator);
}

public class ValidationService : IValidationService
{
    public async Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            string errorsStrings = string.Join("; ", validationErrors);
            return new ServiceResponse(false, Message: errorsStrings);
        }

        return new ServiceResponse(true);
    }
}