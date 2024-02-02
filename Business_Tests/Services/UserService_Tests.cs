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

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUserEmail_ReturnStatusUpdated()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        // Act

        var result = await _userService.UpdateUserAsync(new UserUpdateDto
        {
            Id = existingUser.Id,
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "annan@domain.com",
            Password = "Bytmig123!"

       });

        // Assert
        Assert.Equal(ResultStatus.Updated, result.Status);
        Assert.Equal("annan@domain.com", existingUser.Authentication.Email);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateAllUserDetails_ReturnStatusUpdated()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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

        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        // Act
        var result = await _userService.UpdateUserAsync(new UserUpdateDto
        {
            Id = existingUser.Id,
            RoleName = "User",
            FirstName = "Annan",
            LastName = "Efternamn",
            StreetName = "Gbg",
            PostalCode = "12346",
            City = "Gbg",
            Email = "annan@domain.com",
           

        });
        var updatedUser = await _userService.GetUserDetailsAsync(existingUser.Id);

        // Assert
        Assert.Equal(ResultStatus.Updated, result.Status);
        Assert.Equal("annan@domain.com", existingUser.Authentication.Email);
        Assert.Equal("User", existingUser.Role.RoleName);
        Assert.Equal("Efternamn", existingUser.Profile.LastName);
        Assert.Equal("Gbg", existingUser.Address.StreetName);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldNotUpdateUserEmailIfEmailExists_ReturnStatusAlreadyExists()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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

        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        var user2 = await _userService.CreateUserAsync(new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "annan@domain.com",
            Password = "Bytmig123!",
        });

        // Act
        var result = await _userService.UpdateUserAsync(new UserUpdateDto
        {
            Id = existingUser.Id,
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "annan@domain.com",
            Password = "Bytmig123!"

        });

        // Assert
        Assert.Equal(ResultStatus.AlreadyExist, result.Status);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldNotUpdateUserIfRequiredIsNull_ReturnStatusFailed()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        // Act
        var result = await _userService.UpdateUserAsync(new UserUpdateDto
        {
            Id = existingUser.Id,
            RoleName = "Admin",
            FirstName = null!,
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "annan@domain.com",
            Password = "Bytmig123!"
        });

        // Assert
        Assert.Equal(ResultStatus.Failed, result.Status);
    }

    [Fact]
    public async Task UpdateUserEntity_ShouldUpdateUserEntity_ReturnTrue()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        //Act
        var result = await _userService.UpdateUserEntityAsync(existingUser.Id, 2, 2);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateUserEntity_ShouldNotUpdateUserEntityIfNoRoleId_ReturnFalse()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        //Act
        var result = await _userService.UpdateUserEntityAsync(existingUser.Id, 0, 2);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateProfileEntity_ShouldUpdateProfile_ReturnTrue()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        // Act

        var result = await _userService.UpdateProfileEntityAsync(existingUser.Id, "Annan", "Efternamn");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAuthEntity_ShouldUpdateAuth_ReturnTrue()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        // Act
        var result = await _userService.UpdateAuthEntityAsync(existingUser.Id, "Annan@domain.com", "AnnatPassword!");
        var isValid = _userService.ValidatePassword("AnnatPassword!", existingUser.Authentication.PasswordHash, existingUser.Authentication.PasswordKey);

        // Assert
        Assert.True(result);
        Assert.Equal("Annan@domain.com", existingUser.Authentication.Email);
        Assert.True(isValid);
    }

    [Fact]
    public async Task UpdateAuthEntity_ShouldNotUpdateAuthIfEmailExists_ReturnFalse()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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
        var user2 = await _userService.CreateUserAsync(new UserRegisterDto
        {
            RoleName = "Admin",
            FirstName = "Emil",
            LastName = "Vettainen",
            StreetName = "Skara",
            PostalCode = "12345",
            City = "Skara",
            Email = "Annan@domain.com",
            Password = "Bytmig123!",
        });
        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");

        // Act
        var result = await _userService.UpdateAuthEntityAsync(existingUser.Id, "Annan@domain.com", "AnnatPassword!");
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUserById_ShouldDeleteUserFromDatabase_ReturnDeleted()
    {
        // Arrange
        var user = await _userService.CreateUserAsync(new UserRegisterDto
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

        var existingUser = await _userService.GetUserAsync(x => x.Authentication.Email == "emil@domain.com");
        // Act

        var result = await _userService.DeleteUserByIdAsync(existingUser.Id);

        // Assert
        Assert.Equal(ResultStatus.Deleted, result.Status);
    }

    [Fact]
    public async Task DeleteUserById_ShouldNotDeleteUserIfNotFound_ReturnNotFound()
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
        var result = await _userService.DeleteUserByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public void GenerateSecurePassword_ShouldCreateHashPassword_ValidateHash_ReturnTrue()
    {
        //Arrange
        var password = "Bytmig123!";
        _userService.GenerateSecurePassword(password, out string passwordHash, out string passwordKey);

        // Act
        var isValid = _userService.ValidatePassword("Bytmig123!", passwordHash, passwordKey);

        // Assert
        Assert.True(isValid);
    }



}