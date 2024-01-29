namespace Presentation.MAUI.Mvvm.Models.ProductModels;

public class CreateProductModel
{
    public string CategoryName { get; set; } = null!;
    public string SubCategoryName { get; set; } = null!;
    public string ArticleNumber { get; set; } = null!;
    public string ProductTitle { get; set; } = null!;
    public string Ingress { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Specification { get; set; } = null!;
    public string Manufacture { get; set; } = null!;
    public decimal Price { get; set; }
}
