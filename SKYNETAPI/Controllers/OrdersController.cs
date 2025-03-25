using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKYNETAPI.DTOs;
using SKYNETAPI.Extensions;
using SKYNETCORE.Entities;
using SKYNETCORE.Entities.OrderAggregate;
using SKYNETCORE.Interfaces;
using SKYNETCORE.Specifications;

namespace SKYNETAPI.Controllers;

[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDTO orderDTO)
    {
        var email = User.GetEmail();

        var cart = await cartService.GetCartAsync(orderDTO.CartId);

        if (cart == null) return BadRequest("Cart not found");

        if (cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");

        var items = new List<OrderItem>();

        foreach (var item in cart.Items)
        {
            var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

            if(productItem == null) return BadRequest("Problem with the order!");

            var itemOrdered = new ProductItemOrdered
            {
                ProductId = item.ProductId,
                PictureUrl = item.PictureUrl,
                ProductName = item.ProductName,
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = item.Price,
                Quantity = item.Quantity,
            };

            items.Add(orderItem);
        }

        var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDTO.DeliveryMethodId);

        if (deliveryMethod == null) return BadRequest("No delivery method selected");

        var order = new Order
        {
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = orderDTO.ShippingAddress,
            Subtotal = items.Sum(x => x.Price * x.Quantity),
            PaymentSummary = orderDTO.PaymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email,
        };

        unitOfWork.Repository<Order>().Add(order);

        if (await unitOfWork.Complete())
        {
            return Ok(order);
        }
        return BadRequest("Problem creating order");
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersForUser()
    {
        var spec = new OrderSpecification(User.GetEmail());

        var orders = await unitOfWork.Repository<Order>().ListAsync(spec);

        var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

        return Ok(ordersToReturn);
    }
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var spec = new OrderSpecification(User.GetEmail(), id);

        var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return NotFound();

        var orderToReturn = order.ToDto();

        return Ok(orderToReturn);
    }
}
