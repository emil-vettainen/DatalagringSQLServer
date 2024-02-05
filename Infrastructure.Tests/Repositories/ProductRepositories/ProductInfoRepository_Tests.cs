using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;
using System.Runtime.InteropServices;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class ProductInfoRepository_Tests
{
    private readonly ProductDataContext _productDataContext = new(new DbContextOptionsBuilder<ProductDataContext>()
      .UseInMemoryDatabase($"{Guid.NewGuid()}")
      .Options);
    private readonly IErrorLogger _errorLogger = new ErrorLogger($"{Guid.NewGuid()}");



    [Fact]
    public async Task CheckIfPInfoEntityExists_Should_ReturnTrueIfExists()
    {
        // Arrange
        var _productInfoRepo = new ProductInfoRepository(_productDataContext, _errorLogger);
        await _productInfoRepo.CreateAsync(new ProductInfoEntity
        {
            ArticleNumber = "12345",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specifiaciton"
        });
        // Act
        var result = await _productInfoRepo.ExistsAsync(x => x.ArticleNumber == "12345");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProductInfo_ShouldCreateProductInfoEntity_ReturnProductInfoEntity()
    {
        // Arrange
        var _productInfoRepo = new ProductInfoRepository(_productDataContext, _errorLogger);
        var productInfoEntity = new ProductInfoEntity
        {
            ArticleNumber = "12345",
            ProductTitle = "Title",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specifiaciton"
        };
        // Act
        var result = await _productInfoRepo.CreateAsync(productInfoEntity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProductInfoEntity>(result);   
    }

    [Fact]
    public async Task CreateProductInfo_ShouldNotCreateIfEntityIsNull_ReturnNull()
    {
        // Arrange
        var _productInfoRepo = new ProductInfoRepository(_productDataContext, _errorLogger);
        var productInfoEntity = new ProductInfoEntity
        {
            ArticleNumber = "12345",
            ProductTitle = null!,
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specifiaciton"
        };
        // Act
        var result = await _productInfoRepo.CreateAsync(productInfoEntity);

        // Assert
        Assert.Null(result);
    }
}
