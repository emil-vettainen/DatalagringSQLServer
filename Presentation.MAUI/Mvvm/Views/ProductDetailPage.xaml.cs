using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class ProductDetailPage : ContentPage
{
	public ProductDetailPage(ProductDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		if (BindingContext is ProductDetailViewModel viewModel)
		{
			await viewModel.ShowProductDetails();
		}
	}
}