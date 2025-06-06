﻿using SKYNETCORE.Entities.OrderAggregate;
using SKYNETCORE.Entities;

namespace SKYNETAPI.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public Guid CodeName { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public required string BuyerEmail { get; set; }
    public required ShippingAddress ShippingAddress { get; set; }
    public decimal ShippingPrice { get; set; }
    public required string DeliveryMethod { get; set; }
    public required PaymentSummary PaymentSummary { get; set; }

    public required List<OrderItemDTO> OrderItems { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public required string Status { get; set; } 
    public required string PaymentIntentId { get; set; }
    
   
}
