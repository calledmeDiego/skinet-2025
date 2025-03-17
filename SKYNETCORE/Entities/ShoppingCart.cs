using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Entities;

public class ShoppingCart
{
    public string Id { get; set; }
    public List<CartItem> Items { get; set; } = [];

    public Guid? DeliveryMethodId { get; set; }
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
}
