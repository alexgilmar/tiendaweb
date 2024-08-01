using Microsoft.Extensions.Options;
using TiendaVirgenFatima.Hubs;
using TiendaVirgenFatima.Services;
using TiendaVirgenFatima.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<IMongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<ProductService>();

builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Añadir esto para API controllers
builder.Services.AddSignalR();

var app = builder.Build();

// Configurar middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Asegúrate de que esté habilitado
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Añadir esto para mapear los API controllers
app.MapHub<ProductHub>("/productHub");

app.Run();
