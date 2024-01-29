using Business.Dtos.ProductDtos;
using Business.Services.ProductServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.ProductModels;


namespace Presentation.MAUI.Mvvm.ViewModels;

public partial class AddProductViewModel : ObservableObject
{
    private readonly ProductService _productService;

    public EventHandler? UpdateProductList;

    public AddProductViewModel(ProductService productService)
    {
        _productService = productService;
    }

    [ObservableProperty]
    CreateProductModel _createProductModel = new();

    //[ObservableProperty]
    //List<string> categories = ["Mobilephones", "Computers", "Network"];


    [RelayCommand]
    async Task AddProduct()
    {
        if (!string.IsNullOrWhiteSpace(CreateProductModel.CategoryName) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.SubCategoryName) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.ArticleNumber) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.ProductTitle) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Ingress) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Description) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Specification) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Manufacture) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.CategoryName))

        {
            var result = await _productService.CreateProdukt(new CreateProductDto
            {
                CategoryName = CreateProductModel.CategoryName,
                SubCategoryName = CreateProductModel.SubCategoryName,
                ArticleNumber = CreateProductModel.ArticleNumber,
                ProductTitle = CreateProductModel.ProductTitle,
                Ingress = CreateProductModel.Ingress,
                Description = CreateProductModel.Description,
                Specification = CreateProductModel.Specification,
                Manufacture = CreateProductModel.Manufacture,
                Price = CreateProductModel.Price,

            });
            if (result)
            {
                await Shell.Current.GoToAsync("..");
            }
        }
        else
        {
            await Shell.Current.DisplayAlert
             ("Something went wrong!",
             "Category is required\n" +
             "Subcategory is required\n" +
             "Articlenumber is required\n" +
             "Product title is required\n" +
             "Ingress is required\n" +
             "Description is required\n" +
             "Specification is required\n" +
             "Manufacture is required\n" +
             "Price is required",
             "Ok");
        }
    }
}
