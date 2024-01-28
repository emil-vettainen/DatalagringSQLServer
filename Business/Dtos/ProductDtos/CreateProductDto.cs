namespace Business.Dtos.ProductDtos;

public class CreateProductDto
{
    public string ArticleNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Specification { get; set; } = null!;
    public string ManufactureName {  get; set; } = null!;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
    public string SubCategoryName { get; set; } = null!;

}
