using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using VoluntApp.Models;
using MySql.Data.MySqlClient;

namespace VoluntApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=voluntapp;";

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO usuario (DNI, Nombre, Telefono, Email, Hash_Contrasena, Horas_Registradas)
                                     VALUES (@DNI, @Nombre, @Telefono, @Email, @Hash_Contrasena, 0)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@DNI", model.DNI);
                        cmd.Parameters.AddWithValue("@Nombre", model.Nombre);
                        cmd.Parameters.AddWithValue("@Telefono", model.Telefono);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Hash_Contrasena", model.Hash_Contrasena); // Mejor encriptar

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // ADMIN HARDCODEADO
            if (email == "admin@gmail.com" && password == "Admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Administrador"),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
                HttpContext.Session.SetString("Rol", "Admin");

                return RedirectToAction("IndexAdmin", "Home");
            }

            // USUARIO NORMAL
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM usuario WHERE Email = @Email AND Hash_Contrasena = @Password";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, reader["Nombre"].ToString()),
                                new Claim(ClaimTypes.Email, reader["Email"].ToString()),
                                new Claim(ClaimTypes.NameIdentifier, reader["DNI"].ToString()),
                                new Claim(ClaimTypes.Role, "Voluntario")
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
                            HttpContext.Session.SetString("Rol", "Voluntario");

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            // Si no hay coincidencia
            ModelState.AddModelError("", "Credenciales inválidas");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // Limpia sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

