using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class UserRepository_Tests
{
    private readonly UserDataContext _userDataContext;
    private readonly UserRepository _userRepository;

    public UserRepository_Tests()
    {
        _userDataContext = new UserDataContext(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        var errorLogger = new ErrorLogger($"{Guid.NewGuid()}");

        _userRepository = new UserRepository(_userDataContext, errorLogger);
    }


    [Fact]
    public async Task CheckIfUserEntityExists_Should_ReturnTrue()
    {
        // Arragne
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
        




        var userEntity = await _userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            RoleId = 1,
            AddressId = 1,
        });

        // Act
        var result = await _userRepository.GetOneAsync(x => x.Id == userEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserEntity>(result);
        Assert.Equal(userEntity.Id, result.Id);
    }


}



