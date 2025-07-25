using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using VoluntApp.Models;
using System.Data;

public class LoginController : Controller
{
    private readonly string _connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=voluntapp;";

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string email, string password)
    {
        if (email == "admin@gmail.com" && password == "Admin")
        {
            // Guardamos en sesión que es admin
            HttpContext.Session.SetString("Rol", "Admin");
            return View("IndexAdmin"); // Redirige a la vista especial para admins
        }

        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            string query = "SELECT * FROM usuario WHERE Email = @Email AND Hash_Contrasena = @Password";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Login correcto
                        HttpContext.Session.SetString("Email", email);
                        HttpContext.Session.SetInt32("DNI", reader.GetInt32("DNI"));
                        HttpContext.Session.SetString("Nombre", reader.GetString("Nombre"));

                        return RedirectToAction("Index", "Perfil");
                    }
                    else
                    {
                        ViewBag.Error = "Email o contraseña incorrectos.";
                        return View();
                    }
                }
            }
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}

