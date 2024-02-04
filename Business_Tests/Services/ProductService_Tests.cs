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

    [Fact]
    public async Task CreateProduct_ShouldNotCreateOneProductIfNoArticleNumber_ReturnFailed()
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
    }


    [Fact]
    public async Task UpdateProductAsync_ShouldNotUpdateAllDetailsOfProductIfNoArticleNumber_ReturnFailed()
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
            ArticleNumber = null!,
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
        Assert.Equal(ResultStatus.Failed, result.Status);
       
    }


}
