using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class ProductRepository : BaseRepository<ProductEntity, ProductDataContext>
    {
        private readonly ProductDataContext _context;
        private readonly IErrorLogger _errorLogger;

        public ProductRepository(ProductDataContext context, IErrorLogger errorLogger) : base(context, errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }

        public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _context.ProductEntities
                    .Include(x => x.ProductInfoEntity)
                    .Include(x => x.ProductPriceEntity)
                    .Include(x => x.Category)
                    .Include(x => x.Manufacture)
                    
                    .ToListAsync();
                if (entities.Count > 0)
                {
                    return entities;
                }
            }
            catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - GetAllAsync"); }
            return null!;
        }

        public override async Task<ProductEntity> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate)
        {
            try
            {
                var entity = await _context.ProductEntities

                    .Include(x => x.ProductInfoEntity)
                    .Include(x => x.ProductPriceEntity)

                    .FirstOrDefaultAsync(predicate);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - GetOneAsync"); }
            return null!;
        }
    }
}
