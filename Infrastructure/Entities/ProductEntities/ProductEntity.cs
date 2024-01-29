using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ProductEntity
{
    public string ArticleNumber { get; set; } = null!;

    public string ProductTitle { get; set; } = null!;

    public string Ingress { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Specification { get; set; } = null!;

    public int ManufactureId { get; set; }

    public virtual ManufactureEntity Manufacture { get; set; } = null!;

    public virtual ProductPriceEntity? ProductPriceEntity { get; set; }

    public virtual ICollection<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
}
