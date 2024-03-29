﻿using Business.Dtos.ProductDtos;
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
                    ArticleNumber = productDetail.ArticleNumber,
                    ProductTitle = productDetail.ProductTitle,
                    Ingress = productDetail.Ingress,
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
        if (!string.IsNullOrWhiteSpace(UpdateDetail!.CategoryName) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.ProductTitle) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.Ingress) &&
            !string.IsNullOrWhiteSpace(UpdateDetail.Description) &&
            UpdateDetail.Price != 0 &&
            !string.IsNullOrWhiteSpace(UpdateDetail.Specification))

        {
            var result = await _productService.UpdateProductAsync(new ProductDto
            {
                ArticleNumber = UpdateDetail.ArticleNumber,
                CategoryName = UpdateDetail.CategoryName,
                ProductTitle = UpdateDetail.ProductTitle,
                Ingress = UpdateDetail.Ingress,
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
        else
        {
            await Shell.Current.DisplayAlert
           ("Something went wrong!",
           "Category is required\n" +
           "ProductTitle is required\n" +
           "Ingress is required\n" +
           "Description is required\n" +
           "Specification is required\n" +
           "Manufacture is required\n" +
           "Price must be bigger then 0\n",
           "Ok");
        }
    }

    [RelayCommand]
    async Task DeleteProduct()
    {
        if (ArticleNumber != null)
        {
            var answer = await Shell.Current.DisplayAlert("Warning!", "Are you sure you want to delete?\nThis action cannot be undone.", "Ok", "Cancel");
            if (answer)
            {
                var result = await _productService.DeleteProductByArticleNumberAsync(ArticleNumber);
                switch (result.Status)
                {
                    case ResultStatus.Deleted:
                        await Shell.Current.GoToAsync("../..");
                        break;

                    default:
                        await Shell.Current.DisplayAlert("Something went wrong!", "Please try again", "Ok");
                        break;
                }
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Something went wrong!", "Please try again", "Ok");
        }
    }
}