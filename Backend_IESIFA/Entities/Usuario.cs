using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public int IdRol { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength (60)]
        public string ApellidoMaterno { get; set; }
        [Required, MaxLength(60)]
        public string Correo { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public bool Estado { get; set; }

        //referencias
        public Rol Rol { get; set; }
    }
}
