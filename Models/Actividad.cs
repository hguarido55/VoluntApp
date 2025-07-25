using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoluntApp.Models
{
    public class Voluntariado
    {
        [Key]
        [Column("ID_Voluntariado")]
        public int Id { get; set; }

        public int Plazas { get; set; }

        public string Descripcion { get; set; }

        public string Tipo { get; set; }

        public string Ubicacion { get; set; }

        [Column("Fecha_Inicio")]
        public DateTime FechaInicio { get; set; }

        [Column("Fecha_Fin")]
        public DateTime FechaFin { get; set; }

        public string Estado { get; set; }

        public float Latitud { get; set; }

        public float Longitud { get; set; }

        // Relaciones
        public ICollection<Inscribe> Inscripciones { get; set; }
        public Organizacion Organizacion { get; set; }
    }
}


