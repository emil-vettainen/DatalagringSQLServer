using CommunityToolkit.Mvvm.ComponentModel;

namespace Presentation.MAUI.Mvvm.ViewModels;

[QueryProperty(nameof(ArticleNumber), nameof(ArticleNumber))]
public partial class ProductDetailViewModel : ObservableObject
{
    [ObservableProperty]
    string? articleNumber;

}
