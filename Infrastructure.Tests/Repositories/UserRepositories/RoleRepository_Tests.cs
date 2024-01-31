using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class RoleRepository_Tests 
{

    private readonly UserDataContext _userDataContext = new (new DbContextOptionsBuilder<UserDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    private readonly IErrorLogger errorLogger = new ErrorLogger($"{Guid.NewGuid()}");


    [Fact]
    public async Task CheckIfRoleExist_Should_ReturnTrueIfExsist_ElseReturnFalse()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };
        await roleRepository.CreateAsync(roleEntity);

        // Act
        var result = await roleRepository.ExistsAsync(x => x.RoleName == roleEntity.RoleName);

        // Assert
        Assert.True(result);
    }

    [Fact]
   public async Task CreateRoleAsync_ShouldSaveRoleToDatabase_ReturnRoleEntity()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };

        // Act
        var result = await roleRepository.CreateAsync(roleEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldNotSaveRoleToDatabase_ReturnNull()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity {};

        // Act
        var result = await roleRepository.CreateAsync(roleEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeRoleEntity()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };
        await roleRepository.CreateAsync(roleEntity);

        // Act
        var result = await roleRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<RoleEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetOneAsync_ShouldGetOneRoleFromDatabase_ReturnOneRoleEntity()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };
        await roleRepository.CreateAsync(roleEntity);

        // Act
        var result = await roleRepository.GetOneAsync(x => x.RoleName == roleEntity.RoleName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RoleEntity>(result);
        Assert.Equal(roleEntity.RoleName, result.RoleName);
    }

    [Fact]
    public async Task GetOneAsync_ShouldNotFindOneRoleFromDatabase_ReturnNull()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };

        // Act
        var result = await roleRepository.GetOneAsync(x => x.RoleName == roleEntity.RoleName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_RemoveOneRole_ReturnTure()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };
        await roleRepository.CreateAsync(roleEntity);

        // Act
        var result = await roleRepository.DeleteAsync(x => x.RoleName == roleEntity.RoleName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_NotRemoveOneRole_ReturnFalse()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };
     
        // Act
        var result = await roleRepository.DeleteAsync(x => x.RoleName == roleEntity.RoleName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateRoleWithNewRole_ReturnUpdatedRoleEntity()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };
        roleEntity = await roleRepository.CreateAsync(roleEntity);

        // Act
        roleEntity.RoleName = "User";
        var result = await roleRepository.UpdateAsync(x => x.Id == roleEntity.Id, roleEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roleEntity.Id, result.Id);
        Assert.Equal("User", result.RoleName);
    }

    [Fact]
    public async Task UpdateAsync_Should_NotUpdateRole_IfNoNewRoleExists_ReturnNull()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var nonExistingRoleId = 999;
        var roleEntityToUpdate = new RoleEntity { RoleName = "NewRole" };

        // Act
        var result = await roleRepository.UpdateAsync(x => x.Id == nonExistingRoleId, roleEntityToUpdate);

        // Assert
        Assert.Null(result);
    }
}