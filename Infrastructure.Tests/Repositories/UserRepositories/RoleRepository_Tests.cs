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
   public async Task CreateRoleAsync()
    {
        // Arrange
        var roleRepository = new RoleRepository(_userDataContext, errorLogger);
        var roleEntity = new RoleEntity { RoleName = "Admin" };

        // Act
        var result = await roleRepository.CreateAsync( roleEntity );

        // Assert
        Assert.NotNull(result);

    }
    

}
