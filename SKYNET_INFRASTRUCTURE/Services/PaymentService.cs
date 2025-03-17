using Microsoft.Extensions.Configuration;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNET_INFRASTRUCTURE.Services;

public class PaymentService(IConfiguration configuration, ICartService cartService, 
    IGenericRepository<SKYNETCORE.Entities.Product> productRepo,
    IGenericRepository<DeliveryMethod> dmRepo) : IPaymentService
{
    public async Task<ShoppingCart?> CreatOrUpdatePaymentIntent(string cartId)
    {
        // OBTIENE LA CONFIGURACION DE STRIPE
        StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

        // OBTIENE EL CARRITO DE COMPRA
        var cart = await cartService.GetCartAsync(cartId);

        if (cart == null) return null;
        // COSTO DE ENVIO
        var shippingPrice = 0m;

        if (cart.DeliveryMethodId.HasValue)
        {
            // OBTIENE EL DELIVERYMETHOD
            var deliveryMethod = await dmRepo.GetByIdAsync((Guid)cart.DeliveryMethodId);

            if(deliveryMethod == null) return null;
            //ACTUALIZA EL COSTO DE ENVIO
            shippingPrice = deliveryMethod.Price;
        }
        // Garantiza que los precios del carrito coincidan con los de la base de datos 
        foreach (var item in cart.Items)
        {
           
            var productItem = await productRepo.GetByIdAsync(item.ProductId);

            if (productItem == null) return null;

            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price; 
            }
        }

        //Crea y Actualiza la Intención de Pago en Stripe
        var service = new PaymentIntentService();
        PaymentIntent? intent = null;

        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };
            intent = await service.CreateAsync(options);

            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
            };

            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }

        // Almacena el PaymentIntentId y ClientSecret en el carrito para su uso posterior en el frontend.
        await cartService.SetCartAsync(cart);

        return cart;
    }
}
