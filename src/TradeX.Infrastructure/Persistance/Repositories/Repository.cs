using Microsoft.EntityFrameworkCore;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Shared;
using TradeX.Infrastructure.Persistance;
using TradeX.Infrastructure.Persistance.Extensions;

namespace Bookify.Infrastructure.Repositories;

internal abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginatedList<T>> GetAllAsync(int page , int pageSize)
    {
        return await DbSet.GetPaginationAsync(page,pageSize);
    }

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }
}