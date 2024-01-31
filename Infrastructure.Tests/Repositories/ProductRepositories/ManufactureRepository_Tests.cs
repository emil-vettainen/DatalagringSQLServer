using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.ProductRepositories;

public class ManufactureRepository_Tests
{
    private readonly ProductDataContext _productDataContext = new(new DbContextOptionsBuilder<ProductDataContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
       .Options);
    private readonly IErrorLogger errorLogger = new ErrorLogger($"{Guid.NewGuid()}");



    [Fact]
    public async Task CheckIfManufactureExist_Should_ReturnTrueIfExsist()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var createdManufacture = await manufactureRepository.CreateAsync(new ManufactureEntity{ManufactureName = "Test"});

        // Act
        var result = await manufactureRepository.ExistsAsync(x => x.ManufactureName == createdManufacture.ManufactureName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateManufactureEntityAsync_ShouldSaveEntityToDatabase_ReturnManufactureEntity()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
       
        // Act
        var result = await manufactureRepository.CreateAsync(new ManufactureEntity { ManufactureName = "Test" });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.ManufactureName);
    }

    [Fact]
    public async Task CreateManufactureEntityAsync_ShouldNotSaveEntityToDatabase_ReturnNull()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);

        // Act
        var result = await manufactureRepository.CreateAsync(new ManufactureEntity {});

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeManufactureEntity()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        await manufactureRepository.CreateAsync(new ManufactureEntity { ManufactureName = "Test" });

        // Act
        var result = await manufactureRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ManufactureEntity>>(result);
        Assert.Single(result);  
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneManufacture_ReturnOneManufactureEntity()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        await manufactureRepository.CreateAsync(new ManufactureEntity { ManufactureName = "Test" });

        // Act
        var result = await manufactureRepository.GetOneAsync(x => x.ManufactureName == "Test");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ManufactureEntity>(result);   
        Assert.Equal("Test", result.ManufactureName);   
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotFindOneManufactureFromDatabase_ReturnNull()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var manufactureEntity = new ManufactureEntity { ManufactureName = "Test" };

        // Act
        var result = await manufactureRepository.GetOneAsync(x => x.ManufactureName == "Test");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveOneManufacture_ReturnTure()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var manufactureEntity = await manufactureRepository.CreateAsync(new ManufactureEntity { ManufactureName = "Test" });

        // Act
        var result = await manufactureRepository.DeleteAsync(x => x.Id == manufactureEntity.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_NotRemoveOneManufacture_ReturnFalse()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var manufactureEntity = new ManufactureEntity { ManufactureName= "Test" };

        // Act
        var result = await manufactureRepository.DeleteAsync(x => x.ManufactureName == "Test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateManufacture_ReturnUpdatedManufactureEntity()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var manufactureEntity = await manufactureRepository.CreateAsync(new ManufactureEntity { ManufactureName = "Test" });

        // Act
        manufactureEntity.ManufactureName = "New";
        var result = await manufactureRepository.UpdateAsync(x => x.Id == manufactureEntity.Id, manufactureEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(manufactureEntity.Id, result.Id);
        Assert.Equal("New", result.ManufactureName);
    }

    [Fact]
    public async Task UpdateAsync_Should_NotUpdateManufacture_ReturnNull()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productDataContext, errorLogger);
        var nonExistingManufactureId = 999;
        var manufactureEntity = new ManufactureEntity { ManufactureName = "Test" };

        // Act
        var result = await manufactureRepository.UpdateAsync(x => x.Id == nonExistingManufactureId, manufactureEntity);

        // Assert
        Assert.Null(result);    
    }
}