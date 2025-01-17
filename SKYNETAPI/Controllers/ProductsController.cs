using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKYNET_INFRASTRUCTURE.Data;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;

namespace SKYNETAPI.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductRepository _productRepository) : ControllerBase
{


    // products?type=boards
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] string? brand, [FromQuery] string? type, [FromQuery] string? sort)
    {
        var products = await _productRepository.GetProductsAsync(brand, type, sort);
        
        
        return Ok(products);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }


    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _productRepository.AddProduct(product);

        if (await _productRepository.SaveChangesAsync())
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);

         

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> UpdateProduct([FromRoute]Guid id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        _productRepository.UpdateProduct(product);

        if (await _productRepository.SaveChangesAsync())
            return Ok(product);

                    
        return BadRequest("Problem updating product");

    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
            

        if (product == null) return NotFound();

        _productRepository.DeleteProduct(product);

        if(await _productRepository.SaveChangesAsync())
            return Ok(product);


        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _productRepository.GetBrandsAsync();

        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        var types = await _productRepository.GetTypesAsync();

        return Ok(types);
    }

    private bool ProductExists(Guid id)
    {
        return _productRepository.ProductExists(id);
    }
}
