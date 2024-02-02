using Business.Dtos;
using Business.Services.UserServices;
using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Utilis;

namespace Business_Tests.Services;

public class UserService_Tests
{
    private readonly UserService _userService;
    private readonly UserDataContext _userDataContext;

    public UserService_Tests()
    {
        _userDataContext = new UserDataContext (new DbContextOptionsBuilder<UserDataContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
       .Options);

        var errorLogger = new ErrorLogger($"{Guid.NewGuid()}");
        var userRepository = new UserRepository(_userDataContext, errorLogger);
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var authRepository = new AuthenticationRepository(_userDataContext, errorLogger);
        var profileRepository = new ProfileRepository(_userDataContext, errorLogger);
        var addressRepository = new AddressRepository(_userDataContext, errorLogger);
       
        _userService = new UserService(userRepository, roleRepository, authRepository, profileRepository, addressRepository, errorLogger);
    }



    [Fact]
    public async Task LoginAsync_ShouldValidate_ReturnSucess()
    {
        // Arrange
        await _userService.CreateUserAsync(new UserRegisterDto {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "Test@domain.com",
            Password = "password"
        });

        // Act
        var result = await _userService.LoginAsync(new UserLoginDto
        {
            Email = "Test@domain.com",
            Password = "password",
        });

        // Assert
        Assert.Equal(ResultStatus.Successed, result.Status);  
    }

    [Fact]
    public async Task LoginAsync_ShouldValidate_ReturnWrongpassword()
    {
        // Arrange
        await _userService.CreateUserAsync(new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "Test@domain.com",
            Password = "password"
        });

        // Act
        var result = await _userService.LoginAsync(new UserLoginDto
        {
            Email = "Test@domain.com",
            Password = "annatpassword",
        });

        // Assert
        Assert.Equal(ResultStatus.WrongPassword, result.Status);
    }

    [Fact]
    public async Task LoginAsync_ShouldValidate_ReturnNotFound()
    {
        // Arrange
        await _userService.CreateUserAsync(new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "Test@domain.com",
            Password = "password"
        });

        // Act
        var result = await _userService.LoginAsync(new UserLoginDto
        {
            Email = "annan@domain.com",
            Password = "annatpassword",
        });

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task CreateUser_ShouldSaveUserToDatabase_ReturnSucess()
    {
        // Arrange
        var userRegister = new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "emil@domain.com",
            Password = "Bytmig123!",
        };

        // Act
        var result = await _userService.CreateUserAsync(userRegister);

        // Assert
        Assert.Equal(ResultStatus.Successed, result.Status);      
    }

    [Fact]
    public async Task CreateUser_ShouldNotSaveUserToDatabase_ReturnFailed()
    {
        // Arrange
        var userRegister = new UserRegisterDto
        {
            RoleName = null!,
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "emil@domain.com",
            Password = "Bytmig123!",
        };

        // Act
        var result = await _userService.CreateUserAsync(userRegister);

        // Assert
        Assert.Equal(ResultStatus.Failed, result.Status);
    }

    [Fact]
    public async Task CreateUser_ShouldNotSaveUserToDatabase_ReturnAlreadyExists()
    {
        // Arrange
        await _userService.CreateUserAsync(new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "emil@domain.com",
            Password = "Bytmig123!",
        });

        // Act
        var result = await _userService.CreateUserAsync(new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Annan",
            LastName = "Annan",
            StreetName = "Annan",
            PostalCode = "Annan",
            City = "Annan",
            Email = "emil@domain.com",
            Password = "Bytmig123!",
        });

        // Assert
        Assert.Equal(ResultStatus.AlreadyExist, result.Status);
    }

    [Fact]
    public async Task GetOrCreateRoleAsync_ShouldGetOrCreateRole_ReturnRoleId()
    {
        // Arrange
        var roleName = new UserRegisterDto
        {
            RoleName = "User",
        };
        // Act
        var result = await _userService.GetOrCreateRoleAsync(roleName.RoleName);

        // Asset
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetOrCreateRoleAsync_ShouldNotCreateRole_ReturnRoleId()
    {
        // Arrange
        var roleName = new UserRegisterDto
        {
            RoleName = null!,
        };
        // Act
        var result = await _userService.GetOrCreateRoleAsync(roleName.RoleName);

        // Asset
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetOrCreateAddressAsync_ShouldCreateAddress_ReturnAddressId()
    {
        // Arrange
        var addressDto = new UserRegisterDto
        {
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara"
        };
        // Act
        var result = await _userService.GetOrCreateAddressAsync(addressDto.StreetName, addressDto.PostalCode, addressDto.City);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetOrCreateAddressAsync_ShouldNotCreateAddress_ReturnAddressId()
    {
        // Arrange
        var addressDto = new UserRegisterDto
        {
            StreetName = null!,
            PostalCode = "12345",
            City = "Skara"
        };
        // Act
        var result = await _userService.GetOrCreateAddressAsync(addressDto.StreetName, addressDto.PostalCode, addressDto.City);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CreateUserEntity_ShouldSaveUserEntityToDatabase_ReturnUserEntity()
    {
        // Arrange
        var createdRoleId = await _userService.GetOrCreateRoleAsync("Admin");
        var createdAddressId = await _userService.GetOrCreateAddressAsync("Skara", "12345", "Skara");

        // Act
        var result = await _userService.CreateUserEntityAsync(createdRoleId, createdAddressId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserEntity>(result);  
    }

    [Fact]
    public async Task CreateUserEntity_ShouldNotSaveUserEntityToDatabase_ReturnNull()
    {
        // Arrange
        var createdRoleId = await _userService.GetOrCreateRoleAsync(null!);
        var createdAddressId = await _userService.GetOrCreateAddressAsync("Skara", "12345", "Skara");

        // Act
        var result = await _userService.CreateUserEntityAsync(createdRoleId, createdAddressId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProfileEntity_ShouldSaveProfileEntityToDataBase_ReturnTrue()
    {
        // Arrange
        var profileEntity = new UserRegisterDto
        {
            FirstName = "Emil",
            LastName = "Vettainen"
        };
        // Act
        var result = await _userService.CreateProfileEntityAsync(profileEntity, Guid.NewGuid());
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateProfileEntity_ShouldNotSaveProfileEntityToDataBase_ReturnFalse()
    {
        // Arrange
        var profileEntity = new UserRegisterDto {};
        // Act
        var result = await _userService.CreateProfileEntityAsync(profileEntity, Guid.NewGuid());
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateAuthEntity_ShouldSaveAuthEntityToDatabase_ReturnTrue()
    {
        // Arrange
        var authEntity = new UserRegisterDto { Email = "emil@domain.com", Password= "password" };
        // Act
        var result = await _userService.CreateAuthEntityAsync(authEntity, Guid.NewGuid());
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateAuthEntity_ShouldNotSaveAuthEntityToDatabase_ReturnFalse()
    {
        // Arrange
        var authEntity = new UserRegisterDto { Email = "emil@domain.com", Password = null! };
        // Act
        var result = await _userService.CreateAuthEntityAsync(authEntity, Guid.NewGuid());
        // Assert
        Assert.False(result);
    }









}