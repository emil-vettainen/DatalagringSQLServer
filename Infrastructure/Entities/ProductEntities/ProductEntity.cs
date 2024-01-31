using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ProductEntity
{
    public string ArticleNumber { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int ManufactureId { get; set; }

    public int CategoryId { get; set; }

    public virtual CategoryEntity Category { get; set; } = null!;

    public virtual ManufactureEntity Manufacture { get; set; } = null!;

    public virtual ProductInfoEntity ProductInfoEntity { get; set; } = null!;

    public virtual ProductPriceEntity ProductPriceEntity { get; set; } = null!;
}
