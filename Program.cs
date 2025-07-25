using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios (ANTES de builder.Build())
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // <-- Agregado correctamente aquí
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
    });

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // <-- DEBE ir antes de authentication si se usa en login
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Conexión MySQL (sólo inicialización/test)
string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=voluntapp;";
MySqlConnection databaseConn = new MySqlConnection(connectionString);

try
{
    databaseConn.Open();
    Console.WriteLine("Conexión establecida.");
}
catch (MySqlException ex)
{
    Console.WriteLine($"Error al conectar: {ex.Message}");
}
finally
{
    if (databaseConn.State == System.Data.ConnectionState.Open)
    {
        databaseConn.Close();
        Console.WriteLine("Conexión cerrada.");
    }
}




