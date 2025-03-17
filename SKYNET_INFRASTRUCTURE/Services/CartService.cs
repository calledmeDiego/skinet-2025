using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKYNET_INFRASTRUCTURE.Services;

public class CartService(IConnectionMultiplexer redis) : ICartService
{
    //Interfaz para interactuar con Redis
    private readonly IDatabase _database = redis.GetDatabase();

    //Elimina el carrito de Redis usando su clave (ID).
    public async Task<bool> DeleteCartAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    //Obtiene el carrito desde Redis y lo deserializa (convierte de JSON a objeto).
    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _database.StringGetAsync(key);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart?>(data!);
    }

    //Serializa el carrito a JSON y lo guarda en Redis con expiración de 30 días.
    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var created = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));

        if (!created) return null;

        return await GetCartAsync(cart.Id);
    }
}
