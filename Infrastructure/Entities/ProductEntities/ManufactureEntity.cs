using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ManufactureEntity
{
    public int Id { get; set; }

    public string ManufactureName { get; set; } = null!;

    public virtual ICollection<ProductEntity> ProductEntities { get; set; } = new List<ProductEntity>();
}
