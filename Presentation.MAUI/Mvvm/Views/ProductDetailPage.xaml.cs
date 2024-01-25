using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class ProductDetailPage : ContentPage
{
	public ProductDetailPage(ProductDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}