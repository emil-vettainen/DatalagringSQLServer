using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Shared.Interfaces;

namespace Infrastructure.Repositories.UserRepositories;

public class RoleRepository : BaseRepository<RoleEntity, UserDataContexts>
{
    private readonly UserDataContexts _context;
    private readonly IErrorLogger _errorLogger;
    public RoleRepository(UserDataContexts context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }
}
