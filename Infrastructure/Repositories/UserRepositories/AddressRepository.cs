using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Shared.Interfaces;

namespace Infrastructure.Repositories.UserRepositories;

public class AddressRepository : BaseRepository<AddressEntity, UserDataContext>
{
    private readonly UserDataContext _context;
    private readonly IErrorLogger _errorLogger;

    public AddressRepository(UserDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }
}