using Business.Services.UserServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models;
using Presentation.MAUI.Mvvm.Views;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.MAUI.Mvvm.ViewModels
{
    [QueryProperty(nameof(UserDetailsModel), nameof(UserDetailsModel))]
    public partial class UserDetailViewModel : ObservableObject
    {

        private readonly UserService _userService;

        public UserDetailViewModel(UserService userService)
        {
            _userService = userService;
            ShowUserFirstName().ConfigureAwait(false);
        }

        [ObservableProperty]
        UserDetailsModel userDetailsModel = new();


        public async Task ShowUserFirstName()
        {

          

            var user = await _userService.GetUserDetailsAsync(AppState.UserId);

            if (user != null)
            {

                UserDetailsModel.FirstName = user.FirstName;
                UserDetailsModel.LastName = user.LastName;
                UserDetailsModel.StreetName = user.StreetName;
                UserDetailsModel.PostalCode = user.PostalCode;
                UserDetailsModel.City = user.City;
                UserDetailsModel.RoleName = user.RoleName;
        

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
}
