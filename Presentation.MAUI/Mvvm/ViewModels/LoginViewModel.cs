using Business.Dtos;
using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models;
using Presentation.MAUI.Mvvm.Views;
using Shared.Enums;


namespace Presentation.MAUI.Mvvm.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {

        private readonly UserService _userService;

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
        }

        [ObservableProperty]
        UserLoginModel _userLoginModel = new();


        [ObservableProperty]
        bool _isBusy = false;


        [RelayCommand]
        async Task GoToHomePage()
        {
            if (!string.IsNullOrWhiteSpace(UserLoginModel.Email) && !string.IsNullOrWhiteSpace(UserLoginModel.Password))
            {
                var userLogin = new UserLoginDto
                {
                    Email = UserLoginModel.Email,
                    Password = UserLoginModel.Password,
                };

                var loginResult = await _userService.LoginAsync(userLogin);

                switch (loginResult.Status)
                {
                    case ResultStatus.Successed:
                        UserLoginModel = new();
                        IsBusy = true;
                        await Task.Delay(2000);
                        IsBusy = false;
                        await Shell.Current.GoToAsync($"//{nameof(HomePage)}", animate: false);
                        break;

                    case ResultStatus.NotFound:
                        UserLoginModel = new();
                        await Shell.Current.DisplayAlert("Something went wrong", "Make sure you enter the correct email and password", "Ok");
                        break;
                    default:
                        UserLoginModel = new();
                        await Shell.Current.DisplayAlert("Something went wrong", "Please try again", "Ok");
                        break;
                }
            }
        }

        [RelayCommand]
        async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
