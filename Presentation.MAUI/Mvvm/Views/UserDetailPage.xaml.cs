using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class UserDetailPage : ContentPage
{
	public UserDetailPage(UserDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserDetailViewModel viewModel)
        {
            await viewModel.ShowUserFirstName();
        }
    }
}