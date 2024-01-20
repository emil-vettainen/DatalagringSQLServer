using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.MAUI.Mvvm.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {

        //[RelayCommand]
        //async Task GoToHomePage()
        //{

        //}


        [RelayCommand]
        async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
