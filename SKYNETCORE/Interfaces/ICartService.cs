using SKYNETCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Interfaces;

public interface ICartService
{
    Task<ShoppingCart?> GetCartAsync(string key);
    Task<ShoppingCart?> SetCartAsync(ShoppingCart cart);
    Task<bool> DeleteCartAsync(string key);

}
