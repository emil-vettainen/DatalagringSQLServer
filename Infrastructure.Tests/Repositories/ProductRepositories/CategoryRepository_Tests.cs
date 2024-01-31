using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class CategoryRepository_Tests
{
    private readonly ProductDataContext _productDataContext = new(new DbContextOptionsBuilder<ProductDataContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
       .Options);
    private readonly IErrorLogger errorLogger = new ErrorLogger($"{Guid.NewGuid()}");


    [Fact]
    public async Task CheckIfCategoryExist_Should_ReturnTrueIfExsist()
    {
        // Arrange
        var categoryRepository = new CategoryRepository( _productDataContext, errorLogger );
        var createdCategory = await categoryRepository.CreateAsync(new CategoryEntity { CategoryName = "Test" });

        // Act
        var result = await categoryRepository.ExistsAsync(x => x.CategoryName == createdCategory.CategoryName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateCategoryEntityAsync_ShouldSaveEntityToDatabase_ReturnCategoryEntity()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);

        // Act
        var result = await categoryRepository.CreateAsync(new CategoryEntity { CategoryName = "Test" });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.CategoryName);
    }

    [Fact]
    public async Task CreateCategoryEntityAsync_ShouldNotSaveEntityToDatabase_ReturnNull()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);

        // Act
        var result = await categoryRepository.CreateAsync(new CategoryEntity {});

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeCategoryEntity()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        await categoryRepository.CreateAsync(new CategoryEntity { CategoryName= "Test" });  

        // Act
        var result = await categoryRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<CategoryEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneCategoryFromDatabase_ReturnOneCategoryEntity()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        await categoryRepository.CreateAsync(new CategoryEntity { CategoryName = "Test" });

        // Act
        var result = await categoryRepository.GetOneAsync(x => x.CategoryName ==  "Test");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CategoryEntity>(result);
        Assert.Equal("Test", result.CategoryName);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotFindOneCategoryFromDatabase_ReturnNull()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        var categoryEntity = new CategoryEntity { CategoryName = "Test" };

        // Act
        var result = await categoryRepository.GetOneAsync(x => x.CategoryName == "Test");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveOneCategory_ReturnTrue()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        var categoryEntity = await categoryRepository.CreateAsync(new CategoryEntity { CategoryName = "Test" });

        // Act
        var result = await categoryRepository.DeleteAsync(x => x.CategoryName == categoryEntity.CategoryName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_NotRemoveOneCategory_ReturnFalse()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        var categoryEntity = new CategoryEntity { CategoryName= "Test" };

        // Act
        var result = await categoryRepository.DeleteAsync(x => x.CategoryName == categoryEntity.CategoryName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateCategory_ReturnUpdatedCategpryEntity()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        var categoryEntity = await categoryRepository.CreateAsync(new CategoryEntity { CategoryName = "Test" });

        // Act
        categoryEntity.CategoryName = "New";
        var result = await categoryRepository.UpdateAsync(x => x.Id == categoryEntity.Id, categoryEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryEntity.Id, result.Id);
        Assert.Equal("New", result.CategoryName);
    }

    [Fact]
    public async Task UpdateAsync_Should_NotUpdateCategory_IfNoNewCategory_ReturnNull()
    {
        // Arrange
        var categoryRepository = new CategoryRepository(_productDataContext, errorLogger);
        var nonExistingCategoryId = 999;
        var categoryEntityToUpdate = new CategoryEntity { CategoryName = "NewCategory" };

        // Act
        
        var result = await categoryRepository.UpdateAsync(x => x.Id == nonExistingCategoryId, categoryEntityToUpdate);

        // Assert
        Assert.Null(result);
    }
}