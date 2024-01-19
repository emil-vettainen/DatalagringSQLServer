using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IErrorLogger _errorLogger;


    protected BaseRepository(TContext context, IErrorLogger errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - CreateAsync"); }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            if(entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - GetAllAsync"); }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetTakeAsync(int take)
    {
        try
        {
            var entities = await _context.Set<TEntity>().Take(take).ToListAsync();
            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - GetTakeAsync"); }
        return null!;
    }

    public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - GetOneAsync"); }
        return null!;
    }
}
