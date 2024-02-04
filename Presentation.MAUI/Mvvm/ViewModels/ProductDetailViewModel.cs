using Business.Services.ProductServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.ProductModels;
using Presentation.MAUI.Mvvm.Views;

namespace Presentation.MAUI.Mvvm.ViewModels;

[QueryProperty(nameof(ArticleNumber), nameof(ArticleNumber))]
public partial class ProductDetailViewModel : ObservableObject
{
    private readonly ProductService _productService;

    public ProductDetailViewModel(ProductService productService)
    {
        _productService = productService;

    }

    [ObservableProperty]
    string? articleNumber;

    [ObservableProperty]
    private ProductModel? _productDetail;

    public async Task ShowProductDetails()
    {
        if (ArticleNumber != null)
        {
            var productDetail = await _productService.GetOneProductAsync(ArticleNumber);

            if (productDetail != null)
            {
                var detail = new ProductModel
                {
                    CategoryName = productDetail.CategoryName,
                    ArticleNumber = productDetail.ArticleNumber,
                    ProductTitle = productDetail.ProductTitle,
                    Ingress = productDetail.Ingress,
                    Description = productDetail.Description,
                    Specification = productDetail.Specification,
                    Manufacture = productDetail.Manufacture,
                    Price = productDetail.Price,
                };
                ProductDetail = detail;
            }
        }
    }


    [RelayCommand]
    async Task GoToEditProductPage()
    {
        if (ArticleNumber != null)
        {
            await Shell.Current.GoToAsync($"/EditProductPage?ArticleNumber={ArticleNumber}");
        }
    }
}