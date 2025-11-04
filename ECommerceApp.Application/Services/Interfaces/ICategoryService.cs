using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Category;

namespace ECommerceApp.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CreateCategory>> GetAllAsync();
    Task<GetCategory> GetByIdAsync(Guid id);
    Task<ServiceResponse> AddAsync(CreateCategory entity);
    Task<ServiceResponse> UpdateAsync(UpdateCategory entity);
    Task<ServiceResponse> DeleteAsync(Guid id);
}