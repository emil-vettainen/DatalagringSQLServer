using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Shared.Interfaces;


namespace Infrastructure.Repositories.ProductRepositories
{
    public class ManufactureRepository : BaseRepository<ManufactureEntity, ProductDataContexts>
    {
        private readonly ProductDataContexts _context;
        private readonly IErrorLogger _errorLogger;

        public ManufactureRepository(ProductDataContexts context, IErrorLogger errorLogger) : base(context, errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }
    }
}
