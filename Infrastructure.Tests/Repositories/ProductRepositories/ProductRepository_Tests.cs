using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class ProductRepository_Tests
{
    private readonly ProductDataContext _productDataContext = new(new DbContextOptionsBuilder<ProductDataContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
       .Options);

    private readonly IErrorLogger errorLogger = new ErrorLogger($"{Guid.NewGuid()}");


    [Fact]
    public async Task CheckIfProductExist_Should_ReturnTrueIfExsist()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
       
        var productEntity = await productRepository.CreateAsync(new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        });

        // Act
        var result = await productRepository.ExistsAsync(x => x.ArticleNumber == productEntity.ArticleNumber);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProductEntityAsync_ShouldSaveEntityToDatabase_ReturnProductEntity()
    {
        // Arrange
        var productRepository = new ProductRepository( _productDataContext, errorLogger );
     
        // Act
        var result = await productRepository.CreateAsync(new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        });

        // Assert
        Assert.NotNull(result);
        Assert.Equal( "12345", result.ArticleNumber );
    }

    [Fact]
    public async Task CreateProductEntityAsync_ShouldNotSaveEntityToDatabase_ReturnNull()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);

        // Act
        var result = await productRepository.CreateAsync(new ProductEntity {});

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeProductEntity()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);

        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger );

        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger );

        var categoryId = await categoryRepository.CreateAsync(new CategoryEntity
        {
            CategoryName = "Test"
        });

        var manufactureId = await manufactureRepository.CreateAsync(new ManufactureEntity
        {
            ManufactureName = "Test"
        });

        
        await productRepository.CreateAsync(new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
           
        });

        // Act
        var result = await productRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneProduct_ReturnOneProductEntity()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        await productRepository.CreateAsync(new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        });

        // Act
        var result = await productRepository.GetOneAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProductEntity>(result);
        Assert.Equal("12345", result.ArticleNumber);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotFindOneProductFromDatabase_ReturnNull()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        var productEntity = new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        };

        // Act
        var result = await productRepository.GetOneAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveOneProduct_ReturnTrue()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        await productRepository.CreateAsync(new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        });

        // Act
        var result = await productRepository.DeleteAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_NotRemoveOneProduct_ReturnFalse()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        var productEntity = new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        };

        // Act
        var result = await productRepository.DeleteAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateProduct_ReturnUpdatedProductEntity()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        var productEntity = await productRepository.CreateAsync(new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        });

        // Act
        productEntity.ManufactureId = 2;
        productEntity.CategoryId = 2;
        var result = await productRepository.UpdateAsync(x => x.ArticleNumber == productEntity.ArticleNumber, productEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity.ArticleNumber, result.ArticleNumber);
        Assert.Equal(2, result.ManufactureId);
    }

    [Fact]
    public async Task UpdateAsync_Should_NotUpdateProduct_ReturnNull()
    {
        // Arrange
        var productRepository = new ProductRepository(_productDataContext, errorLogger);
        var nonExistsProduct = "224455";
        var productEntity = new ProductEntity
        {
            ArticleNumber = "12345",
            ManufactureId = 1,
            CategoryId = 1,
        };

        // Act
   
        var result = await productRepository.UpdateAsync(x => x.ArticleNumber == nonExistsProduct, productEntity);

        // Assert
        Assert.Null(result);
    }
}