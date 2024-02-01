﻿using Business.Dtos.ProductDtos;
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
            if (await _productRepository.ExistsAsync(x => x.ArticleNumber == dto.ArticleNumber))
            {
                _result.Status = ResultStatus.AlreadyExist;
            }

            var manufactureId = await GetOrCreateManufactureAsync(dto.Manufacture);
            var categoryId = await GetOrCreateCategoryAsync(dto.CategoryName);

            if (manufactureId == 0 || categoryId == 0)
            {
                _result.Status = ResultStatus.Failed;
            }

            await CreateProductEntityAsync(dto.ArticleNumber, manufactureId, categoryId);
            await CreateProductInfoEntityAsync(dto.ArticleNumber, dto.ProductTitle, dto.Ingress, dto.Description, dto.Specification);
            await CreateProductPriceEntityAsync(dto.ArticleNumber, dto.Price);

            UpdateProductList?.Invoke(this, EventArgs.Empty);
            _result.Status = ResultStatus.Successed;

        }
        catch (Exception ex){ _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProduktAsync");_result.Status = ResultStatus.Failed;}
        return _result;
    }

    private async Task<int> GetOrCreateManufactureAsync(string manufacture)
    {
        try
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - GetOrCreateManufactureAsync"); }
        return 0;
    }

    private async Task<int> GetOrCreateCategoryAsync(string categoryName)
    {
        try
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - GetOrCreateCategoryAsync"); }
        return 0;
    }

    private async Task CreateProductEntityAsync(string articleNumber, int manufactureId, int categoryId)
    {
        try
        {
            await _productRepository.CreateAsync(new ProductEntity
            {
                ArticleNumber = articleNumber,
                Created = DateTime.Now,
                ManufactureId = manufactureId,
                CategoryId = categoryId,
            });
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProductEntityAsync"); }
    }
    
    private async Task CreateProductInfoEntityAsync(string articleNumber, string productTitle, string ingress, string description, string specification)
    {
        try
        {
            await _productInfoRepository.CreateAsync(new ProductInfoEntity
            {
                ArticleNumber = articleNumber,
                ProductTitle = productTitle,
                Ingress = ingress,
                Description = description,
                Specification = specification,
            });
           
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProductInfoAsync"); }
    }

    private async Task CreateProductPriceEntityAsync(string articleNumber, decimal price)
    {
        try
        {
            await _productPriceRepository.CreateAsync(new ProductPriceEntity
            {
                ArticleNumber = articleNumber,
                Price = price,
            });
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - CreateProductPriceAsync"); }
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

            //var productDto = products.Select(x =>
            //{
            //    return new ProductDto
            //    {
            //        CategoryName = x.Category.CategoryName,
            //        ArticleNumber = x.ArticleNumber,
            //        ProductTitle = x.ProductInfoEntity.ProductTitle,
            //        Ingess = x.ProductInfoEntity.Ingress,
            //        Description = x.ProductInfoEntity.Description,
            //        Specification = x.ProductInfoEntity.Specification,
            //        Manufacture = x.Manufacture.ManufactureName,
            //        Price = x.ProductPriceEntity.Price,
            //    };
            //});
            //return productDto;
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
            var categoryId = await GetOrCreateCategoryAsync(productDto.CategoryName);

            if(manufactureId == 0 || categoryId == 0)
            {
                _result.Status = ResultStatus.Failed;
            }
            await UpdateProductEntityAsync(productDto.ArticleNumber, manufactureId, categoryId);
            await UpdateProductInfoEntityAsync(productDto.ArticleNumber, productDto.ProductTitle, productDto.Ingress, productDto.Description, productDto.Specification);
            await UpdateProductPriceEntityAsync(productDto.ArticleNumber, productDto.Price);

            UpdateProductList?.Invoke(this, EventArgs.Empty);
            _result.Status = ResultStatus.Updated;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductAsync"); _result.Status = ResultStatus.Failed; }
        return _result;
    }

    private async Task<bool> UpdateProductInfoEntityAsync(string articleNumber, string productTitle, string ingress, string description, string specification)
    {
        try
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductInfoEntityAsync"); }
        return false;
    }

    private async Task<bool> UpdateProductEntityAsync(string articleNumber, int manufactureId, int categoryId)
    {
        try
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "ProductService - UpdateProductEntityAsync"); }
        return false;
    }

    private async Task<bool> UpdateProductPriceEntityAsync(string articleNumber, decimal price)
    {
        try
        {
            var newProductPriceEntity = await _productPriceRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, new ProductPriceEntity
            {
                ArticleNumber = articleNumber,
                Price = price  
            });
            return newProductPriceEntity != null;
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