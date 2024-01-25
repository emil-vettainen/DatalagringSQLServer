using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class EditProductPage : ContentPage
{
	public EditProductPage(EditProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}