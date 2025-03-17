using SKYNETCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Interfaces;

public interface IPaymentService
{
    Task<ShoppingCart?> CreatOrUpdatePaymentIntent(string cartId);
}
