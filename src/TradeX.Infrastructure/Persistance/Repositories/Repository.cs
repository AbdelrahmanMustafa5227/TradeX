using Microsoft.EntityFrameworkCore;
using TradeX.Domain.Abstractions;
using TradeX.Infrastructure.Persistance;

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

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }
}