using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models;
using Presentation.MAUI.Mvvm.Views;
using Shared.Helper;
using System.Collections.ObjectModel;


namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class HomeViewModel : ObservableObject
{

    private readonly UserService _userService;

    public HomeViewModel(UserService userService)
    {
        _userService = userService;

    }


    [ObservableProperty]
    UserDetailsModel userDetailsModel = new();

    [RelayCommand]
    async Task GoToUserSetting()
    {
        await Shell.Current.GoToAsync($"{nameof(UserDetailPage)}");
    }




 
    public async Task ShowUserFirstName()
    {
        
       
         var user = await _userService.GetUserDetailsAsync(AppState.UserId) ;

         if(user != null)
        {

            UserDetailsModel.FirstName = user.FirstName;
            UserDetailsModel.LastName = user.LastName;
            UserDetailsModel.RoleName = user.RoleName;
            UserDetailsModel.Email = user.Email;

         }



    }

  
}
