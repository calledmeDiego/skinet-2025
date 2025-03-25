using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNET_INFRASTRUCTURE.Data;

public class UnitOfWork(StoreContext context) : IUnitOfWork
{
    // Diccionario concurrente para almacenar repositorios genéricos y asegurar thread-safety.
    // - Key: Nombre de la entidad (ej: "Product", "Order").
    // - Value: Instancia del repositorio específico para esa entidad.
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        context.Dispose();
    }

    // Obtiene o crea un repositorio genérico para una entidad específica.
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;

        // Usa GetOrAdd para evitar crear múltiples instancias del mismo repositorio.
        return
(IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
{
    // Crea dinámicamente el tipo del repositorio genérico para TEntity.
    // Ejemplo: Si TEntity es Product, crea GenericRepository<Product>.
    var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));

    // Crea una instancia del repositorio usando reflection y le inyecta el DbContext.
    return Activator.CreateInstance(repositoryType, context) ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
});
    }
}
