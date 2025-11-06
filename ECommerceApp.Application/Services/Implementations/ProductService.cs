using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services.Implementations;

public class ProductService(IGeneric<Product> productInterface, IMapper mapper) : IProductService
{
    public async Task<IEnumerable<GetProduct>> GetAllAsync()
    {
        var rawData = await productInterface.GetAllAsync();
        
        if (!rawData.Any()) return [];
        
        return mapper.Map<IEnumerable<GetProduct>>(rawData);
    }

    public async Task<GetProduct> GetByIdAsync(Guid id)
    {
        var rawData = await productInterface.GetByIdAsync(id);

        return mapper.Map<GetProduct>(rawData);
    }

    public async Task<ServiceResponse> AddAsync(CreateProduct productDto)
    {
        var mappedData = mapper.Map<Product>(productDto);
        int result = await productInterface.AddAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Product added!") : new ServiceResponse(false, "Product failed to add!");
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateProduct entity)
    {
        var mappedData = mapper.Map<Product>(entity);
        int result = await productInterface.UpdateAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Product updated!") : new ServiceResponse(false, "Product failed to update!");
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        int result = await productInterface.DeleteAsync(id);
        
        if (result == 0) return new ServiceResponse(true, "Product failed to be deleted!");
        
        return result > 0 ? new ServiceResponse(true, "Product deleted!") : new ServiceResponse(false, "Product not found failed to delete!");
    }
}