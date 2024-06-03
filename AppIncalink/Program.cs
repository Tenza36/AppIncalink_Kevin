using QuestPDF.Infrastructure;
using AppIncalink.Datos;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<actividadesDatos>();
builder.Services.AddScoped<grupoDatos>();
builder.Services.AddTransient<actividadesDatos>();
builder.Services.AddTransient<reportesDatos>();

// Configurar QuestPDF
QuestPDF.Settings.License = LicenseType.Community; // Usa LicenseType.Commercial si tienes una licencia comercial

// Agregar servicio de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Usar middleware de sesión
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
