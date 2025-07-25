using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VoluntApp.Models;

public class MapaController : Controller
{
    private readonly string _connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=voluntapp;";

    public async Task<IActionResult> Index()
    {
        List<Voluntariado> actividades = new List<Voluntariado>();

        using (var conn = new MySqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            string query = @"
                SELECT *
                FROM voluntariado
                WHERE Latitud IS NOT NULL AND Longitud IS NOT NULL
            ";

            using (var cmd = new MySqlCommand(query, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    actividades.Add(new Voluntariado
                    {
                        Id = reader.GetInt32("ID_Voluntariado"),
                        Tipo = reader.GetString("Tipo"),
                        Descripcion = reader.GetString("Descripcion"),
                        FechaInicio = reader.GetDateTime("Fecha_Inicio"),
                        Ubicacion = reader.GetString("Ubicacion"),
                        Latitud = reader.GetFloat("Latitud"),
                        Longitud = reader.GetFloat("Longitud")
                    });
                }
            }
        }

        ViewBag.Actividades = actividades; // <- Aquí es donde pasas la lista al ViewBag

        return View(); // No necesitas pasar model explícitamente si usas ViewBag
    }
}





