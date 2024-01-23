using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class CategoryEntity
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<CategoryEntity> InverseParentCategory { get; set; } = new List<CategoryEntity>();

    public virtual CategoryEntity? ParentCategory { get; set; }

    public virtual ICollection<ProductEntity> ArticleNumbers { get; set; } = new List<ProductEntity>();
}
