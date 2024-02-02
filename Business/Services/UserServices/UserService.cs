using Business.Dtos;
using Infrastructure.Entities.UserEntities;
using Infrastructure.Repositories.UserRepositories;
using Shared.Enums;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Responses;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services.UserServices;


public class UserService(UserRepository userRepository, RoleRepository roleRepository, AuthenticationRepository authenticationRepository, ProfileRepository profileRepository, AddressRepository addressRepository, IErrorLogger errorLogger)
{

    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly AuthenticationRepository _authenticationRepository = authenticationRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly IErrorLogger _errorLogger = errorLogger;

    private readonly IServiceResult _result = new ServiceResult();



    public async Task<IServiceResult> LoginAsync(UserLoginDto userLoginDto)
    {
        try
        {
            var user = await _authenticationRepository.GetOneAsync(x => x.Email == userLoginDto.Email);
            if (user != null)
            {
                if(ValidatePassword(userLoginDto.Password, user.PasswordHash, user.PasswordKey))
                {
                    _result.Status = ResultStatus.Successed;
                    AppState.UserId = user.UserId;
                    AppState.IsAuthenticated = true;
                }
                else
                {
                    _result.Status = ResultStatus.WrongPassword;
                }
            }
            else
            {
                _result.Status = ResultStatus.NotFound;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - LoginAsync"); _result.Status = ResultStatus.Failed; }
        return _result;
    }

    public async Task<IServiceResult> CreateUserAsync(UserRegisterDto userRegisterDto)
    {
        try
        {
            if (!await _authenticationRepository.ExistsAsync(x => x.Email == userRegisterDto.Email))
            {
                var roleId = await GetOrCreateRoleAsync(userRegisterDto.RoleName);

                var addressId = await GetOrCreateAddressAsync(userRegisterDto.StreetName, userRegisterDto.PostalCode, userRegisterDto.City);

                if (roleId == 0 || addressId == 0)
                {
                    _result.Status = ResultStatus.Failed;
                }

                var userEntity = await CreateUserEntityAsync(roleId, addressId);
                await CreateProfileEntityAsync(userRegisterDto, userEntity.Id);
                await CreateAuthEntityAsync(userRegisterDto, userEntity.Id);

                _result.Status = ResultStatus.Successed;
            }
            _result.Status = ResultStatus.AlreadyExist;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - CreateUser"); _result.Status = ResultStatus.Failed; }
        return _result;
    }

    public async Task<int> GetOrCreateRoleAsync(string roleName)
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateRoleAsync"); }
        return 0;
    }

    public async Task<int> GetOrCreateAddressAsync(string streetName, string postalCode, string city)
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetOrCreateAddressAsync"); }
        return 0;
    }

    public async Task<UserEntity> CreateUserEntityAsync(int roleId, int addressId)
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
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - CreateUserEntityAsync"); }
        return null!;
    }

    public async Task<bool> CreateProfileEntityAsync(UserRegisterDto userRegisterDto, Guid userId)
    {
        try
        {
            var profileEntity = new ProfileEntity
            {
                UserId = userId,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
            };
            var result = await _profileRepository.CreateAsync(profileEntity);
            return result != null;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - CreateProfileEntityAsync"); }
        return false;
    }

    public async Task<bool> CreateAuthEntityAsync(UserRegisterDto userRegisterDto, Guid userId)
    {
        try
        {
            GenerateSecurePassword(userRegisterDto.Password, out string passwordHash, out string passwordKey);
            var authEntity = new AuthenticationEntity
            {
                UserId = userId,
                Email = userRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordKey = passwordKey,
            };
            var result = await _authenticationRepository.CreateAsync(authEntity);
            return result != null;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - CreateAuthEntityAsync"); }
        return false;
    }

    public async Task<IServiceResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            var roleId = await GetOrCreateRoleAsync(userUpdateDto.RoleName);
            var addressId = await GetOrCreateAddressAsync(userUpdateDto.StreetName, userUpdateDto.PostalCode, userUpdateDto.City);
            if(roleId == 0 || addressId == 0)
            {
                _result.Status = ResultStatus.Failed;
            }
            var result = await UpdateUserEntityAsync(userUpdateDto.Id, roleId, addressId);
            if(result)
            {
                await UpdateProfileEntityAsync(userUpdateDto.Id, userUpdateDto.FirstName, userUpdateDto.LastName);
                var updatedAuth = await UpdateAuthEntityAsync(userUpdateDto.Id, userUpdateDto.Email, userUpdateDto.Password);
                _result.Status = updatedAuth ? ResultStatus.Updated : ResultStatus.AlreadyExist;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - UpdateUserAsync"); _result.Status = ResultStatus.Failed; }
        return _result;
    }

    private async Task<bool> UpdateUserEntityAsync(Guid userId, int roleId, int addressId)
    {
        try
        {
            var newUserEntity = await _userRepository.UpdateAsync(x => x.Id == userId, new UserEntity
            {
                Id = userId,
                RoleId = roleId,
                AddressId = addressId,
                Modified = DateTime.Now,
            });
            return newUserEntity != null;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - UpdateUserEntityAsync"); }
        return false;
    }

    private async Task<bool> UpdateProfileEntityAsync(Guid userId, string firstName, string lastName)
    {
        try
        {
            var newProfileEntity = await _profileRepository.UpdateAsync(x => x.UserId == userId, new ProfileEntity
            {
                FirstName = firstName,
                LastName = lastName,
            });
            return newProfileEntity != null;
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - UpdateProfileEntityAsync"); }
        return false;
    }

    private async Task<bool> UpdateAuthEntityAsync(Guid userId, string email, string? password)
    {
        try
        {
            if (!await _authenticationRepository.ExistsAsync(x => x.Email == email))
            {
                var auth = await _authenticationRepository.GetOneAsync(x => x.UserId == userId);
                if (auth != null)
                {
                    auth.Email = email;

                    if (password != null)
                    {
                        GenerateSecurePassword(password, out string passwordHash, out string passwordKey);
                        auth.PasswordHash = passwordHash;
                        auth.PasswordKey = passwordKey;
                    }

                    var result = await _authenticationRepository.UpdateAsync(x => x.UserId == userId, auth);
                    return result != null;
                }
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - UpdateAuthEntityAsync"); }
        return false;
    }


    public async Task<UserDetailsDto> GetUserDetailsAsync(Guid userId)
    {
        try
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
                    RoleName = user.Role.RoleName,
                    Email = user.Authentication.Email,
                };
                return userDetails;
            }
        }
        catch (Exception ex) { _errorLogger.ErrorLog(ex.Message, "UserService - GetUserDetailsAsync"); }
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



    public void GenerateSecurePassword(string password, out string passwordHash, out string passwordKey)
    {
        using var hmac = new HMACSHA256();
        var key = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        passwordHash = Convert.ToBase64String(hash);
        passwordKey = Convert.ToBase64String(key);
    }

    public bool ValidatePassword(string password, string passwordHash, string passwordKey )
    {
        var stringHash = Convert.FromBase64String(passwordHash);
        var stringKey = Convert.FromBase64String(passwordKey);

        using var hmac = new HMACSHA256(stringKey);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return hash.SequenceEqual(stringHash);
    }
}