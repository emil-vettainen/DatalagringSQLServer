﻿using Business.Dtos.ProductDtos;
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
        LoadProductsAsync().ConfigureAwait(false);

        _productService.UpdateProductList += (sender, e) =>
        {
            LoadProductsAsync().ConfigureAwait(false);
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


    [RelayCommand]
    async Task GoToProductDetail(string articleNumber)
    {
        if (articleNumber != null)
        {
            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?ArticleNumber={articleNumber}");
        }
    }


    [ObservableProperty]
    ObservableCollection<ProductGroup> _groupedProductList = [];



    //public async Task LoadProducts()
    //{
    //    ProductsList.Clear();

    //    var productDtos = await _productService.GetAllProductsAsync();
    //    foreach (var dto in productDtos)
    //    {
    //        var model = new ProductModel
    //        {
    //            ArticleNumber = dto.ArticleNumber,
    //            Title = dto.Title,
    //            Description = dto.Description,
    //            CategoryName = dto.CategoryName,
    //            SubCategoryName = dto.SubCategoryName,
    //            Manufacture = dto.ManufactureName,
    //            Specification = dto.Specification,
    //            Price = dto.Price,


    //        };


    //        ProductsList.Add(model);
    //    }

    //}

    [ObservableProperty]
    ProductGroup _group = [];

    public async Task LoadProductsAsync()
    {
        var productDtos = await _productService.GetAllProductsAsync();

        // Gruppera produkter efter kategori
        var groupedData = productDtos
            .GroupBy(p => p.CategoryName)
            .Select(group =>
            {
                var productGroup = new ProductGroup
                {
                    CategoryName = group.Key
                };

                foreach (var dto in group)
                {
                    productGroup.Add(new ProductModel
                    {
                        ArticleNumber = dto.ArticleNumber,
                        Title = dto.Title,
                        Description = dto.Description,
                        CategoryName = dto.CategoryName,
                        SubCategoryName = dto.SubCategoryName,
                        Manufacture = dto.ManufactureName,
                        Specification = dto.Specification,
                        Price = dto.Price,

                    });
                }

                return productGroup;
            });

        GroupedProductList = new ObservableCollection<ProductGroup>(groupedData);
    }



}
