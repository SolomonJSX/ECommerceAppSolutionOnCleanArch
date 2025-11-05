using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Product;

namespace ECommerceApp.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<GetProduct>> GetAllAsync();
    Task<GetProduct> GetByIdAsync(Guid id);
    Task<ServiceResponse> AddAsync(CreateProduct entity);
    Task<ServiceResponse> UpdateAsync(UpdateProduct entity);
    Task<ServiceResponse> DeleteAsync(Guid id);
}