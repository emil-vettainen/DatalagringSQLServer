using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class AddProductPage : ContentPage
{
	public AddProductPage(AddProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}