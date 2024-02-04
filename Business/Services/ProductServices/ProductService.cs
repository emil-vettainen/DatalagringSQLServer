using Business.Dtos.ProductDtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Responses;

namespace Business.Services.ProductServices;

public class ProductService(CategoryRepository categoryRepository, ProductRepository productRepository, ManufactureRepository manufacturerRepository, ProductPriceRepository productPriceRepository, IErrorLogger errorLogger, ProductInfoRepository productInfoRepository)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly ProductRepository _productRepository = productRepository;
    private readonly ManufactureRepository _manufacturerRepository = manufacturerRepository;
    private readonly ProductPriceRepository _productPriceRepository = productPriceRepository;
    private readonly ProductInfoRepository _productInfoRepository = productInfoRepository;


    private readonly IErrorLogger _errorLogger = errorLogger;
    private readonly IServiceResult _result = new ServiceResult();
    public EventHandler? UpdateProductList;


    public async Task<IServiceResult> CreateProduktAsync(CreateProductDto dto)
    {
        try
        {
            if(await _productRepository.ExistsAsync(x => x.ArticleNumber == dto.ArticleNumber))
            {
                return new ServiceResult { Status = ResultStatus.AlreadyExist };
            }

            var manufactureId = await GetOrCreateManufactureAsync(dto.Manufacture);
            if(manufactureId == 0)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var categoryId = await GetOrCreateCategoryAsync(dto.CategoryName);
            if(categoryId == 0)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var productEntity = await CreateProductEntityAsync(dto.ArticleNumber, manufactureId, categoryId);
            if (!productEntity)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var productInfo = await CreateProductInfoEntityAsync(dto.ArticleNumber, dto.ProductTitle, dto.Ingress, dto.Description, dto.Specification);
            if (!productInfo)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var productPrice = await CreateProductPriceEntityAsync(dto.ArticleNumber, dto.Price);
            if (!productPrice)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }
            else
            {
                UpdateProductList?.Invoke(this, EventArgs.Empty);
                return new ServiceResult { Status = ResultStatus.Successed };
            }
        }
        catch (Exception ex){ _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProduktAsync"); return new ServiceResult { Status = ResultStatus.Failed };}
    }

    public async Task<int> GetOrCreateManufactureAsync(string manufacture)
    {
        try
        {
            if(manufacture != null)
            {
                var manufactureExists = await _manufacturerRepository.ExistsAsync(x => x.ManufactureName == manufacture);
                if (manufactureExists)
                {
                    var existingManufacture = await _manufacturerRepository.GetOneAsync(x => x.ManufactureName == manufacture);
                    return existingManufacture.Id;
                }
                else
                {
                    var manufactureEntity = new ManufactureEntity { ManufactureName = manufacture };
                    var createdManufacture = await _manufacturerRepository.CreateAsync(manufactureEntity);
                    return createdManufacture.Id;
                }
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - GetOrCreateManufactureAsync"); }
        return 0;
    }

    public async Task<int> GetOrCreateCategoryAsync(string categoryName)
    {
        try
        {
            if(categoryName != null)
            {
                var categoryNameExists = await _categoryRepository.ExistsAsync(x => x.CategoryName == categoryName);
                if (categoryNameExists)
                {
                    var existingCategoryName = await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName);
                    return existingCategoryName.Id;
                }
                else
                {
                    var categoryEntity = new CategoryEntity { CategoryName = categoryName };
                    var createdCategoryName = await _categoryRepository.CreateAsync(categoryEntity);
                    return createdCategoryName.Id;
                }
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - GetOrCreateCategoryAsync"); }
        return 0;
    }

    public async Task<bool> CreateProductEntityAsync(string articleNumber, int manufactureId, int categoryId)
    {
        try
        {
            if(articleNumber != null && manufactureId != 0 && categoryId != 0) 
            {
                var result = await _productRepository.CreateAsync(new ProductEntity
                {
                    ArticleNumber = articleNumber,
                    Created = DateTime.Now,
                    ManufactureId = manufactureId,
                    CategoryId = categoryId,
                });
                return result != null;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProductEntityAsync"); }
        return false;
    }
    
    public async Task<bool> CreateProductInfoEntityAsync(string articleNumber, string productTitle, string ingress, string description, string specification)
    {
        try
        {
            if(articleNumber != null && productTitle != null && ingress != null && description != null && specification != null)
            {
                var result = await _productInfoRepository.CreateAsync(new ProductInfoEntity
                {
                    ArticleNumber = articleNumber,
                    ProductTitle = productTitle,
                    Ingress = ingress,
                    Description = description,
                    Specification = specification,
                });
                return result != null;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProductInfoAsync"); }
        return false;
    }

    public async Task<bool> CreateProductPriceEntityAsync(string articleNumber, decimal price)
    {
        try
        {
            if(articleNumber != null && price != 0)
            {
                var result = await _productPriceRepository.CreateAsync(new ProductPriceEntity
                {
                    ArticleNumber = articleNumber,
                    Price = price,
                });
                return result != null;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProductPriceAsync"); }
        return false;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        if (products != null)
        {

            return products.Select(x => new ProductDto
            {
                CategoryName = x.Category.CategoryName,
                ArticleNumber = x.ArticleNumber,
                ProductTitle = x.ProductInfoEntity.ProductTitle,
                Ingress = x.ProductInfoEntity.Ingress,
                Description = x.ProductInfoEntity.Description,
                Specification = x.ProductInfoEntity.Specification,
                Manufacture = x.Manufacture.ManufactureName,
                Price = x.ProductPriceEntity.Price,
            });
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
                CategoryName = product.Category.CategoryName,
                ArticleNumber = product.ArticleNumber,
                ProductTitle = product.ProductInfoEntity.ProductTitle,
                Ingress = product.ProductInfoEntity.Ingress,
                Description = product.ProductInfoEntity.Description,
                Specification = product.ProductInfoEntity.Specification,
                Manufacture = product.Manufacture.ManufactureName,
                Price = product.ProductPriceEntity.Price,
            };
            return productDto;
        }
        return null!;
    }

    public async Task<IServiceResult> UpdateProductAsync(ProductDto productDto)
    {
        try
        {
            var manufactureId = await GetOrCreateManufactureAsync(productDto.Manufacture);
            if(manufactureId == 0)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var categoryId = await GetOrCreateCategoryAsync(productDto.CategoryName);
            if(categoryId == 0)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var updatedProductEntity = await UpdateProductEntityAsync(productDto.ArticleNumber, manufactureId, categoryId);
            if (!updatedProductEntity)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var updatedProductInfoEntity = await UpdateProductInfoEntityAsync(productDto.ArticleNumber, productDto.ProductTitle, productDto.Ingress, productDto.Description, productDto.Specification);
            if (!updatedProductInfoEntity)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }

            var updatedProductPriceEntity = await UpdateProductPriceEntityAsync(productDto.ArticleNumber, productDto.Price);
            if (!updatedProductPriceEntity)
            {
                return new ServiceResult { Status = ResultStatus.Failed };
            }
            else
            {
                UpdateProductList?.Invoke(this, EventArgs.Empty);
                return new ServiceResult { Status = ResultStatus.Updated };
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductAsync"); return new ServiceResult { Status = ResultStatus.Failed }; }
    }

    public async Task<bool> UpdateProductInfoEntityAsync(string articleNumber, string productTitle, string ingress, string description, string specification)
    {
        try
        {
            if(articleNumber != null && productTitle != null && ingress != null && description != null && specification != null)
            {
                var newProductInfoEntity = await _productInfoRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, new ProductInfoEntity
                {
                    ArticleNumber = articleNumber,
                    ProductTitle = productTitle,
                    Ingress = ingress,
                    Description = description,
                    Specification = specification

                });
                return newProductInfoEntity != null;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductInfoEntityAsync"); }
        return false;
    }

    public async Task<bool> UpdateProductEntityAsync(string articleNumber, int manufactureId, int categoryId)
    {
        try
        {
            if(articleNumber != null && manufactureId != 0 && categoryId != 0)
            {
                var newProductEntity = await _productRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, new ProductEntity
                {
                    ArticleNumber = articleNumber,
                    Modified = DateTime.Now,
                    ManufactureId = manufactureId,
                    CategoryId = categoryId,
                });
                return newProductEntity != null;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductEntityAsync"); }
        return false;
    }

    public async Task<bool> UpdateProductPriceEntityAsync(string articleNumber, decimal price)
    {
        try
        {
            if(articleNumber != null && price != 0)
            {
                var newProductPriceEntity = await _productPriceRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, new ProductPriceEntity
                {
                    ArticleNumber = articleNumber,
                    Price = price
                });
                return newProductPriceEntity != null;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductPriceEntityAsync"); }
        return false;
    }

    public async Task<IServiceResult> DeleteProductByArticleNumberAsync(string articleNumber)
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