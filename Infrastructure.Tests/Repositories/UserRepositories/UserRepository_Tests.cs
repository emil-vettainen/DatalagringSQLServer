using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class UserRepository_Tests
{
    private readonly UserDataContext _userDataContext = new UserDataContext(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    private readonly IErrorLogger _errorLogger = new ErrorLogger($"{Guid.NewGuid()}");


    [Fact]
    public async Task CheckIfUserEntityExists_Should_ReturnTrue()
    {
        // Arragne
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            RoleId = 1,
            AddressId = 1,

        }); ;

        // Act
        var result = await _userRepository.ExistsAsync(x => x.Id == userEntity.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateUserEntity_ShouldSaveEntityToDatabase_ReturnUserEntity()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var userEntity = new UserEntity { Id = Guid.NewGuid(), RoleId = 1, AddressId = 1 };

        // Act
        var result = await _userRepository.CreateAsync(userEntity); ;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userEntity.Id, result.Id);
    }

    [Fact]
    public async Task CreateUserEntity_ShouldNotSaveEntityToDatabase_ReturnNull()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var emptyUserEntity = new UserEntity { };

        // Act
        var result = await _userRepository.CreateAsync(emptyUserEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GettAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeUserEntity()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            RoleId = 1,
            AddressId = 1,
        });

        // Act
        var result = await _userRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserEntity>>(result);
        Assert.Single(result);  
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneUserEntity_ReturnOneUserEntity()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var _roleRepository = new RoleRepository(_userDataContext, _errorLogger);
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);

        var role = await _roleRepository.CreateAsync(new RoleEntity { RoleName = "Admin" });
        var address = await _addressRepository.CreateAsync(new AddressEntity { StreetName = "skara", PostalCode = "12345", City = "Skara" });


        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            RoleId = role.Id,
            AddressId = address.Id,
        });

        // Act
        var result = await _userRepository.GetOneAsync(x => x.Id == userEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserEntity>(result);
        Assert.Equal("Admin", userEntity.Role.RoleName);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotGetOneUserEntityIfNotExists_ReturnNull()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var _roleRepository = new RoleRepository(_userDataContext, _errorLogger);
        var _addressRepository = new AddressRepository(_userDataContext, _errorLogger);

        var role = await _roleRepository.CreateAsync(new RoleEntity { RoleName = "Admin" });
        var address = await _addressRepository.CreateAsync(new AddressEntity { StreetName = "skara", PostalCode = "12345", City = "Skara" });

        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            RoleId = 1,
            AddressId = 1,
        });

        // Act
        var result = await _userRepository.GetOneAsync(x => x.Id == Guid.Empty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveOnUserEntity_ReturnTrue()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            AddressId = 1,
            RoleId = 1,
        });

        // Act
        var result = await _userRepository.DeleteAsync(x => x.Id == userEntity.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_NotRemoveIfNotFound_ReturnFalse()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            AddressId = 1,
            RoleId = 1,
        });

        // Act
        var result = await _userRepository.DeleteAsync(x => x.Id == Guid.Empty);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUserEntity_ShouldUpdateUserEntity_ReturnUpdatedEntity()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            AddressId = 1,
            RoleId = 1,
        });

        // Act
        userEntity.AddressId = 2;
        var result = await _userRepository.UpdateAsync(x => x.Id == userEntity.Id, userEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.AddressId);
    }

    [Fact]
    public async Task UpdateUserEntity_ShouldNotUpdateUserEntityIfNoEntityFound_ReturnNull()
    {
        // Arrange
        var _userRepository = new UserRepository(_userDataContext, _errorLogger);
        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            AddressId = 1,
            RoleId = 1,
        });

        // Act
        userEntity.AddressId = 2;
        var result = await _userRepository.UpdateAsync(x => x.Id == Guid.Empty, userEntity);

        // Assert
        Assert.Null(result);
    }

}