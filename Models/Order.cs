using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Order
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
