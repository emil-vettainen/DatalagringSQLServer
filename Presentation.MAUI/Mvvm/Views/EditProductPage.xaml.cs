using Presentation.MAUI.Mvvm.ViewModels;

namespace Presentation.MAUI.Mvvm.Views;

public partial class EditProductPage : ContentPage
{
	public EditProductPage(EditProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is EditProductViewModel viewModel)
        {
            await viewModel.ShowEditProductDetails();
        }
    }

}