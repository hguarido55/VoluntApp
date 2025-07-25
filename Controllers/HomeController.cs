using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VoluntApp.Models;

namespace VoluntApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult IndexAdmin()
        {
            // Carga de ejemplo; aquí deberías obtenerlo de la base de datos
            var actividades = new List<Voluntariado>(); // Sustituir por carga real

            return View(actividades);
        }

    }
}
