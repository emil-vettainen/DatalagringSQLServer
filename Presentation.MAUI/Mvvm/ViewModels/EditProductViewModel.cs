using Business.Services.ProductServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.MAUI.Mvvm.ViewModels;

[QueryProperty(nameof(ArticleNumber), nameof(ArticleNumber))]
public partial class EditProductViewModel : ObservableObject
{
    private readonly ProductService _productService;

    public EditProductViewModel(ProductService productService)
    {
        _productService = productService;
    }

    [ObservableProperty]
    string? articleNumber;


    [RelayCommand]
    async Task DeleteProduct()
    {
        if (ArticleNumber != null)
        {
            await _productService.DeleteProductByArticleNumber(ArticleNumber);
            await Shell.Current.GoToAsync("../..");
        }
    }


}
