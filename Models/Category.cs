using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Category
{
    public int Id { get; set; }

    public string NameCategory { get; set; } = null!;

    public string? DescriptionCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
