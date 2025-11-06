using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Category;
using ECommerceApp.Application.DTOs.Category;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services.Implementations;

public class CategoryService(IGeneric<Category> categoryInterface, IMapper mapper) : ICategoryService
{
    public async Task<IEnumerable<GetCategory>> GetAllAsync()
    {
        var rawData = await categoryInterface.GetAllAsync();
        
        if (!rawData.Any()) return [];
        
        return mapper.Map<IEnumerable<GetCategory>>(rawData);
    }

    public async Task<GetCategory> GetByIdAsync(Guid id)
    {
        var rawData = await categoryInterface.GetByIdAsync(id);

        return mapper.Map<GetCategory>(rawData);
    }

    public async Task<ServiceResponse> AddAsync(CreateCategory category)
    {
        var mappedData = mapper.Map<Category>(category);
        int result = await categoryInterface.AddAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Category added!") : new ServiceResponse(false, "Category failed to add!");
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateCategory entity)
    {
        var mappedData = mapper.Map<Category>(entity);
        int result = await categoryInterface.UpdateAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Category updated!") : new ServiceResponse(false, "Category failed to update!");
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        int result = await categoryInterface.DeleteAsync(id);
        
        if (result == 0) return new ServiceResponse(true, "Category failed to be deleted!");
        
        return result > 0 ? new ServiceResponse(true, "Category deleted!") : new ServiceResponse(false, "Category not found failed to delete!");
    }
}