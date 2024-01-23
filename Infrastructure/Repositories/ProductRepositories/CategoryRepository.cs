using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Shared.Interfaces;


namespace Infrastructure.Repositories.ProductRepositories
{
    public class CategoryRepository : BaseRepository<CategoryEntity, ProductDataContexts>
    {
        private readonly ProductDataContexts _context;
        private readonly IErrorLogger _errorLogger;

        public CategoryRepository(ProductDataContexts context, IErrorLogger errorLogger) : base(context, errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }
    }
}
