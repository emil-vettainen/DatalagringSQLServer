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


    //[Fact]
    //public async Task CheckIfProductExist_Should_ReturnTrueIfExsist()
    //{
    //    // Arrange
    //    var productRepository = new ProductRepository(_productDataContext, errorLogger);



    //    var productEntity = new ProductEntity
    //    {
    //        ArticleNumber = "12345",
    //        ManufactureId = 1,
    //        CategoryId = 1,

    //    };
    //    await productRepository.CreateAsync(productEntity);

    //    // Act
    //    var result = await productRepository.ExistsAsync(x => x.ArticleNumber == productEntity.ArticleNumber);

    //    // Assert
    //    Assert.True(result);
    //}

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
}
