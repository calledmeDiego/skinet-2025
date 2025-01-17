// SERVICE  ARE THINGS THAT INYECT IN OTHER CLASSES

using Microsoft.EntityFrameworkCore;
using SKYNET_INFRASTRUCTURE.Data;
using SKYNETCORE.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// si elegimos addscoped quiere decir que la solicitud vivira tantito tiempo como la solicitud http
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// MIDDLEWARE CONSIDER
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();

    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}

app.Run();
