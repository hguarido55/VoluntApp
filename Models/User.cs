using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoluntApp.Models
{
    public class User
    {
        public int DNI { get; set; }  // Clave primaria

        public int HorasRegistradas { get; set; }

        public int Telefono { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }

        public string HashContrasena { get; set; }

        public bool Disponibilidad { get; set; }

        public ICollection<Inscribe> Inscripciones { get; set; }  // Relación con Inscribe (actividades en las que se ha inscrito)
                                                                  // otras propiedades
        public List<Voluntariado> Actividades { get; set; } = new List<Voluntariado>();
    }
}
