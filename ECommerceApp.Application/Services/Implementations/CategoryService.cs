using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Category;
using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services.Implementations;

public class CategoryService(IGeneric<Category> productInterface, IMapper mapper) : ICategoryService
{
    public async Task<IEnumerable<GetCategory>> GetAllAsync()
    {
        var rawData = await productInterface.GetAllAsync();
        
        if (!rawData.Any()) return [];
        
        return mapper.Map<IEnumerable<GetCategory>>(rawData);
    }

    public async Task<GetCategory> GetByIdAsync(Guid id)
    {
        var rawData = await productInterface.GetByIdAsync(id);

        return mapper.Map<GetCategory>(rawData);
    }

    public async Task<ServiceResponse> AddAsync(CreateCategory category)
    {
        var mappedData = mapper.Map<Category>(category);
        int result = await productInterface.AddAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Product added!") : new ServiceResponse(false, "Product failed to add!");
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateCategory entity)
    {
        var mappedData = mapper.Map<Category>(entity);
        int result = await productInterface.UpdateAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Product updated!") : new ServiceResponse(false, "Product failed to update!");
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        int result = await productInterface.DeleteAsync(id);
        return result > 0 ? new ServiceResponse(true, "Product deleted!") : new ServiceResponse(false, "Product failed to delete!");
    }
}