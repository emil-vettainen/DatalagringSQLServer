namespace Business.Dtos.ProductDtos;

public class CreateProductDto
{
    public string ArticleNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Specification { get; set; } = null!;
    public string Manufacture {  get; set; } = null!;
    public int ManufactureId { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;

}
