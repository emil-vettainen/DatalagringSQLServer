using Business.Dtos.ProductDtos;
using Business.Services.ProductServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.MAUI.Mvvm.Models.ProductModels;
using Shared.Enums;

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

    [ObservableProperty]
    private ProductModel? _updateDetail;


    public async Task ShowEditProductDetails()
    {
        if (ArticleNumber != null)
        {
            var productDetail = await _productService.GetOneProductAsync(ArticleNumber);

            if (productDetail != null)
            {
                var detail = new ProductModel
                {
                    CategoryName = productDetail.CategoryName,
                    SubCategoryName = productDetail.SubCategoryName,
                    ArticleNumber = productDetail.ArticleNumber,
                    ProductTitle = productDetail.ProductTitle,
                    Ingress = productDetail.Ingess,
                    Description = productDetail.Description,
                    Specification = productDetail.Specification,
                    Manufacture = productDetail.Manufacture,
                    Price = productDetail.Price,
                };

                UpdateDetail = detail;
            }
        }
    }

    [RelayCommand]
    async Task UpdateProduct()
    {
        if (!string.IsNullOrWhiteSpace(UpdateDetail.CategoryName) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.SubCategoryName) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.ArticleNumber) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.ProductTitle) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.Ingress) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.Description) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.Specification))
           
        {

                var result = await _productService.UpdateProductAsync(new ProductDto
                {
                    CategoryName = UpdateDetail.CategoryName,
                    SubCategoryName = UpdateDetail.SubCategoryName,
                    ArticleNumber = UpdateDetail.ArticleNumber,
                    ProductTitle = UpdateDetail.ProductTitle,
                    Ingess = UpdateDetail.Ingress,
                    Description = UpdateDetail.Description,
                    Specification = UpdateDetail.Specification,
                    Manufacture = UpdateDetail.Manufacture,
                    Price = UpdateDetail.Price,

                });

                switch (result.Status)
                {
                    case ResultStatus.Updated:
                       

                        await Shell.Current.DisplayAlert("Updated!", "Product was updated", "Ok");
                       
                        break;



                    default:
                        
                        await Shell.Current.DisplayAlert("Something went wrong!", "Please try again", "Ok");
                        break;
                }
            
        }
    }


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
