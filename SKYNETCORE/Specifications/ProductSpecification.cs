using SKYNETCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Specifications;

// Clase que implementa una especificación específica para productos
public class ProductSpecification: BaseSpecification<Product>
{
    // Constructor que recibe los filtros de marca y tipo
    public ProductSpecification(string? brand, string? type, string? sort): base(
        x => (string.IsNullOrWhiteSpace(brand) || x.Brand == brand  ) && 
        (string.IsNullOrWhiteSpace(type) || x.Type == type )
        )
    {
        switch (sort)
        {
            case "priceAsc": 
                AddOrderBy(x=> x.Price); 
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;

            default:
                AddOrderBy(x => x.Name);
                break;
        }
        
    }
}
