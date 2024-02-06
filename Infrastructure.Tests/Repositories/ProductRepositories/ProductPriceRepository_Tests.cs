using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class ProductPriceRepository_Tests
{
    private readonly ProductDataContext _productDataContext = new(new DbContextOptionsBuilder<ProductDataContext>()
      .UseInMemoryDatabase($"{Guid.NewGuid()}")
      .Options);
    private readonly IErrorLogger _errorLogger = new ErrorLogger($"{Guid.NewGuid()}");




    [Fact]
    public async Task CheckIfProductPriceEntityExist_Should_ReturnTrueIfExsist()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        var result = await _productPriceRepository.ExistsAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProductPriceEntity_ShouldSaveEntityToDatabase_ReturnProductPriceEntity()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);

        // Act
        var result = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(99, result.Price);
    }

    [Fact]
    public async Task CreateProductPriceEntity_ShouldNotSaveIfEntityIsNull_ReturnNull()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);

        // Act
        var result = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 0 });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumberableOfTypProductPriceEntity()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 999 });

        // Act
        var result = await _productPriceRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductPriceEntity>>(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneEntity_ReturnOneOfTypProductPriceEntity()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        var price = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        var result = await _productPriceRepository.GetOneAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProductPriceEntity>(result);  
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotGetOneEntityIfEntityNotFound_ReturnNull()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        var price = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        var result = await _productPriceRepository.GetOneAsync(x => x.ArticleNumber == "12345678");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteOnProductPriceEntity_ReturnTrue()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        var price = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        var result = await _productPriceRepository.DeleteAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotDeleteIfNotExists_ReturnFalse()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        var price = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        var result = await _productPriceRepository.DeleteAsync(x => x.ArticleNumber == "1234567");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductPriceEntity_ReturnUpdatedEntity()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        var price = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        price.Price = 1111;
        var result = await _productPriceRepository.UpdateAsync(x => x.ArticleNumber == "12345", price);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1111, result.Price);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateProductPriceEntityIfNotExists_ReturnNull()
    {
        // Arrange
        var _productPriceRepository = new ProductPriceRepository(_productDataContext, _errorLogger);
        var price = await _productPriceRepository.CreateAsync(new ProductPriceEntity { ArticleNumber = "12345", Price = 99 });

        // Act
        price.Price = 1111;
        var result = await _productPriceRepository.UpdateAsync(x => x.ArticleNumber == "1234567", price);

        // Assert
        Assert.Null(result);
    }

}