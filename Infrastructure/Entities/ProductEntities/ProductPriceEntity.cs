using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ProductPriceEntity
{
    public string ArticleNumber { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;
}
