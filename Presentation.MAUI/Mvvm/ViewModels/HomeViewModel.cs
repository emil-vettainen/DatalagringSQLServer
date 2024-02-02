using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Presentation.MAUI.Mvvm.Models.UserModels;
using Shared.Helper;


namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class HomeViewModel(UserService userService) : ObservableObject
{
    private readonly UserService _userService = userService;

    [ObservableProperty]
    UserDetailsModel userDetailsModel = new();

    public async Task WelcomeUser()
    {
         var user = await _userService.GetUserDetailsAsync(AppState.UserId) ;
         if(user != null)
         {
            UserDetailsModel.FirstName = user.FirstName;
         }
    }
}