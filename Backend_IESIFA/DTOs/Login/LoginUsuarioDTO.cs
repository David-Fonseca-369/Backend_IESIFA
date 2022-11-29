using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Login
{
    public class LoginUsuarioDTO
    {
        [Required, EmailAddress]
        public string Correo { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
