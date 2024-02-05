using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class AddressRepository_Tests
{
    private readonly UserDataContext _userDataContext = new UserDataContext(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

   private readonly IErrorLogger _errorLogger = new ErrorLogger($"{Guid.NewGuid()}");


    [Fact]
    public async Task CheckIfAddressExist_Should_ReturnTrueIfExsist()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = new AddressEntity
        {
            StreetName = "Gata",
            PostalCode = "12345",
            City = "Ort",
        };
        var createdAddress = await _addressRepository.CreateAsync(addressEntity);
       
        // Act
        var result = await _addressRepository.ExistsAsync(x => x.Id == createdAddress.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateAddressEntity_ShouldSaveEntityToDatabase_ReturnAddressEntity()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = new AddressEntity
        {
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        };

        // Act
        var result = await _addressRepository.CreateAsync(addressEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Skara", result.StreetName);
    }

    [Fact]
    public async Task CreateAddressEntity_ShouldNotSaveEntityToDatabase_ReturnNull()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = new AddressEntity {};

        // Act
        var result = await _addressRepository.CreateAsync(addressEntity);

        // Assert
        Assert.Null(result);    
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeAddressEntity()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        await _addressRepository.CreateAsync(new AddressEntity
        {
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        });

        // Act
        var result = await _addressRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneAddressEntity_ReturnOneAddressEntity()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = await _addressRepository.CreateAsync(new AddressEntity
        {
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        });

        // Act
        var result = await _addressRepository.GetOneAsync(x => x.Id == addressEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AddressEntity>(result);
        Assert.Equal(addressEntity.Id, result.Id);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotFindAddressEntityFromDatabase_ReturnNull()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = new AddressEntity
        {
            Id = 1,
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        };

        // Act
        var result = await _addressRepository.GetOneAsync(x => x.Id == addressEntity.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveOneAddress_ReturnTure()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = await _addressRepository.CreateAsync(new AddressEntity
        {
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        });

        // Act
        var result = await _addressRepository.DeleteAsync(x => x.Id == addressEntity.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_NotRemoveOneAddress_ReturnFalse()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = await _addressRepository.CreateAsync(new AddressEntity
        {
            Id = 1,
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        });

        // Act
        var result = await _addressRepository.DeleteAsync(x => x.Id == 2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateAddressentity_ReturnUpdatedAddressEntity()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var addressEntity = await _addressRepository.CreateAsync(new AddressEntity
        {
            Id = 1,
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        });

        // Act
        addressEntity.StreetName = "Annat";
        var result = await _addressRepository.UpdateAsync(x => x.Id == addressEntity.Id, addressEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Annat", result.StreetName);
    }

    [Fact]
    public async Task UpdateAsync_Should_NotUpdateAddressentity_IfNoNewAddressExists_ReturnNull()
    {
        // Arrange
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);
        var nonExistsingAddressId = 999;
        var addressEntityToUpdate = new AddressEntity
        {
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        };

        // Act
        var result = await _addressRepository.UpdateAsync(x => x.Id == nonExistsingAddressId, addressEntityToUpdate);

        // Assert
        Assert.Null(result);
    }
}