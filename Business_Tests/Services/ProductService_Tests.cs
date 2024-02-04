using Business.Dtos.ProductDtos;
using Business.Services.ProductServices;
using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Utilis;

namespace Business_Tests.Services;

public class ProductService_Tests
{
    private readonly ProductService _productService;
    private readonly ProductDataContext _productDataContext;

    public ProductService_Tests()
    {
        _productDataContext = new ProductDataContext(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        var errorLogger = new ErrorLogger($"{Guid.NewGuid()}");
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var productPriceRepository = new ProductPriceRepository(_productDataContext, errorLogger);
        var productInfoRepository = new ProductInfoRepository(_productDataContext, errorLogger);


        _productService = new ProductService(categoryRepository, productRepository, manufactureRepository, productPriceRepository, errorLogger, productInfoRepository);
    }


    [Fact]
    public async Task CreateProductAsync_ShouldCreateOneProductToDatabase_ReturnSuccessed()
    {
        // Arrange
        var product = new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 9999
        };

        // Act
        var result = await _productService.CreateProduktAsync(product);
        var createdProduct = await _productService.GetOneProductAsync("12345");

        // Assert
        Assert.Equal(ResultStatus.Successed, result.Status);
        Assert.Equal("Category", createdProduct.CategoryName);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldNotCreateOneProductIfNoArticleNumber_ReturnFailed()
    {
        // Arrange
        var product = new CreateProductDto
        {
            ArticleNumber = null!,
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 9999
        };

        // Act
        var result = await _productService.CreateProduktAsync(product);

        // Assert
        Assert.Equal(ResultStatus.Failed, result.Status);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldNotCreateOneProductIfAlreadyExists_ReturnAlreadyExists()
    {
        // Arrange
        await _productService.CreateProduktAsync(new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 9999
        });

        // Act
        var result = await _productService.CreateProduktAsync(new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Annat",
            ProductTitle = "Iphone",
            Ingress = "Grym",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 9999
        });

        // Assert
        Assert.Equal(ResultStatus.AlreadyExist, result.Status);
    }

    [Fact]
    public async Task GetAllProductDetails_ShouldGetAllDetailsOfProduct_ReturnIEnumerableOfProductEntity()
    {
        // Arrange
        var product = new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 9999
        };
        await _productService.CreateProduktAsync(product);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result);
    }

    [Fact]
    public async Task GetOrCreateCategoryEntity_ShouldGetOrCreate_ReturnCategoryId()
    {
        // Arrange
        var category = new CreateProductDto
        {
            CategoryName = "Category",
        };

        // Act
        var result = await _productService.GetOrCreateCategoryAsync(category.CategoryName);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetOrCreateCategoryEntity_ShouldNotGetOrCreateIfCategoryNameNull_Return0()
    {
        // Arrange
        var categoryEntity = new CreateProductDto {};    

        // Act
        var result = await _productService.GetOrCreateCategoryAsync(categoryEntity.CategoryName);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetOrCreateManufacuteEnrtity_ShouldGetOrCreate_ReturnManufactureId()
    {
        // Arrange
        var manufactureEntity = new ManufactureEntity { ManufactureName = "Apple" };

        // Act
        var result = await _productService.GetOrCreateManufactureAsync(manufactureEntity.ManufactureName);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetOrCreateManufacuteEnrtity_ShouldNotGetOrCreateIfManufactureIsNull_Return0()
    {
        // Arrange
        var manufactureEntity = new ManufactureEntity { };

        // Act
        var result = await _productService.GetOrCreateManufactureAsync(manufactureEntity.ManufactureName);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CreateProductEntity_ShouldCreateProductEntity_ReturnTrue()
    {
        // Arrange

        // Act
        var result = await _productService.CreateProductEntityAsync("12345", 1, 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProductEntity_ShouldNotCreateProductEntity_ReturnFalse()
    {
        // Arrange

        // Act
        var result = await _productService.CreateProductEntityAsync("12345", 1, 0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateProductInfoEntity_ShouldCreateEntity_ReturnTrue()
    {
        // Arrange

        // Act
        var result = await _productService.CreateProductInfoEntityAsync
            (
                "12345", "Title", "Ingress", "Descripton", "Specification"
            );

        // Assert
        Assert.True(result);
    }
    [Fact]
    public async Task CreateProductInfoEntity_ShouldNotCreateEntity_ReturnFalse()
    {
        // Arrange

        // Act
        var result = await _productService.CreateProductInfoEntityAsync
            (
                "12345", "Title", "Ingress", "Descripton", null!
            );

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateProductPriceEntity_ShouldCreateEntity_ReturnTrue()
    {
        // Arrange

        // Act
        var result = await _productService.CreateProductPriceEntityAsync("12345", 5000);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProductPriceEntity_ShouldNotCreateEntity_ReturnFalse()
    {
        // Arrange

        // Act
        var result = await _productService.CreateProductPriceEntityAsync("12345", 0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateAllDetailsOfProduct_ReturnUpdated()
    {
        // Arrange
        var product = await _productService.CreateProduktAsync(new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Spec",
            Manufacture = "Manufacture",
            Price = 999
        });

        // Act
        var result = await _productService.UpdateProductAsync(new ProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Annan",
            ProductTitle = "AnnanTitle",
            Ingress = "Ingress2",
            Description = "Description2",
            Specification = "Spec2",
            Manufacture = "Manufacture2",
            Price = 111
        });
        var updatedProduct = await _productService.GetOneProductAsync("12345");

        // Assert
        Assert.Equal(ResultStatus.Updated, result.Status);
        Assert.Equal("Annan", updatedProduct.CategoryName);
        Assert.Equal(111, updatedProduct.Price);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldNotUpdateAllDetailsOfProductIfRequiredNull_ReturnFailed()
    {
        // Arrange
        await _productService.CreateProduktAsync(new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Spec",
            Manufacture = "Manufacture",
            Price = 999
        });

        // Act
        var result = await _productService.UpdateProductAsync(new ProductDto
        {
            ArticleNumber = null!,
            CategoryName = "Annan",
            ProductTitle = "AnnanTitle",
            Ingress = "Ingress2",
            Description = "Description2",
            Specification = "Spec2",
            Manufacture = "Manufacture2",
            Price = 111
        });

        // Assert
        Assert.Equal(ResultStatus.Failed, result.Status);
    }

    [Fact]
    public async Task UpdateProductInfoEntity_ShouldUpdateEntity_ReturnTrue()
    {
        // Arrange
        await _productService.CreateProductInfoEntityAsync("12345", "Title", "Ingress", "Descripton", "Specification");

        // Act
        var updatetEntity = await _productService.UpdateProductInfoEntityAsync("12345", "annantitle", "annaningress", "annandesc", "annanspec");

        // Assert
        Assert.True(updatetEntity);
    }

    [Fact]
    public async Task UpdateProductInfoEntity_ShouldNotUpdateEntity_ReturnFalse()
    {
        // Arrange
        await _productService.CreateProductInfoEntityAsync("12345", "Title", "Ingress", "Descripton", "Specification");

        // Act
        var updatetEntity = await _productService.UpdateProductInfoEntityAsync("12345", null!, "annaningress", "annandesc", "annanspec");

        // Assert
        Assert.False(updatetEntity);
    }

    [Fact]
    public async Task UpdateProductEntity_ShouldUpdateEntity_ReturnTrue()
    {
        // Arrange
        await _productService.CreateProductEntityAsync("12345", 1, 1);

        // Act
        var updatedEntity = await _productService.UpdateProductEntityAsync("12345", 2, 2);

        // Assert
        Assert.True(updatedEntity);
    }

    [Fact]
    public async Task UpdateProductEntity_ShouldNotUpdateEntity_ReturnFalse()
    {
        // Arrange
        await _productService.CreateProductEntityAsync("12345", 1, 1);

        // Act
        var updatedEntity = await _productService.UpdateProductEntityAsync("12345", 0, 0);

        // Assert
        Assert.False(updatedEntity);
    }

    [Fact]
    public async Task UpdateProductPriceEntity_ShouldUpdateEntity_ReturnTrue()
    {
        // Arrange
        await _productService.CreateProductPriceEntityAsync("12345", 999);

        // Act
        var updatedEntity = await _productService.UpdateProductPriceEntityAsync("12345", 111);

        //Assert
        Assert.True(updatedEntity);
    }

    [Fact]
    public async Task UpdateProductPriceEntity_ShouldNotUpdateEntity_ReturnFalse()
    {
        // Arrange
        await _productService.CreateProductPriceEntityAsync("12345", 999);

        // Act
        var updatedEntity = await _productService.UpdateProductPriceEntityAsync("12345", 0);

        //Assert
        Assert.False(updatedEntity);
    }

    [Fact]
    public async Task DeleteOneProduct_ShouldDeleteOnProductByArticlenumber_ReturnDeleted()
    {
        // Arrange
        await _productService.CreateProduktAsync(new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 99999
        });
        // Act
        var result = await _productService.DeleteProductByArticleNumberAsync("12345");

        // Assert
        Assert.Equal(ResultStatus.Deleted, result.Status);
    }

    [Fact]
    public async Task DeleteOneProduct_ShouldNotDeleteIfNotFound_ReturnNotFound()
    {
        // Arrange
        await _productService.CreateProduktAsync(new CreateProductDto
        {
            ArticleNumber = "12345",
            CategoryName = "Category",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Manufacture = "Manufacture",
            Price = 99999
        });
        // Act
        var result = await _productService.DeleteProductByArticleNumberAsync("123456");

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

}