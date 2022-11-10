using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Usuarios
{
    public class UsuarioEditarDTO
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
        public string Password { get; set; }
    }
}
