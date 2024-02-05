using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Utilis;
using System.Runtime.CompilerServices;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class ProfileRepository_Tests
{
    private readonly UserDataContext _userDataContext;
    private readonly ProfileRepository _profileRepository;

    public ProfileRepository_Tests()
    {
        _userDataContext = new UserDataContext(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        var errorLogger = new ErrorLogger($"{Guid.NewGuid()}");
        _profileRepository = new ProfileRepository(_userDataContext, errorLogger);
    }


    [Fact]
    public async Task CheckIfProfileEntityExist_Should_ReturnTrueIfExists()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        var result = await _profileRepository.ExistsAsync(x => x.UserId == profileEntity.UserId);

        // Assert
        Assert.True(result);   
    }

    [Fact]
    public async Task CreateProfileEntity_ShouldSaveToDatabase_ReturnProfileEntity()
    {
        // Arrange
        var profileEntity = new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        };

        // Act
        var result = await _profileRepository.CreateAsync(profileEntity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProfileEntity>(result);
    }

    [Fact]
    public async Task CreateProfileEntity_ShouldNotSaveToDatabaseIfEntityIsNull_ReturnNull()
    {
        // Arrange
        var profileEntity = new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = null!,
            LastName = null!,
        };

        // Act
        var result = await _profileRepository.CreateAsync(profileEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeProfileEntity()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        var result = await _profileRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProfileEntity>>(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneRecord_ReturnOneProfileEntity()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        var result = await _profileRepository.GetOneAsync(x => x.UserId == profileEntity.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProfileEntity>(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotGetOneIfNotExists_ReturnNull()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        var result = await _profileRepository.GetOneAsync(x => x.UserId == Guid.Empty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProfileEntity_ReturnTrue()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        var result = await _profileRepository.DeleteAsync(x => x.UserId == profileEntity.UserId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotDeleteProfileEntityIfNotFound_ReturnFalse()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        var result = await _profileRepository.DeleteAsync(x => x.UserId == Guid.Empty);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateProfileEntity_ShouldUpdateEntity_ReturnUpdatedEntity()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        profileEntity.FirstName = "Annat";
        var result = await _profileRepository.UpdateAsync(x => x.UserId == profileEntity.UserId, profileEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Annat", profileEntity.FirstName);
    }

    [Fact]
    public async Task UpdateProfileEntity_ShouldNotUpdateEntityIfNoEntityExists_ReturnNull()
    {
        // Arrange
        var profileEntity = await _profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "Emil",
            LastName = "Vettainen"
        });

        // Act
        profileEntity.FirstName = "Annat";
        var result = await _profileRepository.UpdateAsync(x => x.UserId == Guid.Empty, profileEntity);

        // Assert
        Assert.Null(result);
    }

}