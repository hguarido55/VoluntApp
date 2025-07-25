using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using MySql.Data.MySqlClient;
using VoluntApp.Models;

[Authorize]
public class PerfilController : Controller
{
    private readonly string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=voluntapp;";

    public IActionResult Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var usuario = new User();
        var actividades = new List<Voluntariado>();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Obtener datos del usuario
            var userQuery = "SELECT * FROM usuario WHERE DNI = @id";
            using (var cmd = new MySqlCommand(userQuery, connection))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario.Nombre = reader.GetString("Nombre");
                        usuario.Email = reader.GetString("Email");
                        usuario.HorasRegistradas = reader.GetInt32("Horas_Registradas");
                    }
                }
            }

            // Obtener actividades inscritas por el usuario desde la tabla Inscribe
            var actividadesQuery = @"
                SELECT v.*
                FROM inscribe i
                JOIN voluntariado v ON v.ID_Voluntariado = i.ID_Voluntariado
                WHERE i.DNI = @id";

            using (var cmd = new MySqlCommand(actividadesQuery, connection))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actividades.Add(new Voluntariado
                        {
                            Id = reader.GetInt32("ID_Voluntariado"),
                            Tipo = reader.GetString("Tipo"),
                            FechaInicio = reader.GetDateTime("Fecha_Inicio"),
                            FechaFin = reader.GetDateTime("Fecha_Fin"),
                            Ubicacion = reader.GetString("Ubicacion"),
                            Plazas = reader.GetInt32("Plazas"),
                            Descripcion = reader.GetString("Descripcion"),
                            Estado = reader.GetString("Estado")
                        });
                    }
                }
            }
        }

        var viewModel = new PerfilViewModel
        {
            Usuario = usuario,
            Actividades = actividades,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            TotalHoras = usuario.HorasRegistradas
        };

        return View(viewModel);
    }

    public IActionResult GenerarPDF()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string nombre = "", email = "";
        int horas = 0;
        var actividades = new List<(string Titulo, string Fecha)>();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var userQuery = "SELECT Nombre, Email, Horas_Registradas FROM usuario WHERE DNI = @id";
            using (var cmd = new MySqlCommand(userQuery, connection))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nombre = reader.GetString("Nombre");
                        email = reader.GetString("Email");
                        horas = reader.GetInt32("Horas_Registradas");
                    }
                }
            }

            var actividadesQuery = @"
                SELECT v.Tipo, v.Fecha_Inicio
                FROM inscribe i
                JOIN voluntariado v ON v.ID_Voluntariado = i.ID_Voluntariado
                WHERE i.DNI = @id";

            using (var cmd = new MySqlCommand(actividadesQuery, connection))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actividades.Add((
                            reader.GetString("Tipo"),
                            reader.GetDateTime("Fecha_Inicio").ToString("yyyy-MM-dd")
                        ));
                    }
                }
            }
        }

        // Generar PDF
        var doc = new PdfDocument();
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Verdana", 14, XFontStyle.Regular);

        gfx.DrawString($"Reporte de Voluntariado de {nombre}", font, XBrushes.Black, new XPoint(40, 40));
        gfx.DrawString($"Email: {email}", font, XBrushes.Black, new XPoint(40, 70));
        gfx.DrawString($"Horas Totales: {horas}", font, XBrushes.Black, new XPoint(40, 100));

        gfx.DrawString($"Actividades inscritas:", font, XBrushes.Black, new XPoint(40, 140));
        int y = 170;
        foreach (var act in actividades)
        {
            gfx.DrawString($"- {act.Titulo} ({act.Fecha})", font, XBrushes.Black, new XPoint(60, y));
            y += 25;
        }

        using (var stream = new MemoryStream())
        {
            doc.Save(stream, false);
            return File(stream.ToArray(), "application/pdf", "reporte_voluntariado.pdf");
        }
    }
}


