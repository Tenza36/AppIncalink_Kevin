using QuestPDF.Infrastructure;
using AppIncalink.Datos;


var builder = WebApplication.CreateBuilder(args);

 //Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<actividadesDatos>();
builder.Services.AddScoped<grupoDatos>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<actividadesDatos>();
// Configurar QuestPDF
QuestPDF.Settings.License = LicenseType.Community; // Usa LicenseType.Commercial si tienes una licencia comercial

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=index}/{id?}");
//pattern: "{controller=calendario}/{action=Calendario}/{id?}");

app.Run();
