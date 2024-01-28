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

    [RelayCommand]
    async Task AddProduct()
    {
        if (!string.IsNullOrWhiteSpace(CreateProductModel.ArticleNumber) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Title) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Description) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Specification) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.Manufacture) &&
            !string.IsNullOrWhiteSpace(CreateProductModel.CategoryName))

        {
            var createProductDto = new CreateProductDto
            {
                ArticleNumber = CreateProductModel.ArticleNumber,
                Title = CreateProductModel.Title,
                Description = CreateProductModel.Description,
                Specification = CreateProductModel.Specification,
                ManufactureName = CreateProductModel.Manufacture,
                CategoryName = CreateProductModel.CategoryName,
                Price = CreateProductModel.Price,
                SubCategoryName = CreateProductModel.SubCategoryName,
            };


            var result = await _productService.CreateProdukt(createProductDto);
            if(result)
            {
                
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
