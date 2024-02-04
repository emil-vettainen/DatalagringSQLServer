using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.UserRepositories;


public class UserRepository : BaseRepository<UserEntity, UserDataContext>
{
    private readonly UserDataContext _context;
    private readonly IErrorLogger _errorLogger;
    public UserRepository(UserDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }

    public override async Task<UserEntity> CreateAsync(UserEntity entity)
    {
        try
        {
            if(entity.Id != Guid.Empty && entity.RoleId != 0 && entity.AddressId != 0)
            {
                _context.Set<UserEntity>().Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
           
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - CreateAsync"); }
        return null!;
    }

    public override async Task<UserEntity> GetOneAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Users
                .Include(x => x.Profile)
                .Include(x => x.Role)
                .Include(x => x.Address)
                .Include(x => x.Authentication)
                .FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserRepo - GetOneAsync"); }
        return null!;
    }
}