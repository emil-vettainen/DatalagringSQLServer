using Business.Dtos;
using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.UserModels;
using Shared.Enums;

namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class RegisterViewModel(UserService userService) : ObservableObject
{
    private readonly UserService _userService = userService;


    [ObservableProperty]
    UserRegisterModel _userRegister = new();

    [ObservableProperty]
    List<string> roles = ["Admin", "User", "Manager"];

    [RelayCommand]
    async Task RegisterUser()
    {
        if (!string.IsNullOrWhiteSpace(UserRegister.RoleName) &&
            !string.IsNullOrWhiteSpace(UserRegister.FirstName) &&
            !string.IsNullOrWhiteSpace(UserRegister.LastName) &&
            !string.IsNullOrWhiteSpace(UserRegister.StreetName) &&
            !string.IsNullOrWhiteSpace(UserRegister.PostalCode) &&
            !string.IsNullOrWhiteSpace(UserRegister.City) &&
            !string.IsNullOrWhiteSpace(UserRegister.Email) &&
            !string.IsNullOrWhiteSpace(UserRegister.Password))
            
        {
            var result = await _userService.CreateUserAsync(new UserRegisterDto
            {
                RoleName = UserRegister.RoleName,
                FirstName = UserRegister.FirstName,
                LastName = UserRegister.LastName,
                StreetName = UserRegister.StreetName,
                PostalCode = UserRegister.PostalCode,
                City = UserRegister.City,
                Email = UserRegister.Email,
                Password = UserRegister.Password,
            });

            switch (result.Status) 
            {
                case ResultStatus.Successed:
                    UserRegister = new();
                    await Shell.Current.DisplayAlert("Success", "User was created", "Ok");
                    await Shell.Current.GoToAsync("..");
                    break;

                case ResultStatus.AlreadyExist:
                    UserRegister = new();
                    await Shell.Current.DisplayAlert("Something went wrong", "User already exists", "Ok");
                    break;

                default:
                    UserRegister = new();
                    await Shell.Current.DisplayAlert("Something went wrong", "Please try again", "Ok");
                    break;
            }
        }
        else
        {
            await Shell.Current.DisplayAlert
                ("Something went wrong!", 
                "Rolenname is required\n" +
                "Firstname is required\n" +
                "Lastname is required\n" +
                "Streetname is required\n" +
                "Postalcode is required\n" +
                "City is required\n" +
                "Email is required\n" +
                "Password is required", 
                "Ok");
        }
    }
}