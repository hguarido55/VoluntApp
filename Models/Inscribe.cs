namespace VoluntApp.Models
{
    public class Inscribe
    {
        public int DNI { get; set; }  // Clave foránea a Usuario

        public int IdVoluntariado { get; set; }  // Clave foránea a Voluntariado

        public string Estado { get; set; }

        public string Descripcion { get; set; }

        public User Usuario { get; set; }  // Relación con Usuario

        public Voluntariado Voluntariado { get; set; }  // Relación con Voluntariado
    }
}

