using System.Linq.Expressions;
using Identity.Domain.Interfaces.Repositories;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public abstract class Repository<T>(IdentityDbContext context) : IRepository<T>
    where T : class
{
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }
    
    public async Task<IEnumerable<T>> GetAllNoTrackingAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await context.Set<T>().AddRangeAsync(entities);
    }

    public async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}