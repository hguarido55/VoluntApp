using System.ComponentModel.DataAnnotations;

namespace VoluntApp.Models
{
    public class Organizacion
    {
        public int Id { get; set; }  // ID_ONG

        public string Nombre { get; set; }

        public int Telefono { get; set; }

        public float Latitud { get; set; }

        public float Longitud { get; set; }

        public ICollection<Voluntariado> Voluntariados { get; set; }  // Relación con Voluntariado
    }
}
