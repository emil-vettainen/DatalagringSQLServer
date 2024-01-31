using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ProductInfoEntity
{
    public string ArticleNumber { get; set; } = null!;

    public string ProductTitle { get; set; } = null!;

    public string Ingress { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Specification { get; set; } = null!;

    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;
}
