using System;
using System.Collections.Generic;

namespace VoluntApp.Models
{
    public class PerfilViewModel
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int TotalHoras { get; set; }
        public User Usuario { get; set; }
        public List<Voluntariado> Actividades { get; set; } = new List<Voluntariado>();
    }

    public class ActividadViewModel
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Ubicacion { get; set; }
        public int Horas { get; set; }

    }
}

