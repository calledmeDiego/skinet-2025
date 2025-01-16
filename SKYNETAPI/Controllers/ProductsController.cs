using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKYNET_INFRASTRUCTURE.Data;
using SKYNETCORE.Entities;

namespace SKYNETAPI.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _storeContext;

    public ProductsController(StoreContext storeContext)
    {
        this._storeContext = storeContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        
        var products = await _storeContext.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Product>> GetProduct([FromRoute] Guid id)
    {
        var product = await _storeContext.Products.FindAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }


    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
         _storeContext.Products.Add(product);

        await _storeContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> UpdateProduct([FromRoute]Guid id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        _storeContext.Entry(product).State = EntityState.Modified;

        await _storeContext.SaveChangesAsync();

        return Ok(product);
        

    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var product = await _storeContext.Products.FindAsync(id);

        if (product == null) return NotFound();

        _storeContext.Products.Remove(product);

        await _storeContext.SaveChangesAsync();

        return Ok(product);
    }

    private bool ProductExists(Guid id)
    {
        return _storeContext.Products.Any(x => x.Id == id);
    }
}
