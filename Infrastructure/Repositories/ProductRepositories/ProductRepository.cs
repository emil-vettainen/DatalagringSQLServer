using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class ProductRepository : BaseRepository<ProductEntity, ProductDataContexts>
    {
        private readonly ProductDataContexts _context;
        private readonly IErrorLogger _errorLogger;

        public ProductRepository(ProductDataContexts context, IErrorLogger errorLogger) : base(context, errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }

        public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _context.ProductEntities.Include(x => x.Categories).Include(x => x.Manufacture).ToListAsync();
                if(entities.Count > 0)
                {
                    return entities;
                }
            }
            catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - GetAllAsync"); }
            return null!;
        }
    }
}
