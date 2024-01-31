using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class CategoryEntity
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<ProductEntity> ProductEntities { get; set; } = new List<ProductEntity>();
}
