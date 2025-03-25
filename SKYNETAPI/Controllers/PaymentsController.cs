using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;

namespace SKYNETAPI.Controllers;


public class PaymentsController(IPaymentService paymentService, IUnitOfWork unitOfWork) : BaseApiController
{
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

}
