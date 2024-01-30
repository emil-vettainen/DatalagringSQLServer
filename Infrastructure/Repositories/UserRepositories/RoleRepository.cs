using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Shared.Interfaces;

namespace Infrastructure.Repositories.UserRepositories;

public class RoleRepository : BaseRepository<RoleEntity, UserDataContext>
{
    private readonly UserDataContext _context;
    private readonly IErrorLogger _errorLogger;

    public RoleRepository(UserDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }
}
