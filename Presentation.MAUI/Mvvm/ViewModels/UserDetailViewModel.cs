using Business.Dtos;
using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.UserModels;
using Presentation.MAUI.Mvvm.Views;
using Shared.Enums;
using Shared.Helper;


namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class UserDetailViewModel(UserService userService) : ObservableObject
{

    private readonly UserService _userService = userService;

    [ObservableProperty]
    UserDetailsModel userDetailsModel = new();

    [ObservableProperty]
    List<string> roles = ["Admin", "User", "Manager"];

    public async Task ShowUserDetails()
    {
        var user = await _userService.GetUserDetailsAsync(AppState.UserId);
        if (user != null)
        {
            UserDetailsModel.RoleName = user.RoleName;
            UserDetailsModel.SlecectedRoleIndex = Roles.IndexOf(UserDetailsModel.RoleName);
            UserDetailsModel.Id = user.Id;
            UserDetailsModel.FirstName = user.FirstName;
            UserDetailsModel.LastName = user.LastName;
            UserDetailsModel.StreetName = user.StreetName;
            UserDetailsModel.PostalCode = user.PostalCode;
            UserDetailsModel.City = user.City;
            UserDetailsModel.Email = user.Email;
        }
    }

    [ObservableProperty]
    string? _newPassword;

    [RelayCommand]
    async Task UpdateUser()
    {
        if (!string.IsNullOrWhiteSpace(UserDetailsModel.FirstName) &&
            !string.IsNullOrWhiteSpace(UserDetailsModel.LastName) &&
            !string.IsNullOrWhiteSpace(UserDetailsModel.StreetName) &&
            !string.IsNullOrWhiteSpace(UserDetailsModel.PostalCode) &&
            !string.IsNullOrWhiteSpace(UserDetailsModel.City))
        {
            var roleName = Roles.ElementAtOrDefault(UserDetailsModel.SlecectedRoleIndex);
            if (roleName != null)
            {
                var result = await _userService.UpdateUserAsync(new UserUpdateDto
                {
                    Id = UserDetailsModel.Id,
                    RoleName = roleName,
                    FirstName = UserDetailsModel.FirstName,
                    LastName = UserDetailsModel.LastName,
                    StreetName = UserDetailsModel.StreetName,
                    PostalCode = UserDetailsModel.PostalCode,
                    City = UserDetailsModel.City,
                    Email = UserDetailsModel.Email,
                    Password = string.IsNullOrWhiteSpace(NewPassword) ? null! : NewPassword 
                });

                switch (result.Status)
                {
                    case ResultStatus.Updated:
                        NewPassword = string.Empty;
                        await Shell.Current.DisplayAlert("Updated!", "User was updated", "Ok");
                        break;

                    case ResultStatus.AlreadyExist:
                        NewPassword = string.Empty;
                        await Shell.Current.DisplayAlert("Something went wrong!", "Email already exists", "Ok");
                        break;

                    default:
                        NewPassword = string.Empty;
                        await Shell.Current.DisplayAlert("Something went wrong!", "Please try again", "Ok");
                        break;
                }
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
            "Email is required\n",
            "Ok");
        }
    }

    [RelayCommand]
    async Task DeleteUser()
    {
        var answer = await Shell.Current.DisplayAlert("Warning!", "Are you sure you want to delete?\nThis action cannot be undone.", "Ok", "Cancel");
        if(answer)
        {
            var result = await _userService.DeleteUserByIdAsync(AppState.UserId);
            switch (result.Status)
            {
                case ResultStatus.Deleted:
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", animate: false);
                    break;

                default:
                    await Shell.Current.DisplayAlert("Something went wrong!", "Please try again", "Ok");
                    break;
            }
        }
    }
}