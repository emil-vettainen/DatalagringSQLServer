using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class ProductPage : ContentPage
{
	public ProductPage(ProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		
	}
}