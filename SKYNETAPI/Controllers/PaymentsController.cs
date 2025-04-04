using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SKYNETAPI.Extensions;
using SKYNETAPI.SignalR;
using SKYNETCORE.Entities;
using SKYNETCORE.Entities.OrderAggregate;
using SKYNETCORE.Interfaces;
using SKYNETCORE.Specifications;
using Stripe;

namespace SKYNETAPI.Controllers;


public class PaymentsController(IPaymentService paymentService, IUnitOfWork unitOfWork, ILogger<PaymentsController> logger, IConfiguration config, IHubContext<NotificationHub> hubContext) : BaseApiController
{
    private readonly string _whSecret = config["StripeSettings:WhSecret"]!;

    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<IActionResult> CreateOrUpdatePaymenteIntent(string cartId)
    {
        var cart = await paymentService.CreatOrUpdatePaymentIntent(cartId);

        if (cart == null) return BadRequest("Problem with your cart");

        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<IActionResult> GetDeliveryMethods()
    {
        return Ok(await unitOfWork.Repository<DeliveryMethod >().ListAllAsync());
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebHook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = ConstructStripeEvent(json);

            if (stripeEvent.Data.Object is not PaymentIntent intent)
            {
                return BadRequest("Invalid event data");
            }

            await HandlePaymentIntentSucceeded(intent);

            return Ok();
        }
        catch (StripeException ex)
        {

            logger.LogError(ex, "Stripe webhook error");
            return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
        }
        catch (Exception ex) {

            logger.LogError(ex, "An unexpected error occurred");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
    {
        if (intent.Status == "succeeded")
        {
            var spec = new OrderSpecification(intent.Id, true);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if (order == null) throw new Exception("Order not found");

            if ((long)order.GetTotal() * 100 != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMismatch;
            }
            else {
                order.Status = OrderStatus.PaymentReceived;
            }

            await unitOfWork.Complete();

            //TODO: SignalR
            var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);

            if (!string.IsNullOrEmpty(connectionId))
            {
                await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification",order.ToDto());
            }

        }
    }

    private Event ConstructStripeEvent(string json)
    {
        try
        {
            return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Failed to construct stripe event");
            throw new StripeException("Invalid signature");
        }
    }
}
