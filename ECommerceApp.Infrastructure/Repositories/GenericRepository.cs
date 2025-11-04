using ECommerceApp.Application.Exceptions;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories;

public class GenericRepository<TEntity>(AppDbContext context) : IGeneric<TEntity> where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        var result = await context.Set<TEntity>().FindAsync(id) ?? throw new ItemNotFoundException($"Item with ID: {id} not found");
        return result;
    }

    public async Task<int> AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await context.Set<TEntity>().FindAsync(id) ?? throw new ItemNotFoundException($"Item with ID: {id} not found");
        
        context.Set<TEntity>().Remove(entity);
        return await context.SaveChangesAsync();
    }
}