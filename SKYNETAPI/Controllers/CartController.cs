using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;

namespace SKYNETAPI.Controllers;


public class CartController(ICartService cartService) : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetCartById(string id)
    {
        var cart = await cartService.GetCartAsync(id);
        return Ok(cart ?? new ShoppingCart { Id = id});
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart(ShoppingCart cart)
    {
        var updatedCart = await cartService.SetCartAsync(cart);

        if (updatedCart == null)
        {
            return BadRequest("Problem with cart");
        }

        return Ok(updatedCart);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCart(string id)
    {
        var result = await cartService.DeleteCartAsync(id);

        if (!result) return BadRequest("Problem deleting cart");

        return Ok(result);
    }

}
