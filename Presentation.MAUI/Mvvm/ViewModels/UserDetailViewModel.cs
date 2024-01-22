using Business.Dtos;
using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models;
using Presentation.MAUI.Mvvm.Views;
using Shared.Enums;
using Shared.Helper;


namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class UserDetailViewModel : ObservableObject
{

    private readonly UserService _userService;

    public UserDetailViewModel(UserService userService)
    {
        _userService = userService;
    }


    [ObservableProperty]
    bool _isBusy = false;



    [ObservableProperty]
    UserDetailsModel userDetailsModel = new();


    public async Task ShowUserFirstName()
    {
        var user = await _userService.GetUserDetailsAsync(AppState.UserId);

        if (user != null)
        {
            UserDetailsModel.Id = user.Id;
            UserDetailsModel.FirstName = user.FirstName;
            UserDetailsModel.LastName = user.LastName;
            UserDetailsModel.StreetName = user.StreetName;
            UserDetailsModel.PostalCode = user.PostalCode;
            UserDetailsModel.City = user.City;
            UserDetailsModel.RoleName = user.RoleName;
        }
    }

    [RelayCommand]
    async Task DeleteUser()
    {
        var answer = await Shell.Current.DisplayAlert("Warning!", "Are you sure you want to delete the contact?\nThis action cannot be undone.", "Ok", "Cancel");
        if(answer)
        {
            var result = await _userService.DeleteUserByIdAsync(AppState.UserId);

            switch (result.Status)
            {
                case ResultStatus.Deleted:
                    IsBusy = true;
                    await Task.Delay(2000);
                    IsBusy = false;
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", animate: false);
                    break;

                default:
                    IsBusy = true;
                    await Task.Delay(2000);
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Something went wrong!", "Please try again", "Ok");
                    break;
            }
        }
    }


    [RelayCommand]
    async Task UpdateUser()
    {
        if (!string.IsNullOrWhiteSpace(UserDetailsModel.FirstName))
        {
            var userUpdateDto = new UserUpdateDto
            {
                Id = UserDetailsModel.Id,
                FirstName = UserDetailsModel.FirstName,
                LastName = UserDetailsModel.LastName,
                StreetName = UserDetailsModel.StreetName,
                PostalCode = UserDetailsModel.PostalCode,
                City = UserDetailsModel.City,
                RoleName = UserDetailsModel.RoleName,

            };

            await _userService.UpdateUserAsync(userUpdateDto);
        }

    }


    [RelayCommand]
    async Task SignOut()
    {
        AppState.IsAuthenticated = false;
        AppState.UserId = Guid.Empty;
       
     
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}
