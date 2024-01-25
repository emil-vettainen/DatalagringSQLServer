﻿using Business.Dtos;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.UserRepositories;
using Shared.Enums;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Responses;
using Shared.Utilis;

namespace Business.Services.UserServices;


public class UserService(UserRepository userRepository, RoleRepository roleRepository, AuthenticationRepository authenticationRepository, ProfileRepository profileRepository, AddressRepository addressRepository, IErrorLogger errorLogger)
{

    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AuthenticationRepository _authenticationRepository = authenticationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;

    private readonly IServiceResult _result = new ServiceResult();
    private readonly IErrorLogger _errorLogger = errorLogger;





    public async Task<IServiceResult> CreateUser(UserRegisterDto userRegisterDto)
    {
        try
        {
            if (await _authenticationRepository.ExistsAsync(x => x.Email == userRegisterDto.Email))
            {
                _result.Status = ResultStatus.AlreadyExist;
            }
            var roleId = await GetOrCreateRoleAsync(userRegisterDto.RoleName);
            var addressId = await GetOrCreateAddressAsync(userRegisterDto.StreetName, userRegisterDto.PostalCode, userRegisterDto.City);
            var userEntity = await CreateUserEntity(roleId, addressId);
            await CreateProfile(userRegisterDto, userEntity.Id );
            await CreateAuth(userRegisterDto, userEntity.Id);

            _result.Status = ResultStatus.Successed;
        }
        catch (Exception ex)
        {
            _errorLogger.ErrorLog(ex.Message, "UserService - CreateUser");
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
                Id = user.Id,
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

    public async Task<IServiceResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            var roleId = await GetOrCreateRoleAsync(userUpdateDto.RoleName);

            var user = await _userRepository.GetOneAsync(x => x.Id == userUpdateDto.Id);
            if (user != null)
            {
                user.RoleId = roleId;
                user.Modified = userUpdateDto.Modified;
                await _userRepository.UpdateAsync(x => x.Id == user.Id, user);
            }

            var profile = await _profileRepository.GetOneAsync(x => x.UserId == userUpdateDto.Id);
            if (profile != null)
            {
                profile.FirstName = userUpdateDto.FirstName;
                profile.LastName = userUpdateDto.LastName;

                await _profileRepository.UpdateAsync(x => x.UserId == userUpdateDto.Id, profile);
            }

            _result.Status = ResultStatus.Updated;
        }
        catch (Exception)
        {
            _result.Status = ResultStatus.Failed;
          
        }

        return _result;

    }

    private async Task<int> GetOrCreateRoleAsync(string roleName)
    {
        try
        {
            var roleExists = await _roleRepository.ExistsAsync(x => x.RoleName == roleName);
            if (roleExists)
            {
                var existingRole = await _roleRepository.GetOneAsync(x => x.RoleName == roleName);
                return existingRole.Id;
            }
            else
            {
                var roleEntity = new RoleEntity { RoleName = roleName };
                var createdRole = await _roleRepository.CreateAsync(roleEntity);
                return createdRole.Id;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - ExistsAsync"); }
        return 0;
    }


    private async Task<int> GetOrCreateAddressAsync(string streetName, string postalCode, string city)
    {
        try
        {
            var addressExists = await _addressRepository.ExistsAsync(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
            if (addressExists)
            {
                var existingAddress = await _addressRepository.GetOneAsync(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
                return existingAddress.Id;
            }
            else
            {
                var addressEntity = new AddressEntity { StreetName = streetName, PostalCode = postalCode, City = city };
                var createdAddress = await _addressRepository.CreateAsync(addressEntity);
                return createdAddress.Id;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - ExistsAsync"); }
        return 0;
    }


    private async Task CreateProfile(UserRegisterDto userRegisterDto, Guid userId)
    {
        try
        {
            var profileEntity = new ProfileEntity
            {
                UserId = userId,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
            };

            await _profileRepository.CreateAsync(profileEntity);
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - ExistsAsync"); }
    }

    private async Task CreateAuth(UserRegisterDto userRegisterDto, Guid userId)
    {
        try
        {
            var authEntity = new AuthenticationEntity
            {
                UserId= userId,
                Email = userRegisterDto.Email,
                Password = userRegisterDto.Password,
            };

            await _authenticationRepository.CreateAsync(authEntity);
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - ExistsAsync"); }
    }

    private async Task<UserEntity> CreateUserEntity(int roleId, int addressId)
    {
        try
        {
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                RoleId = roleId,
                AddressId = addressId,
            };

           return await _userRepository.CreateAsync(userEntity);  
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "BaseRepo - ExistsAsync"); }
        return null!;
    }


    public async Task<IServiceResult> DeleteUserByIdAsync(Guid userId)
    {
        try
        {
            var deletedUser = await _userRepository.DeleteAsync(x => x.Id == userId);
            _result.Status = deletedUser ? ResultStatus.Deleted : ResultStatus.NotFound;
        }
        catch (Exception ex)
        { _result.Status = ResultStatus.Failed; _errorLogger.ErrorLog(ex.Message, "UserRepo - DeleteUserByIdAsync"); }
        return _result;
    }
}