﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKYNET_INFRASTRUCTURE.Data;
using SKYNETAPI.RequestHelpers;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;
using SKYNETCORE.Specifications;

namespace SKYNETAPI.Controllers; 


public class ProductsController(IGenericRepository<Product>  _productRepository) : BaseApiController
{


    // products?type=boards
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        // Crea especificación
        var specifications = new ProductSpecification(specParams);

        // Usa el repositorio para obtener productos que cumplen la especificación
        var results = await CreatePagedResult(_productRepository, specifications, specParams.PageIndex, specParams.PageSize);
        
        // Mostrar productos
        return results;
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }


    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _productRepository.Add(product);

        if (await _productRepository.SaveAllAsync())
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);

         

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> UpdateProduct([FromRoute]Guid id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        _productRepository.Update(product);

        if (await _productRepository.SaveAllAsync())
            return Ok(product);

                    
        return BadRequest("Problem updating product");

    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
            

        if (product == null) return NotFound();

        _productRepository.Remove(product);

        if(await _productRepository.SaveAllAsync())
            return Ok(product);


        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        //var brands = await _productRepository.GetBrandsAsync();
        var spec = new BrandListSpecification();
        var brands = await _productRepository.ListAsync(spec);

        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        //var types = await _productRepository.GetTypesAsync();
        var spec = new BrandListSpecification();
        var types = await _productRepository.ListAsync(spec);

        return Ok(types);

        
    }

    private bool ProductExists(Guid id)
    {
        return _productRepository.Exists(id);
    }
}
