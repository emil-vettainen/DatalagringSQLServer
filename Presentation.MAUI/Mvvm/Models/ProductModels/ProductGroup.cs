using System.Collections.ObjectModel;

namespace Presentation.MAUI.Mvvm.Models.ProductModels;

public class ProductGroup : ObservableCollection<ProductModel>
{
    public string CategoryName { get; set; } = null!;
   
}
