using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Shared.Interfaces;

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
}


