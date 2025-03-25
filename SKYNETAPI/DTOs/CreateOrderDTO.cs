using SKYNETCORE.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace SKYNETAPI.DTOs;

public class CreateOrderDTO
{
    [Required]
    public string CartId { get; set; } = string.Empty;
    [Required]
    public Guid DeliveryMethodId { get; set; }
    [Required]
    public ShippingAddress ShippingAddress { get; set; } = null!;
    [Required]
    public PaymentSummary PaymentSummary { get; set; } = null!;
}
