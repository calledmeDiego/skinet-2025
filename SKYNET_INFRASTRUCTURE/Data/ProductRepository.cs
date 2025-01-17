using Microsoft.EntityFrameworkCore;
using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNET_INFRASTRUCTURE.Data;

public class ProductRepository(StoreContext storeContext) : IProductRepository
{
    

    public void AddProduct(Product product)
    {
        storeContext.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        storeContext.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await storeContext.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await storeContext.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var productsQuery = storeContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            productsQuery = productsQuery.Where(x => x.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            productsQuery = productsQuery.Where(x => x.Type == type);
        productsQuery = sort switch
        {
            "priceAsc" => productsQuery.OrderBy(x => x.Price),
            "priceDesc" => productsQuery.OrderByDescending(x => x.Price),
            _ => productsQuery.OrderBy(x => x.Name)

        };

        return await productsQuery.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await storeContext.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(Guid id)
    {
        return storeContext.Products.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await storeContext.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        storeContext.Entry(product).State = EntityState.Modified;
    }
}
