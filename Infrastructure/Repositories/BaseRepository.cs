using Infrastructure.Interfaces;
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

    // If exists (select 1 from tabke where id = 1)
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entityExists = await _context.Set<TEntity>().AnyAsync(predicate);
            return entityExists;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - ExistsAsync"); }
        return false;
    }


    // Insert into table values (value1, value2)
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

    // Select * from table
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            if (entities.Count != 0)
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

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null && updatedEntity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - UpdateAsync"); }
        return null!;
    }

    // Delete from table where Id = 1
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - DeleteAsync"); }
        return false;
    }
}