using SKYNETAPI.DTOs;
using SKYNETCORE.Entities.OrderAggregate;

namespace SKYNETAPI.Extensions;

public static class OrderMappingExtensions
{
    // EXTENSION DE METODO
    public static OrderDTO ToDto(this Order order)
    {
        return new OrderDTO
        {
            Id = order.OrderNumber,
            CodeName = order.Id,
            BuyerEmail = order.BuyerEmail,
            OrderDate = order.OrderDate,
            ShippingAddress = order.ShippingAddress,
            PaymentSummary = order.PaymentSummary,
            DeliveryMethod = order.DeliveryMethod.Description,
            ShippingPrice = order.DeliveryMethod.Price,
            OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
            Subtotal = order.Subtotal,
            Total = order.GetTotal(),
            Status = order.Status.ToString(),
            PaymentIntentId = order.PaymentIntentId,
        };
    }

    public static OrderItemDTO ToDto(this OrderItem orderItem)
    {
        return new OrderItemDTO
        {
            ProductId = orderItem.ItemOrdered.ProductId,
            ProductName = orderItem.ItemOrdered.ProductName,
            PictureUrl = orderItem.ItemOrdered.PictureUrl,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity,
        };
    }
}
