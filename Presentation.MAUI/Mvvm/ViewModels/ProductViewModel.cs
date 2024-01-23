using Business.Dtos.ProductDtos;
using Business.Services.ProductServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.ProductModels;
using Presentation.MAUI.Mvvm.Views;
using System.Collections.ObjectModel;


namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class ProductViewModel : ObservableObject
{

    private readonly ProductService _productService;

    public ProductViewModel(ProductService productService)
    {
        _productService = productService;
        LoadProducts().ConfigureAwait(false);

        _productService.UpdateProductList += (sender, e) =>
        {
            LoadProducts().ConfigureAwait(false);
        };
    }

    [ObservableProperty]
    ProductModel _productModel = new();

    [ObservableProperty]
    ObservableCollection<ProductModel> _productsList = [];


    [RelayCommand]
    async Task GoToAddProduct()
    {
        await Shell.Current.GoToAsync($"{nameof(AddProductPage)}");
    }


    public async Task LoadProducts()
    {
        ProductsList.Clear();

        var productDtos = await _productService.GetAllProductsAsync();
        foreach (var dto in productDtos)
        {
            var model = new ProductModel
            {
                ArticleNumber = dto.ArticleNumber,
                Title = dto.Title,
                Description = dto.Description,
                CategoryName = dto.CategoryName,
                Manufacture = dto.Manufacture,
                Specification = dto.Specification,
              
            };

          
            ProductsList.Add(model);
        }

    }
    


 

}
