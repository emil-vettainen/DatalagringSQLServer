using Business.Dtos;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.UserRepositories;

namespace Business.Services.UserServices;


public class UserService(UserRepository userRepository, RoleRepository roleRepository, AuthenticationRepository authenticationRepository, ProfileRepository profileRepository, AddressRepository addressRepository)
{

    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AuthenticationRepository _authenticationRepository = authenticationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;




    public async Task<bool> CreateUser(UserRegisterDto userRegisterDto)
    {
        var userExists = await _authenticationRepository.ExistsAsync(x => x.Email == userRegisterDto.Email);

        if (userExists)
        {
            return false;
        }

        var roleExists = await _roleRepository.ExistsAsync(x => x.RoleName == userRegisterDto.RoleName);
        int roleId;

        if (roleExists)
        {
            var existingRole = await _roleRepository.GetOneAsync(x => x.RoleName == userRegisterDto.RoleName);
            roleId = existingRole.Id;
        }

        else
        {
            var roleEntity = new RoleEntity
            {
                RoleName = userRegisterDto.RoleName,
            };

            var createdRole = await _roleRepository.CreateAsync(roleEntity);
            roleId = createdRole.Id;
        }
        

        var addressEntity = new AddressEntity
        {
            Id = userRegisterDto.AddressId,
            StreetName = userRegisterDto.StreetName,
            PostalCode = userRegisterDto.PostalCode,
            City = userRegisterDto.City,
        };

        var createdAddress = await _addressRepository.CreateAsync(addressEntity);

        var userEntity = new UserEntity
        {
            Id = userRegisterDto.Id,
            Created = userRegisterDto.Created,
            AddressId = createdAddress.Id,
            RoleId = roleId,
        };

        await _userRepository.CreateAsync(userEntity);

        var authEntity = new AuthenticationEntity
        {
            UserId = userEntity.Id,
            Email = userRegisterDto.Email,
            Password = userRegisterDto.Password,
        };

        await _authenticationRepository.CreateAsync(authEntity);

        var profileEntity = new ProfileEntity
        {
            UserId = userEntity.Id,
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
        };

        await _profileRepository.CreateAsync(profileEntity);

        return true;

    }
}
