using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string NameProduct { get; set; } = null!;

    public string? DescriptionProduct { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
