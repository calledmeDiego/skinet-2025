using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Entities.OrderAggregate;

public class ProductItemOrdered
{
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }

}
