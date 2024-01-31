using Business.Dtos;
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

   
    private readonly IErrorLogger _errorLogger = errorLogger;
    private readonly IServiceResult _result = new ServiceResult();

    public EventHandler? UpdateProductList;




    public async Task<bool> CreateProdukt(CreateProductDto createProductDto)
    {
        if (await _productRepository.ExistsAsync(x => x.ArticleNumber == createProductDto.ArticleNumber))
        {
            return false;
        }

        var manufactureId = await GetOrCreateManufactureAsync(createProductDto.Manufacture);


        var subCategory = await GetOrCreateCategoryAsync(createProductDto.SubCategoryName, createProductDto.CategoryName);


        var productEntity = new ProductEntity
        {
            
            ArticleNumber = createProductDto.ArticleNumber,
            ProductTitle = createProductDto.ProductTitle,
            Ingress = createProductDto.Ingress,
            Description = createProductDto.Description,
            Specification = createProductDto.Specification,
            ManufactureId = manufactureId,

        };

        //productEntity.Categories.Add(subCategory);
        //productEntity.Categories.Add(mainCategory);

        var createdProduct = await _productRepository.CreateAsync(productEntity);


    
        await _productPriceRepository.CreateAsync(new ProductPriceEntity
        {
            ArticleNumber = createdProduct.ArticleNumber,
            Price = createProductDto.Price,
        });



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
                var mainCategory = x.Categories.FirstOrDefault(x => x.ParentCategoryId == null)?.CategoryName!;
                var subCategory = x.Categories.FirstOrDefault(x => x.ParentCategoryId != null)?.CategoryName!;

                return new ProductDto
                {
                    CategoryName = mainCategory,
                    SubCategoryName = subCategory,
                    ArticleNumber = x.ArticleNumber,
                    ProductTitle = x.ProductTitle,
                    Ingess = x.Ingress,
                    Description = x.Description,
                    Specification = x.Specification,
                    Manufacture = x.Manufacture.Manufacturers,
                    Price = x.ProductPriceEntity?.Price ?? 0,
                    
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
                CategoryName = product.Categories.FirstOrDefault(x => x.ParentCategoryId == null)?.CategoryName!,
                SubCategoryName = product.Categories.FirstOrDefault(x => x.ParentCategoryId != null)?.CategoryName!,
                ArticleNumber = product.ArticleNumber,
                ProductTitle = product.ProductTitle,
                Ingess = product.Ingress,
                Specification = product.Specification,
                Description = product.Description,
                Manufacture = product.Manufacture.Manufacturers,
                Price = product.ProductPriceEntity?.Price ?? 0,
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
            var mainCategory = await GetOrCreateCategoryAsync(productDto.CategoryName);
            //var subCategory = await GetOrCreateCategoryAsync(productDto.SubCategoryName, mainCategory.Id);

            var product = await _productRepository.GetOneAsync(x => x.ArticleNumber == productDto.ArticleNumber);
            if (product != null)
            {
                product.ProductTitle = productDto.ProductTitle;
                product.Ingress = productDto.Ingess;
                product.Description = productDto.Description;
                product.Specification = productDto.Specification;
                product.ManufactureId = manufactureId;
            }

            //product.Categories.Add(subCategory);
            //product.Categories.Add(mainCategory);

            await _productRepository.UpdateAsync(x => x.ArticleNumber == productDto.ArticleNumber, product);


            await UpdateProductPriceEntityAsync(productDto.ArticleNumber, productDto.Price);
            UpdateProductList?.Invoke(this, EventArgs.Empty);
            _result.Status = ResultStatus.Updated;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - UpdateUserAsync"); _result.Status = ResultStatus.Failed; }
        return _result;
    }


    private async Task UpdateProductPriceEntityAsync(string articleNumber, decimal price)
    {
        var productPrice = await _productPriceRepository.GetOneAsync(x => x.ArticleNumber == articleNumber);
        if (productPrice != null)
        {
            productPrice.ArticleNumber = articleNumber;
            productPrice.Price = price;

            await _productPriceRepository.UpdateAsync(x => x.ArticleNumber == articleNumber, productPrice);

        }
    }

    //private async Task UpdateProfileEntityAsync(Guid userId, string firstName, string lastName)
    //{
    //    var profile = await _profileRepository.GetOneAsync(x => x.UserId == userId);
    //    if (profile != null)
    //    {
    //        profile.FirstName = firstName;
    //        profile.LastName = lastName;

    //        await _profileRepository.UpdateAsync(x => x.UserId == userId, profile);
    //    }
    //}

    //private async Task UpdateAuthEntityAsync(Guid userId, string email, string password)
    //{


    //    var auth = await _authenticationRepository.GetOneAsync(x => x.UserId == userId);
    //    if (auth != null)
    //    {
    //        auth.Email = email;
    //        auth.Password = password;

    //        await _authenticationRepository.UpdateAsync(x => x.UserId == userId, auth);
    //    }
    //}





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

    //private async Task<CategoryEntity> GetOrCreateCategoryAsync(string categoryName)
    //{
    //    try
    //    {
    //        var categoryExists = await _categoryRepository.ExistsAsync(x => x.CategoryName == categoryName);
    //        if (categoryExists)
    //        {
    //            var existingCategory = await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName);
    //            return existingCategory;
    //        }
    //        else
    //        {
    //            var categoryEntity = new CategoryEntity { CategoryName = categoryName};
    //            var createdCategory = await _categoryRepository.CreateAsync(categoryEntity);
    //            return createdCategory;
    //        }
    //    }
    //    catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateCategoryAsync"); }
    //    return null!;
    //}


    private async Task<CategoryEntity> GetOrCreateCategoryAsync(string subCategoryName, string categoryName = "")
    {
        try
        {
            var subCategoryEntity = await _categoryRepository.GetOneAsync(x => x.CategoryName == subCategoryName);
            if (subCategoryEntity == null) 
            {
                if (!string.IsNullOrEmpty(categoryName))
                {
                    var categoryEntity = await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName);
                    categoryEntity ??= await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = categoryName });

                    subCategoryEntity = await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = subCategoryName, ParentCategoryId = categoryEntity.Id });
                }
                else
                {
                    subCategoryEntity ??= await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = subCategoryName });
                }
            }

            return subCategoryEntity!;






          
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateCategoryAsync"); }
        return null!;
    }



    //private async Task<CategoryEntity> GetOrCreateCategoryAsync(string categoryName, int? parentCategoryId = null)
    //{
    //    try
    //    {
    //        // Laptop (1    Laptop      null )

    //        var categoryEntity = await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName);
    //        categoryEntity ??= await _categoryRepository.CreateAsync(new CategoryEntity { CategoryName = categoryName });





    //        var categoryExists = await _categoryRepository.ExistsAsync(x => x.CategoryName == categoryName && x.ParentCategoryId == parentCategoryId);
    //        if (categoryExists)
    //        {
    //            var existingCategory = await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName && x.ParentCategoryId == parentCategoryId);
    //            return existingCategory;
    //        }
    //        else
    //        {
    //            var categoryEntity = new CategoryEntity { CategoryName = categoryName, ParentCategoryId = parentCategoryId };
    //            var createdCategory = await _categoryRepository.CreateAsync(categoryEntity);
    //            return createdCategory;
    //        }
    //    }
    //    catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateCategoryAsync"); }
    //    return null!;
    //}


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
                var manufactureEntity = new ManufactureEntity { Manufacturers = manufactureName };
                var createdManufacture = await _manufacturerRepository.CreateAsync(manufactureEntity);
                return manufactureEntity.Id;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateAddressAsync"); }
        return 0;
    }

}
