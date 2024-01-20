using Business.Dtos;
using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models;
using Shared.Enums;

namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class RegisterViewModel(UserService userService) : ObservableObject
{
    private readonly UserService _userService = userService;


    [ObservableProperty]
    UserRegisterModel _userRegister = new();

    [RelayCommand]
    async Task RegisterUser()
    {
        if (!string.IsNullOrWhiteSpace(UserRegister.FirstName) &&
            !string.IsNullOrWhiteSpace(UserRegister.LastName) &&
            !string.IsNullOrWhiteSpace(UserRegister.Email) &&
            !string.IsNullOrWhiteSpace(UserRegister.Password) &&
            !string.IsNullOrWhiteSpace(UserRegister.RoleName))
        {
            var userRegisterDto = new UserRegisterDto
            {

                FirstName = UserRegister.FirstName,
                LastName = UserRegister.LastName,
                Email = UserRegister.Email,
                Password = UserRegister.Password,
                StreetName = UserRegister.StreetName,
                PostalCode = UserRegister.PostalCode,   
                City = UserRegister.City,
                RoleName = UserRegister.RoleName

            };

            var result = await _userService.CreateUser(userRegisterDto);
        }


        else
        {
            await Shell.Current.DisplayAlert("Something went wrong!", "Is required", "Ok");
        }
    }
}
