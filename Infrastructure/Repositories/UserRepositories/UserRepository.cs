using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.UserRepositories;


public class UserRepository : BaseRepository<UserEntity, UserDataContexts>
{
    private readonly UserDataContexts _context;
    private readonly IErrorLogger _errorLogger;
    public UserRepository(UserDataContexts context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
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