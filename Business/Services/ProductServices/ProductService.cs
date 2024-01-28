using Business.Dtos.ProductDtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Entities.UserEntities;
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

        var manufactureId = await GetOrCreateManufactureAsync(createProductDto.ManufactureName);


      



       


        var mainCategory = await GetOrCreateCategoryAsync(createProductDto.CategoryName);
        var subCategory = await GetOrCreateCategoryAsync(createProductDto.SubCategoryName, mainCategory.Id);




        var productEntity = new ProductEntity
        {
            ArticleNumber = createProductDto.ArticleNumber,
            Title = createProductDto.Title,
            Description = createProductDto.Description,
            Specification = createProductDto.Specification,
            ManufactureId = manufactureId,
           

        };

        productEntity.Categories.Add(subCategory);
        productEntity.Categories.Add(mainCategory);

        var createdProduct = await _productRepository.CreateAsync(productEntity);

        var priceEntity = new ProductPriceEntity
        {
            ArticleNumber = createdProduct.ArticleNumber,
            Price = createProductDto.Price,
        };



        var price = await _productPriceRepository.CreateAsync(priceEntity);



        UpdateProductList?.Invoke(this, EventArgs.Empty);
        return true;
    }


    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        if (products != null)
        {

           

            var productDto = products.Select(x => 
            {
                var mainCategory = x.Categories.FirstOrDefault(x => x.ParentCategoryId == null)?.CategoryName ?? "";
                var subCategory = x.Categories.FirstOrDefault(x => x.ParentCategoryId != null)?.CategoryName ?? "";

                return new ProductDto
                {
                    ArticleNumber = x.ArticleNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Specification = x.Specification,
                    ManufactureName = x.Manufacture.Manufacturers,
                    Price = x.ProductPriceEntity.Price,
                    CategoryName = mainCategory,
                    SubCategoryName = subCategory,
                };

           
                
            });

            return productDto;
        }


        return Enumerable.Empty<ProductDto>();
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

    private async Task<CategoryEntity> GetOrCreateCategoryAsync(string categoryName, int? parentCategoryId = null)
    {
        try
        {
            var categoryExists = await _categoryRepository.ExistsAsync(x => x.CategoryName == categoryName && x.ParentCategoryId == parentCategoryId);
            if (categoryExists)
            {
                var existingCategory = await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName);
                return existingCategory;
            }
            else
            {
                var categoryEntity = new CategoryEntity { CategoryName = categoryName, ParentCategoryId = parentCategoryId };
                var createdCategory = await _categoryRepository.CreateAsync(categoryEntity);
                return createdCategory;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateCategoryAsync"); }
        return null!;
    }



    private async Task<int> GetOrCreateManufactureAsync(string manufactureName)
    {
        try
        {
            var manufactureExists = await _manufacturerRepository.ExistsAsync(x => x.Manufacturers == manufactureName);
            if (manufactureExists)
            {
                var existingManufacture = await _manufacturerRepository.GetOneAsync(x => x.Manufacturers == manufactureName);
                return existingManufacture.Id;
            }
            else
            {
                var manufactureEntity = new ManufactureEntity { Manufacturers = manufactureName};
                var createdManufacture = await _manufacturerRepository.CreateAsync(manufactureEntity);
                return manufactureEntity.Id;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateAddressAsync"); }
        return 0;
    }

}
