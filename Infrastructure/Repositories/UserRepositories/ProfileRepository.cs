using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Shared.Interfaces;

namespace Infrastructure.Repositories.UserRepositories;

public class ProfileRepository(UserDataContext context, IErrorLogger errorLogger) : BaseRepository<ProfileEntity, UserDataContext>(context, errorLogger)
{
    private readonly UserDataContext _context = context;
    private readonly IErrorLogger _errorLogger = errorLogger;
}
