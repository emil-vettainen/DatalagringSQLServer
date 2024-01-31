using Infrastructure.Contexts;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Utilis;

namespace Infrastructure.Tests.Repositories.UserRepositories;

public class AddressRepository_Tests
{

    private readonly UserDataContext _userDataContext = new(new DbContextOptionsBuilder<UserDataContext>()
       .UseInMemoryDatabase($"{Guid.NewGuid()}")
       .Options);

    private readonly IErrorLogger errorLogger = new ErrorLogger($"{Guid.NewGuid()}");


    [Fact]
    public async Task CheckIfAddressExist_Should_ReturnTrueIfExsist()
    {
        // Arrange
        var addressRepository = new AddressRepository(_userDataContext, errorLogger);
        var addressEntity = new AddressEntity
        {
            StreetName = "Gata",
            PostalCode = "12345",
            City = "Ort",
        };

        var createdAddress = await addressRepository.CreateAsync(addressEntity);
       

        // Act
        var result = await addressRepository.ExistsAsync(x => x.Id == createdAddress.Id);

        // Assert
        Assert.True(result);
    }
}
