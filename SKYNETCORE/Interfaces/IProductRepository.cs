using SKYNETCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Interfaces;

public interface IProductRepository
{
    // SOLO DE LECTURA, NO MANIPULAR
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetTypesAsync();


    // SINCRONOS PORQUE NO INTERACTUAN CON LA BASE DE DATOS
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(Guid id);
    Task<bool> SaveChangesAsync();
}
