using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SKYNETCORE.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNET_INFRASTRUCTURE.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Configura la propiedad ShippingAddress como un "Owned Entity" (entidad propiedad).
        // Esto significa que ShippingAddress no es una tabla separada, sino que se almacena dentro de la tabla Order.
        builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner());
        // [ShippingAddress] se guarda en columnas como: ShippingAddress_Line1, ShippingAddress_City, etc.

        // Configura PaymentSummary como otro "Owned Entity" dentro de Order.
        builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());
        // Similar a ShippingAddress, sus propiedades se guardan dentro de la tabla Order.

        // Configura cómo se mapea el enum OrderStatus a la base de datos:
        // - Al guardar: Convierte el enum a su representación en string (ej: "Pending").
        // - Al leer: Convierte el string de la BD al enum correspondiente.
        builder.Property(x => x.Status)
            .HasConversion(
                o => o.ToString(),               // Convertidor al guardar (enum → string)
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o) // Convertidor al leer (string → enum)
            );

        // Define el tipo de columna en la BD para Subtotal como decimal(18,2):
        // - 18 dígitos en total, 2 decimales (ej: 1234567890123456.99).
        builder.Property(x => x.Subtotal)
            .HasColumnType("decimal(18,2)");

        // Define la relación entre Order y OrderItems (uno a muchos):
        // - Una Order puede tener muchos OrderItems.
        // - Al eliminar una Order, se eliminan automáticamente todos sus OrderItems (DeleteBehavior.Cascade).
        builder.HasMany(x => x.OrderItems)    // Order tiene una colección de OrderItems
            .WithOne()                        // OrderItem tiene una referencia a Order (sin navegación explícita)
            .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada
         builder.Property(x => x.OrderDate).HasConversion(
        // Conversión al guardar en la base de datos
                d => d.ToUniversalTime(),

        // Conversión al leer desde la base de datos
                d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
            );
        builder.Property(x => x.OrderNumber).UseIdentityColumn(500,1).ValueGeneratedOnAdd();
    }
}
