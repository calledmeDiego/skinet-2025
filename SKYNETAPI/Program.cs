// SERVICE  ARE THINGS THAT INYECT IN OTHER CLASSES

using Microsoft.EntityFrameworkCore;
using SKYNET_INFRASTRUCTURE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// MIDDLEWARE CONSIDER
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
