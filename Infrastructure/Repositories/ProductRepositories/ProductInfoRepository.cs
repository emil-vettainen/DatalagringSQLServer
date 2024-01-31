using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Shared.Interfaces;

namespace Infrastructure.Repositories.ProductRepositories;

public class ProductInfoRepository : BaseRepository<ProductInfoEntity, ProductDataContext>
{
    private readonly ProductDataContext _context;
    private readonly IErrorLogger _errorLogger;
    public ProductInfoRepository(ProductDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
    {
        _context = context;
        _errorLogger = errorLogger;
    }
}
