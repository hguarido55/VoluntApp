using System.ComponentModel.DataAnnotations;

namespace VoluntApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string DNI { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Hash_Contrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Hash_Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}

