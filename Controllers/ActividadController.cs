using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using VoluntApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

public class ActividadController : Controller
{
    private readonly string _connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=voluntapp;";

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        ViewBag.UserId = userId;

        var actividades = new List<Voluntariado>();
        var actividadesInscritas = new HashSet<int>();

        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            // Obtener todas las actividades activas
            using (var cmd = new MySqlCommand("SELECT * FROM Voluntariado WHERE Estado = 'Activo'", conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    actividades.Add(new Voluntariado
                    {
                        Id = reader.GetInt32("ID_Voluntariado"),
                        Plazas = reader.GetInt32("Plazas"),
                        Descripcion = reader.GetString("Descripcion"),
                        Tipo = reader.GetString("Tipo"),
                        Ubicacion = reader.GetString("Ubicacion"),
                        FechaInicio = reader.GetDateTime("Fecha_Inicio"),
                        FechaFin = reader.GetDateTime("Fecha_Fin"),
                        Estado = reader.GetString("Estado"),
                        Latitud = reader.GetFloat("Latitud"),
                        Longitud = reader.GetFloat("Longitud")
                    });
                }
            }

            // Obtener actividades inscritas
            using (var inscribeCmd = new MySqlCommand("SELECT ID_Voluntariado FROM inscribe WHERE DNI = @DNI", conn))
            {
                inscribeCmd.Parameters.AddWithValue("@DNI", userId);
                using (var reader = await inscribeCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        actividadesInscritas.Add(reader.GetInt32("ID_Voluntariado"));
                    }
                }
            }
        }

        ViewBag.ActividadesInscritas = actividadesInscritas;

        if (User.IsInRole("organizacion") || User.IsInRole("admin"))
        {
            return View("IndexAdmin", actividades);
        }

        return View("Index", actividades);
    }

    [Authorize]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Crear(Voluntariado actividad)
    {
        if (ModelState.IsValid)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var query = "INSERT INTO voluntariado (Plazas, Descripcion, Tipo, Ubicacion, Fecha_Inicio, Fecha_Fin, Estado, Latitud, Longitud) " +
                            "VALUES (@Plazas, @Descripcion, @Tipo, @Ubicacion, @Fecha_Inicio, @Fecha_Fin, @Estado, @Latitud, @Longitud)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Plazas", actividad.Plazas);
                    cmd.Parameters.AddWithValue("@Descripcion", actividad.Descripcion);
                    cmd.Parameters.AddWithValue("@Tipo", actividad.Tipo);
                    cmd.Parameters.AddWithValue("@Ubicacion", actividad.Ubicacion);
                    cmd.Parameters.AddWithValue("@Fecha_Inicio", actividad.FechaInicio);
                    cmd.Parameters.AddWithValue("@Fecha_Fin", actividad.FechaFin);
                    cmd.Parameters.AddWithValue("@Estado", actividad.Estado);
                    cmd.Parameters.AddWithValue("@Latitud", actividad.Latitud);
                    cmd.Parameters.AddWithValue("@Longitud", actividad.Longitud);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(actividad);
    }

    [Authorize]
    public async Task<IActionResult> Editar(int id)
    {
        Voluntariado actividad = null;

        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var query = "SELECT * FROM Voluntariado WHERE ID_Voluntariado = @ID";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        actividad = new Voluntariado
                        {
                            Id = reader.GetInt32("ID_Voluntariado"),
                            Plazas = reader.GetInt32("Plazas"),
                            Descripcion = reader.GetString("Descripcion"),
                            Tipo = reader.GetString("Tipo"),
                            Ubicacion = reader.GetString("Ubicacion"),
                            FechaInicio = reader.GetDateTime("Fecha_Inicio"),
                            FechaFin = reader.GetDateTime("Fecha_Fin"),
                            Estado = reader.GetString("Estado"),
                            Latitud = reader.GetFloat("Latitud"),
                            Longitud = reader.GetFloat("Longitud")
                        };
                    }
                }
            }
        }

        if (actividad == null) return NotFound();

        return View(actividad);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Editar(Voluntariado actividad)
    {
        if (ModelState.IsValid)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var query = "UPDATE Voluntariado SET Plazas = @Plazas, Descripcion = @Descripcion, Tipo = @Tipo, " +
                            "Ubicacion = @Ubicacion, Fecha_Inicio = @Fecha_Inicio, Fecha_Fin = @Fecha_Fin, Estado = @Estado, " +
                            "Latitud = @Latitud, Longitud = @Longitud " +
                            "WHERE ID_Voluntariado = @ID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", actividad.Id);
                    cmd.Parameters.AddWithValue("@Plazas", actividad.Plazas);
                    cmd.Parameters.AddWithValue("@Descripcion", actividad.Descripcion);
                    cmd.Parameters.AddWithValue("@Tipo", actividad.Tipo);
                    cmd.Parameters.AddWithValue("@Ubicacion", actividad.Ubicacion);
                    cmd.Parameters.AddWithValue("@Fecha_Inicio", actividad.FechaInicio);
                    cmd.Parameters.AddWithValue("@Fecha_Fin", actividad.FechaFin);
                    cmd.Parameters.AddWithValue("@Estado", actividad.Estado);
                    cmd.Parameters.AddWithValue("@Latitud", actividad.Latitud);
                    cmd.Parameters.AddWithValue("@Longitud", actividad.Longitud);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(actividad);
    }

    [Authorize(Roles = "organizacion,admin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var query = "DELETE FROM Voluntariado WHERE ID_Voluntariado = @ID";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Inscribirse(int id, string Estado, string Descripcion)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM inscribe WHERE DNI = @uid AND ID_Voluntariado = @vid", conn);
            checkCmd.Parameters.AddWithValue("@uid", userId);
            checkCmd.Parameters.AddWithValue("@vid", id);

            var exists = Convert.ToInt32(await checkCmd.ExecuteScalarAsync()) > 0;

            if (!exists)
            {
                var cmd = new MySqlCommand("INSERT INTO inscribe (DNI, ID_Voluntariado, Estado, Descripcion) VALUES (@uid, @vid, @est, @desc)", conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@vid", id);
                cmd.Parameters.AddWithValue("@est", Estado);
                cmd.Parameters.AddWithValue("@desc", Descripcion);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        return RedirectToAction("Index", "Perfil");
    }
}






