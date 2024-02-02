using Business.Dtos.ProductDtos;
using Business.Services.ProductServices;
using Infrastructure.Contexts;
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
    public async Task CreateProduct_ShouldCreateOneProductToDatabase_ReturnSuccessed()
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

        // Assert
        Assert.Equal(ResultStatus.Successed, result.Status);  
    }




}
