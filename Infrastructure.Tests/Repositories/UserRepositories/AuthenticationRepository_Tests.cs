using Business.Services.UserServices;
using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class AuthenticationRepository_Tests
{
    private readonly UserDataContext _userDataContext = new UserDataContext(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

    private readonly IErrorLogger _errorLogger = new ErrorLogger($"{Guid.NewGuid()}");

   

    [Fact]
    public async Task CheckIfAuthEntityExists_ShouldReturnTrueIfExists()
    {
        // Arrange
        var userRepo = new UserRepository( _userDataContext, _errorLogger );
        var roleRepo = new RoleRepository( _userDataContext, _errorLogger );
        var profileRepo = new ProfileRepository( _userDataContext, _errorLogger );
        var addressRepo = new AddressRepository( _userDataContext, _errorLogger );
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);

        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        var result = await _authenticationRepository.ExistsAsync(x => x.UserId == authEntity.UserId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateAuthEntity_ShouldCreateEntity_ReturnAuthEntity()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);

        // Act
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Assert
        Assert.NotNull(authEntity);
        Assert.Equal("Email@domain.com", authEntity.Email);
        Assert.IsType<AuthenticationEntity>(authEntity);
    }

    [Fact]
    public async Task CreateAuthEntity_ShouldNotCreateEntityIfEntityIsNull_ReturnNull()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);

        // Act
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = null!,
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Assert
        Assert.Null(authEntity);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumberableOfTypeAuthEntity()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        var result = await _authenticationRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<AuthenticationEntity>>(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneAuthEntity_ReturnOneTypeOfAuthEntity()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        var result = await _authenticationRepository.GetOneAsync(x => x.UserId == authEntity.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<AuthenticationEntity>(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotGetOneAuthEntityIfNotFound_ReturnNull()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        var result = await _authenticationRepository.GetOneAsync(x => x.UserId == Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity_ReturnTrue()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        var result = await _authenticationRepository.DeleteAsync(x => x.UserId == authEntity.UserId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotDeleteEntityIfNotFound_ReturnFalse()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        var result = await _authenticationRepository.DeleteAsync(x => x.UserId == Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmail_ReturnUpdatedEntity()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        authEntity.Email = "Annan@domain.com";
        var result = await _authenticationRepository.UpdateAsync(x => x.UserId == authEntity.UserId, authEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Annan@domain.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateIfEntityNotExists_ReturnNull()
    {
        // Arrange
        var userRepo = new UserRepository(_userDataContext, _errorLogger);
        var roleRepo = new RoleRepository(_userDataContext, _errorLogger);
        var profileRepo = new ProfileRepository(_userDataContext, _errorLogger);
        var addressRepo = new AddressRepository(_userDataContext, _errorLogger);
        var _authenticationRepository = new AuthenticationRepository(_userDataContext, _errorLogger);
        var userService = new UserService(userRepo, roleRepo, _authenticationRepository, profileRepo, addressRepo, _errorLogger);

        var password = "Bytmig123!";
        userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
        var authEntity = await _authenticationRepository.CreateAsync(new AuthenticationEntity
        {
            UserId = Guid.NewGuid(),
            Email = "Email@domain.com",
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
        });

        // Act
        authEntity.Email = "Annan@domain.com";
        var result = await _authenticationRepository.UpdateAsync(x => x.UserId == Guid.NewGuid(), authEntity);

        // Assert
        Assert.Null(result);
    }

}