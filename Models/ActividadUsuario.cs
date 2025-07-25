namespace VoluntApp.Models
{
    public class ActividadUsuario
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ActividadId { get; set; }

        public Voluntariado Actividad { get; set; }
    }
}
