using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Shared.Interfaces;

namespace Infrastructure.Repositories.UserRepositories;

public class ProfileRepository : BaseRepository<ProfileEntity, UserDataContext>
{
    private readonly UserDataContext _context;
    private readonly IErrorLogger _errorLogger;

    public ProfileRepository(UserDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }
}
