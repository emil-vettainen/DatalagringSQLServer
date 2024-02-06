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

        public override async Task<ProductPriceEntity> CreateAsync(ProductPriceEntity entity)
        {
            try
            {
                if(entity.Price != 0)
                {
                    _context.Set<ProductPriceEntity>().Add(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
            }
            catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - CreateAsync"); }
            return null!;
        }
    }
}
