using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.MAUI.Mvvm.Models.ProductModels
{
    public class ProductModel
    {
        public string ArticleNumber { get; set; } = null!;
        public string ProductTitle { get; set; } = null!;
        public string Ingress {  get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Specification { get; set; } = null!;
        public string Manufacture { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public string SubCategoryName { get; set; } = null!;
    }
}
