using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dtos.ProductDtos
{
    public class ProductDto
    {
        public string CategoryName { get; set; } = null!;
        public string SubCategoryName { get; set; } = null!;
        public string ArticleNumber { get; set; } = null!;
        public string ProductTitle { get; set; } = null!;
        public string Ingess {  get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Specification { get; set; } = null!;
        public string Manufacture { get; set; } = null!;
        public decimal Price { get; set; }
    
    }
}
