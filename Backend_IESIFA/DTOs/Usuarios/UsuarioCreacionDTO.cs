using Backend_IESIFA.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Usuarios
{
    public class UsuarioCreacionDTO
    {
        [Required]
        public int IdRol { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoMaterno { get; set; }
        [Required, MaxLength(60), EmailAddress]
        public string Correo { get; set; }
        [Required, MaxLength(60), MinLength(8, ErrorMessage = "La contraseña debe contener mínimo 8 caracteres.")]
        public string Password { get; set; }        
    }
}
