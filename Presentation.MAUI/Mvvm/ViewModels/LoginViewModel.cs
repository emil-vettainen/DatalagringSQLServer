using Business.Dtos;
using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.UserModels;
using Presentation.MAUI.Mvvm.Views;
using Shared.Enums;


namespace Presentation.MAUI.Mvvm.ViewModels
{
    public partial class LoginViewModel(UserService userService) : ObservableObject
    {
        private readonly UserService _userService = userService;

        [ObservableProperty]
        UserLoginModel _userLoginModel = new();

        [RelayCommand]
        async Task GoToHomePage()
        {
            if (!string.IsNullOrWhiteSpace(UserLoginModel.Email) && !string.IsNullOrWhiteSpace(UserLoginModel.Password))
            {
                var loginResult = await _userService.LoginAsync(new UserLoginDto
                {
                    Email = UserLoginModel.Email,
                    Password = UserLoginModel.Password,
                });
                switch (loginResult.Status)
                {
                    case ResultStatus.Successed:
                        UserLoginModel = new();
                        await Shell.Current.GoToAsync($"//{nameof(HomePage)}", animate: false);
                        break;

                    case ResultStatus.WrongPassword:
                        UserLoginModel = new();
                        await Shell.Current.DisplayAlert("Something went wrong", "Make sure you enter the correct email and password", "Ok");
                        break;

                    case ResultStatus.NotFound:
                        UserLoginModel = new();
                        await Shell.Current.DisplayAlert("Something went wrong", "The user does not exists", "Ok");
                        break;

                    default:
                        UserLoginModel = new();
                        await Shell.Current.DisplayAlert("Something went wrong", "Please try again", "Ok");
                        break;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Something went wrong", "Email is required\nPassword is required", "Ok");
            }
        }

        [RelayCommand]
        async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}