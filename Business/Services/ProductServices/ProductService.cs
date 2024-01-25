using Business.Dtos.ProductDtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Responses;
using Shared.Utilis;

namespace Business.Services.ProductServices;

public class ProductService(CategoryRepository categoryRepository, ProductRepository productRepository, ManufactureRepository manufacturerRepository, ProductPriceRepository productPriceRepository, IErrorLogger errorLogger)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly ProductRepository _productRepository = productRepository;
    private readonly ManufactureRepository _manufacturerRepository = manufacturerRepository;
    private readonly ProductPriceRepository _productPriceRepository = productPriceRepository;
    public EventHandler? UpdateProductList;

    private readonly IErrorLogger _errorLogger = errorLogger;

    private readonly IServiceResult _result = new ServiceResult();

    public async Task<bool> CreateProdukt(CreateProductDto createProductDto)
    {
        if (await _productRepository.ExistsAsync(x => x.ArticleNumber == createProductDto.ArticleNumber))
        {
            return false;
        }

        var manufactureEntity = new ManufactureEntity
        {
            Manufacturers = createProductDto.Manufacture,
        };

        var createdManufacture = await _manufacturerRepository.CreateAsync(manufactureEntity);


        var productEntity = new ProductEntity
        {
            ArticleNumber = createProductDto.ArticleNumber,
            Title = createProductDto.Title,
            Description = createProductDto.Description,
            Specification = createProductDto.Specification,
            ManufactureId = createdManufacture.Id,

        };

        var createdProduct = await _productRepository.CreateAsync(productEntity);

        var priceEntity = new ProductPriceEntity
        {
            ArticleNumber = createdProduct.ArticleNumber,
            Price = createProductDto.Price,
        };

        var price = await _productPriceRepository.CreateAsync(priceEntity);

        var categoryEntity = new CategoryEntity
        {
            CategoryName = createProductDto.CategoryName,
        };

        await _categoryRepository.CreateAsync(categoryEntity);
        UpdateProductList?.Invoke(this, EventArgs.Empty);
        return true;
    }


    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        if (products != null)
        {

            var productDto = products.Select(x => new ProductDto { Title = x.Title, Description = x.Description, Specification = x.Specification, ArticleNumber = x.ArticleNumber, Manufacture = x.Manufacture.Manufacturers });

            return productDto;
        }


        return null!;
    }


    public async Task<ProductDto> GetOneProductAsync(string articleNumber)
    {
        var product = await _productRepository.GetOneAsync(x => x.ArticleNumber == articleNumber);
        if (product != null)
        {
            var productDto = new ProductDto
            {
                ArticleNumber = product.ArticleNumber,
                Specification = product.Specification,
                Description = product.Description,
            };

            return productDto;
        }
        return null!;
    }


    public async Task<IServiceResult> DeleteProductByArticleNumber(string articleNumber)
    {
        try
        {
            var deletedProduct = await _productRepository.DeleteAsync(x => x.ArticleNumber == articleNumber);
            _result.Status = deletedProduct ? ResultStatus.Deleted : ResultStatus.NotFound;
        }
        catch (Exception ex)
        { _result.Status = ResultStatus.Failed; _errorLogger.ErrorLog(ex.Message, "UserRepo - DeleteUserByIdAsync"); }
        UpdateProductList?.Invoke(this, EventArgs.Empty);
        return _result;
    }



}
