using Business.Dtos;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.UserRepositories;
using Shared.Enums;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Responses;
using Shared.Utilis;

namespace Business.Services.UserServices;


public class UserService(UserRepository userRepository, RoleRepository roleRepository, AuthenticationRepository authenticationRepository, ProfileRepository profileRepository, AddressRepository addressRepository)
{

    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AuthenticationRepository _authenticationRepository = authenticationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;

    private readonly IServiceResult _result = new ServiceResult();


    public async Task<IServiceResult> CreateUser(UserRegisterDto userRegisterDto)
    {
        try
        {
            if (await _authenticationRepository.ExistsAsync(x => x.Email == userRegisterDto.Email))
            {
                _result.Status = ResultStatus.AlreadyExist;
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

            
        }
        catch (Exception ex) 
        {
          
            _result.Status = ResultStatus.Failed; 
        }
        return _result;
    }

 

    public async Task<IServiceResult> LoginAsync(UserLoginDto userLoginDto)
    {
        try
        {
            var user = await _authenticationRepository.GetOneAsync(x => x.Email == userLoginDto.Email);
            if (user != null && user.Password == userLoginDto.Password)
            {
                _result.Status = ResultStatus.Successed;
                AppState.UserId = user.UserId;
                AppState.IsAuthenticated = true;

              
            }
            else
            {
                _result.Status = ResultStatus.NotFound;
            }  
        }
        catch (Exception) { _result.Status = ResultStatus.Failed; }
        return _result;
    }


    public async Task<UserDetailsDto> GetUserDetailsAsync(Guid userId)
    {
        var user = await _userRepository.GetOneAsync(x => x.Id == userId);
        if (user != null)
        {
            var userDetails = new UserDetailsDto
            {
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
                StreetName = user.Address.StreetName,
                PostalCode = user.Address.PostalCode,
                City = user.Address.City,
                RoleName = user.Role.RoleName
                
                


            };

            return userDetails;
        }

        return null!;
    }


}


