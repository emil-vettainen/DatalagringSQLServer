using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Shared.Interfaces;


namespace Infrastructure.Repositories.ProductRepositories
{
    public class ProductPriceRepository : BaseRepository<ProductPriceEntity, ProductDataContext>
    {
        private readonly ProductDataContext _context;
        private readonly IErrorLogger _errorLogger;

        public ProductPriceRepository(ProductDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }
    }
}
